using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace BinaryEditor_2
{
    public class MyHexBox : UserControl
    {
        // Длина строки
        private const int nStringLength = 16;

        // Количество строк в буфере
        private const int nStringCount = 29;

        // Базовый размер буфера
        private const int nStartBufferLength = nStringLength * nStringCount;

        // Буфер
        byte[] aBuffer = new byte[nStartBufferLength];

        private FileInfo FileInfo;

        private BinaryReader br;
        private VScrollBar vScrollBar1;

        // Для сложения строк
        StringBuilder sb = new StringBuilder(100000);

        public MyHexBox() : base()
        {
            InitializeComponent();
        }


        private void EndOldStream()
        {
            // Освобождаем ресурсы
            if (br != null)
            {
                br.BaseStream.Close();
                br.Dispose();
            }
        }

        // Открытие файла
        public void ReadFile(string fileName)
        {
            EndOldStream();

            FileInfo = new FileInfo(fileName);

            br = new BinaryReader(new BufferedStream(File.OpenRead(FileInfo.FullName), 1024 * 1024));

            // Ставим начальное значение
            vScrollBar1.Value = 0;

            // Максимальное значение исходя из количества строчек
            vScrollBar1.Maximum = (int)(FileInfo.Length / nStringLength);

            // Малое изменение - строка
            vScrollBar1.SmallChange = 1;

            // Большое изменение - страница
            vScrollBar1.LargeChange = nStringCount;


            ReadNextPage();
        }

        private void ReadNextPage()
        {
            long nNewStreamPosition = br.BaseStream.Position + nStartBufferLength;

            // Хватит ли стартового размера буфера для чтения информации
            if (nNewStreamPosition > FileInfo.Length)
            {
                // Это уже конец файла?
                if (br.BaseStream.Position == FileInfo.Length)
                {
                    MessageBox.Show("Невозможно загрузить следующую страницу. Вы находитесь в конце файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    br.BaseStream.Position -= aBuffer.Length;
                }
                else
                {
                    // Изменяем размер буфера
                    Array.Resize(ref aBuffer, (int)(FileInfo.Length - br.BaseStream.Position));
                }
            }
            // Если размер буфера был изменён ранее
            else if (aBuffer.Length != nStartBufferLength)
                Array.Resize(ref aBuffer, nStartBufferLength);

            // Считываем данные
            Text = ReadPage();

            // Нарисовать текст
            Invalidate();
        }

        // Метод перехода на предыдущую страницу
        private void ReadPrevPage()
        {
            // Проверяем новую позицию потока чтения
            long nNewStreamPosition = br.BaseStream.Position - (aBuffer.Length + nStartBufferLength);

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
            Text = ReadPage();

            // Нарисовать текст
            Invalidate();
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

            Text = sb.ToString();

            // Возвращаем буфер, конвертированный в строку
            return Text;
        }

        // Считываем строку
        private void ReadString(StringBuilder sbMini, int nCurrent, int nRemain)
        {
            // Размер буфера чтения
            int nBufferSize = nStringLength;

            // Если до конца буфера осталось меньше 16 символов - изменяем размер буфера строки
            if (nRemain < nStringLength)
                nBufferSize = nRemain;

            // Записываем номер
            long lStrNumber = br.BaseStream.Position - nRemain;

            Text = Convert.ToString(lStrNumber, 16);

            Text = Text.PadLeft(8, '0');

            sbMini.Append(Text);

            // Добавляем разделитель
            sbMini.Append(" | ");

            // Добавляем байты
            Text = BitConverter.ToString(aBuffer, nCurrent, nBufferSize);
            Text = Text.Replace('-', ' ');
            sbMini.Append(Text);

            // Добавляем разделитель
            sbMini.Append(" | ");

            // Получаем строку 
            Text = Encoding.Default.GetString(aBuffer, nCurrent, nBufferSize);

            // Добавляем текст
            sbMini.Append(Text);

            // Преобразуем считанный объединитель строк в строку
            Text = sbMini.ToString();

            // Очищаем строку от непечатных символов
            for (int j = 0; j < Text.Length; j++)
            {
                if (char.IsControl(Text[j]))
                    Text = Text.Replace(Text[j], '.');
            }

            // Добавляем строку в общий соединитель строк
            sb.Append(Text);

            // Добавляем переход на следующую строку
            sb.AppendLine();

            // Очищаем соединитель строк для строки
            sbMini.Clear();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            StringFormat style = new StringFormat
            {
                Alignment = StringAlignment.Near
            };

            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, style);
        }

        // Метод обработки скроллинга
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (vScrollBar1.Maximum != 0)
            {
                // Положительное - вверх, отрицательное - вниз
                int del = e.Delta;

                if (del > 0)
                {
                    ReadPrevPage();
                }
                else
                {
                    ReadNextPage();
                }

                // Расчитываем положение ползунка
                double dPos = (double)(br.BaseStream.Position - aBuffer.Length) / (double)FileInfo.Length;
                vScrollBar1.Value = (int)(dPos * vScrollBar1.Maximum);
            }

        }

        private void InitializeComponent()
        {
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.LargeChange = 1;
            this.vScrollBar1.Location = new System.Drawing.Point(332, 0);
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 162);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // MyHexBox
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.vScrollBar1);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.Name = "MyHexBox";
            this.Size = new System.Drawing.Size(352, 162);
            this.ResumeLayout(false);

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            double dPos = (double)e.NewValue / (double)vScrollBar1.Maximum;
            br.BaseStream.Position = (long)(dPos * FileInfo.Length);
            ReadNextPage();
        }
    }
}
