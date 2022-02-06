namespace SteganographyGraduate
{
    partial class StegoForm
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
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabPage_encrypt = new System.Windows.Forms.TabPage();
            this.tabPage_decrypt = new System.Windows.Forms.TabPage();
            this.tabPage_check = new System.Windows.Forms.TabPage();
            this.tabPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.tabPage_encrypt);
            this.tabPages.Controls.Add(this.tabPage_decrypt);
            this.tabPages.Controls.Add(this.tabPage_check);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(501, 417);
            this.tabPages.TabIndex = 0;
            // 
            // tabPage_encrypt
            // 
            this.tabPage_encrypt.Location = new System.Drawing.Point(4, 22);
            this.tabPage_encrypt.Name = "tabPage_encrypt";
            this.tabPage_encrypt.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_encrypt.Size = new System.Drawing.Size(396, 245);
            this.tabPage_encrypt.TabIndex = 0;
            this.tabPage_encrypt.Text = "Внедрить";
            this.tabPage_encrypt.UseVisualStyleBackColor = true;
            // 
            // tabPage_decrypt
            // 
            this.tabPage_decrypt.Location = new System.Drawing.Point(4, 22);
            this.tabPage_decrypt.Name = "tabPage_decrypt";
            this.tabPage_decrypt.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_decrypt.Size = new System.Drawing.Size(493, 391);
            this.tabPage_decrypt.TabIndex = 1;
            this.tabPage_decrypt.Text = "Извлечь";
            this.tabPage_decrypt.UseVisualStyleBackColor = true;
            // 
            // tabPage_check
            // 
            this.tabPage_check.Location = new System.Drawing.Point(4, 22);
            this.tabPage_check.Name = "tabPage_check";
            this.tabPage_check.Size = new System.Drawing.Size(396, 245);
            this.tabPage_check.TabIndex = 2;
            this.tabPage_check.Text = "Проверить";
            this.tabPage_check.UseVisualStyleBackColor = true;
            // 
            // StegoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 417);
            this.Controls.Add(this.tabPages);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(517, 456);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(517, 456);
            this.Name = "StegoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Стеганосистема";
            this.tabPages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage tabPage_encrypt;
        private System.Windows.Forms.TabPage tabPage_decrypt;
        private System.Windows.Forms.TabPage tabPage_check;
    }
}

