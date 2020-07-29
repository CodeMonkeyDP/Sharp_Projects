namespace BinaryEditor
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainTextBox = new System.Windows.Forms.RichTextBox();
            this.scrollBar = new System.Windows.Forms.VScrollBar();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(800, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileButton
            // 
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenButton,
            this.CloseButton});
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(48, 20);
            this.FileButton.Text = "Файл";
            // 
            // OpenButton
            // 
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(153, 22);
            this.OpenButton.Text = "Открыть файл";
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(153, 22);
            this.CloseButton.Text = "Выход";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MainTextBox
            // 
            this.MainTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainTextBox.Location = new System.Drawing.Point(0, 27);
            this.MainTextBox.Name = "MainTextBox";
            this.MainTextBox.ReadOnly = true;
            this.MainTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.MainTextBox.Size = new System.Drawing.Size(768, 382);
            this.MainTextBox.TabIndex = 5;
            this.MainTextBox.Text = "";
            // 
            // scrollBar
            // 
            this.scrollBar.LargeChange = 2;
            this.scrollBar.Location = new System.Drawing.Point(771, 28);
            this.scrollBar.Maximum = 1;
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(17, 381);
            this.scrollBar.TabIndex = 6;
            this.scrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VScrollBar1_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 413);
            this.Controls.Add(this.scrollBar);
            this.Controls.Add(this.MainTextBox);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 452);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileButton;
        private System.Windows.Forms.ToolStripMenuItem OpenButton;
        private System.Windows.Forms.ToolStripMenuItem CloseButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.RichTextBox MainTextBox;
        private System.Windows.Forms.VScrollBar scrollBar;
    }
}

