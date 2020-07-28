using System;
using System.IO;
using System.Windows.Forms;

namespace BinaryEditor
{
    public partial class Form1 : Form
    {
        // Текущая страница
        Page page;

        FileInfo CInfo;

        public Form1()
        {
            InitializeComponent();
            page = new Page();
            MainTextBox.MouseWheel += new MouseEventHandler(this.OnMouseWheel);
            
        }

        // Метод обработки нажатия кнопки "Закрыть"
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Метод открытия файла
        private void OpenButton_Click(object sender, EventArgs e)
        {
            // Если принято решение считать файл - 
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Освобождаем предыдущий файл
                page.EndOldStream();

                // Получаем информацию о файле
                CInfo = new FileInfo(OpenFileDialog.FileName);

                // Устанавливаем значение максимума для ползунка
                scrollBar.Maximum = (int)(CInfo.Length/ page.StartBufferLength + 1);

                // Устанавливаем ползунок в начало
                scrollBar.Value = scrollBar.Minimum;

                // Считываем файл
                MainTextBox.Text = page.ReadFile(CInfo);
            }
        }

        // Метод обработки скроллинга
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            // Если в файле как минимум 1 страница
            if (scrollBar.Maximum != 1)
            {
                // Положительное - вверх, отрицательное - вниз
                int del = e.Delta;

                if (del > 0)
                {
                    MainTextBox.Text = page.ReadPrevPage();
                }
                else
                {
                    MainTextBox.Text = page.ReadNextPage();
                }

                // Расчитываем положение ползунка
                scrollBar.Value = (int)(page.PagePosition / page.StartBufferLength);
            }
        }

        // Метод обработки ручного перемещения ползунка
        private void VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            // Новое положение для чтения
            long lNewPagePosition = (long)e.NewValue * (long)page.StartBufferLength;
            page.PagePosition = lNewPagePosition;
            MainTextBox.Text = page.ReadNextPage();
        }
    }
}