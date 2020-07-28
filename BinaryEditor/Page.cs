using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Windows.Forms;
using System.Text;

namespace BinaryEditor
{
    public class Page
    {
        // Длина строки
        private const int nStringLength = 16;

        // Базовый размер буфера
        private const int nStartBufferLength = nStringLength * 29;

        // Информация о файле
        FileInfo CInfo;

        // Буфер
        byte[] aBuffer = new byte[nStartBufferLength];

        // Для считывания файлов
        BinaryReader br;

        // Для сложения строк
        StringBuilder sb = new StringBuilder(100000);

        // Строка для вывода
        string outString;

        public Page ()
        {

        }

        // Функция получения стандартного размера буфера
        public int StartBufferLength { get { return nStartBufferLength; } }

        // Функция получения и установки позиции потока чтения
        public long PagePosition { get { return br.BaseStream.Position - aBuffer.Length; } set { br.BaseStream.Position = value; } }

        // Метод освобождения ресурсов файла
        public void EndOldStream()
        {
            // Освобождаем ресурсы
            if (br != null)
            {
                br.BaseStream.Close();
                br.Dispose();
            }

            // Восстанавливаем размер буфера по умолчанию
            if (aBuffer.Length != nStartBufferLength)
                Array.Resize(ref aBuffer, nStartBufferLength);
        }

        // Метод считывания файла
        public string ReadFile(FileInfo info)
        {
            // Получаем информацию о файле
            CInfo = info;

            br = new BinaryReader(new BufferedStream(File.OpenRead(info.FullName), 1024 * 1024));

            // Считываем содержимое первой страницы
            return ReadNextPage();
        }

        // Метод считывания страницы
        private string ReadPage()
        {
            // Считываем данные в буфер
            aBuffer = br.ReadBytes(aBuffer.Length);

            // Очистка складывателя строк
            sb.Clear();

            // Считыватель строк для каждой строки
            StringBuilder sbMini = new StringBuilder();

            // Цикл по буферу
            for (int i = 0; i < aBuffer.Length; i++)
            {
                // Для каждого 16 символа - считываем начиная с него строку
                if (i % nStringLength == 0)
                {
                    int nRemain = aBuffer.Length - i;
                    ReadString(sbMini, i, nRemain);
                }
            }

            outString = sb.ToString();

            // Возвращаем буфер, конвертированный в строку
            return outString;
        }

        // Считываем строку
        private void ReadString(StringBuilder sbMini, int nCurrent, int nRemain)
        {
            // Размер буфера чтения
            int nBufferSize = nStringLength;

            // Если до конца буфера осталось меньше 16 символов - изменяем размер буфера строки
            if (nRemain < nStringLength)
                nBufferSize = nRemain;

            // Получаем строку 
            outString = Encoding.Default.GetString(aBuffer, nCurrent, nBufferSize);

            // Записываем номер
            sbMini.Append((br.BaseStream.Position - nRemain).ToString("D8"));

            // Добавляем разделитель
            sbMini.Append(" | ");

            // Добавляем байты
            sbMini.Append(BitConverter.ToString(aBuffer, nCurrent, nBufferSize));

            // Добавляем разделитель
            sbMini.Append(" | ");

            // Добавляем текст
            sbMini.Append(outString);

            // Преобразуем считанный объединитель строк в строку
            outString = sbMini.ToString();

            // Очищаем строку от непечатных символов
            for (int j = 0; j < outString.Length; j++)
            {
                if (char.IsControl(outString[j]))
                    outString = outString.Replace(outString[j], '.');
            }

            // Добавляем строку в общий соединитель строк
            sb.Append(outString);

            // Добавляем переход на следующую строку
            sb.AppendLine();

            // Очищаем соединитель строк для строки
            sbMini.Clear();
        }

        // Метод перехода на предыдущую страницу
        public string ReadPrevPage()
        {
            // Проверяем новую позицию потока чтения
            long nNewStreamPosition = br.BaseStream.Position - ( aBuffer.Length + nStartBufferLength );

            // Если позиция отрицательная, выводим ошибку
            if (nNewStreamPosition < 0)
            {
                MessageBox.Show("Невозможно загрузить предыдущую страницу. Вы находитесь в начале файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                br.BaseStream.Position = 0;
            }
            else
            {
                // Если буфер имеет нестандартную длину, изменяем его размер
                if (aBuffer.Length != nStartBufferLength)
                    Array.Resize(ref aBuffer, nStartBufferLength);

                br.BaseStream.Position = nNewStreamPosition;
            }

            // Считываем страницу
            return ReadPage();
        }

        // Метод перехода на следующую страницу
        public string ReadNextPage()
        {
            long nNewStreamPosition = br.BaseStream.Position + nStartBufferLength;

            // Хватит ли стартового размера буфера для чтения информации
            if (nNewStreamPosition > CInfo.Length)
            {
                // Это уже конец файла?
                if (br.BaseStream.Position == CInfo.Length)
                {
                    MessageBox.Show("Невозможно загрузить следующую страницу. Вы находитесь в конце файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    br.BaseStream.Position -= aBuffer.Length;
                }
                else
                {
                    // Изменяем размер буфера
                    Array.Resize(ref aBuffer, (int)(CInfo.Length - br.BaseStream.Position));
                }
            }
            // Если размер буфера был изменён ранее
            else if (aBuffer.Length != nStartBufferLength)
                Array.Resize(ref aBuffer, nStartBufferLength);

            // Считываем данные
            return ReadPage();
        }
    }
}
