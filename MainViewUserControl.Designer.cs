namespace SteganographyGraduate
{
    partial class MainViewUserControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_keyDescription = new System.Windows.Forms.Label();
            this.lbl_txtPathDescription = new System.Windows.Forms.Label();
            this.lbl_imgPathDescription = new System.Windows.Forms.Label();
            this.btn_mainAction = new System.Windows.Forms.Button();
            this.tb_key = new System.Windows.Forms.TextBox();
            this.tb_txtPath = new System.Windows.Forms.TextBox();
            this.tb_imgPath = new System.Windows.Forms.TextBox();
            this.btn_openTxtFileDialog = new System.Windows.Forms.Button();
            this.btn_openImgFileDialog = new System.Windows.Forms.Button();
            this.panel_choice = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.rBtn_LSB = new System.Windows.Forms.RadioButton();
            this.rBtn_SimpleRandom = new System.Windows.Forms.RadioButton();
            this.rBtn_Simple = new System.Windows.Forms.RadioButton();
            this.panel_choice.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_keyDescription
            // 
            this.lbl_keyDescription.AutoSize = true;
            this.lbl_keyDescription.Location = new System.Drawing.Point(3, 286);
            this.lbl_keyDescription.Name = "lbl_keyDescription";
            this.lbl_keyDescription.Size = new System.Drawing.Size(80, 13);
            this.lbl_keyDescription.TabIndex = 19;
            this.lbl_keyDescription.Text = "Укажите ключ";
            // 
            // lbl_txtPathDescription
            // 
            this.lbl_txtPathDescription.AutoSize = true;
            this.lbl_txtPathDescription.Location = new System.Drawing.Point(3, 220);
            this.lbl_txtPathDescription.Name = "lbl_txtPathDescription";
            this.lbl_txtPathDescription.Size = new System.Drawing.Size(141, 13);
            this.lbl_txtPathDescription.TabIndex = 18;
            this.lbl_txtPathDescription.Text = "Укажите файл-сообщение";
            // 
            // lbl_imgPathDescription
            // 
            this.lbl_imgPathDescription.AutoSize = true;
            this.lbl_imgPathDescription.Location = new System.Drawing.Point(3, 142);
            this.lbl_imgPathDescription.Name = "lbl_imgPathDescription";
            this.lbl_imgPathDescription.Size = new System.Drawing.Size(137, 13);
            this.lbl_imgPathDescription.TabIndex = 17;
            this.lbl_imgPathDescription.Text = "Укажите файл-контейнер";
            // 
            // btn_mainAction
            // 
            this.btn_mainAction.Location = new System.Drawing.Point(394, 352);
            this.btn_mainAction.Name = "btn_mainAction";
            this.btn_mainAction.Size = new System.Drawing.Size(75, 23);
            this.btn_mainAction.TabIndex = 16;
            this.btn_mainAction.Text = "button4";
            this.btn_mainAction.UseVisualStyleBackColor = true;
            this.btn_mainAction.Click += new System.EventHandler(this.btn_mainAction_Click);
            // 
            // tb_key
            // 
            this.tb_key.Location = new System.Drawing.Point(3, 302);
            this.tb_key.Name = "tb_key";
            this.tb_key.Size = new System.Drawing.Size(430, 20);
            this.tb_key.TabIndex = 14;
            // 
            // tb_txtPath
            // 
            this.tb_txtPath.Location = new System.Drawing.Point(3, 236);
            this.tb_txtPath.Name = "tb_txtPath";
            this.tb_txtPath.ReadOnly = true;
            this.tb_txtPath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tb_txtPath.Size = new System.Drawing.Size(430, 20);
            this.tb_txtPath.TabIndex = 13;
            // 
            // tb_imgPath
            // 
            this.tb_imgPath.Location = new System.Drawing.Point(3, 161);
            this.tb_imgPath.Name = "tb_imgPath";
            this.tb_imgPath.ReadOnly = true;
            this.tb_imgPath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tb_imgPath.Size = new System.Drawing.Size(430, 20);
            this.tb_imgPath.TabIndex = 12;
            // 
            // btn_openTxtFileDialog
            // 
            this.btn_openTxtFileDialog.Location = new System.Drawing.Point(394, 207);
            this.btn_openTxtFileDialog.Name = "btn_openTxtFileDialog";
            this.btn_openTxtFileDialog.Size = new System.Drawing.Size(75, 23);
            this.btn_openTxtFileDialog.TabIndex = 11;
            this.btn_openTxtFileDialog.Text = "button2";
            this.btn_openTxtFileDialog.UseVisualStyleBackColor = true;
            this.btn_openTxtFileDialog.Click += new System.EventHandler(this.btn_openTxtFileDialog_Click);
            // 
            // btn_openImgFileDialog
            // 
            this.btn_openImgFileDialog.Location = new System.Drawing.Point(394, 132);
            this.btn_openImgFileDialog.Name = "btn_openImgFileDialog";
            this.btn_openImgFileDialog.Size = new System.Drawing.Size(75, 23);
            this.btn_openImgFileDialog.TabIndex = 10;
            this.btn_openImgFileDialog.Text = "button1";
            this.btn_openImgFileDialog.UseVisualStyleBackColor = true;
            this.btn_openImgFileDialog.Click += new System.EventHandler(this.btn_openImgFileDialog_Click);
            // 
            // panel_choice
            // 
            this.panel_choice.Controls.Add(this.radioButton4);
            this.panel_choice.Controls.Add(this.rBtn_LSB);
            this.panel_choice.Controls.Add(this.rBtn_SimpleRandom);
            this.panel_choice.Controls.Add(this.rBtn_Simple);
            this.panel_choice.Location = new System.Drawing.Point(6, 3);
            this.panel_choice.Name = "panel_choice";
            this.panel_choice.Size = new System.Drawing.Size(382, 136);
            this.panel_choice.TabIndex = 20;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(4, 83);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(85, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // rBtn_LSB
            // 
            this.rBtn_LSB.AutoSize = true;
            this.rBtn_LSB.Location = new System.Drawing.Point(4, 60);
            this.rBtn_LSB.Name = "rBtn_LSB";
            this.rBtn_LSB.Size = new System.Drawing.Size(85, 17);
            this.rBtn_LSB.TabIndex = 2;
            this.rBtn_LSB.TabStop = true;
            this.rBtn_LSB.Text = "radioButton3";
            this.rBtn_LSB.UseVisualStyleBackColor = true;
            this.rBtn_LSB.CheckedChanged += new System.EventHandler(this.rBtn_3_CheckedChanged);
            // 
            // rBtn_SimpleRandom
            // 
            this.rBtn_SimpleRandom.AutoSize = true;
            this.rBtn_SimpleRandom.Location = new System.Drawing.Point(4, 37);
            this.rBtn_SimpleRandom.Name = "rBtn_SimpleRandom";
            this.rBtn_SimpleRandom.Size = new System.Drawing.Size(85, 17);
            this.rBtn_SimpleRandom.TabIndex = 1;
            this.rBtn_SimpleRandom.TabStop = true;
            this.rBtn_SimpleRandom.Text = "radioButton2";
            this.rBtn_SimpleRandom.UseVisualStyleBackColor = true;
            this.rBtn_SimpleRandom.CheckedChanged += new System.EventHandler(this.rBtn_SimpleRandom_CheckedChanged);
            // 
            // rBtn_Simple
            // 
            this.rBtn_Simple.AutoSize = true;
            this.rBtn_Simple.Checked = true;
            this.rBtn_Simple.Location = new System.Drawing.Point(4, 14);
            this.rBtn_Simple.Name = "rBtn_Simple";
            this.rBtn_Simple.Size = new System.Drawing.Size(85, 17);
            this.rBtn_Simple.TabIndex = 0;
            this.rBtn_Simple.TabStop = true;
            this.rBtn_Simple.Text = "radioButton1";
            this.rBtn_Simple.UseVisualStyleBackColor = true;
            this.rBtn_Simple.CheckedChanged += new System.EventHandler(this.rBtn_Simple_CheckedChanged);
            // 
            // MainViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_choice);
            this.Controls.Add(this.lbl_keyDescription);
            this.Controls.Add(this.lbl_txtPathDescription);
            this.Controls.Add(this.lbl_imgPathDescription);
            this.Controls.Add(this.btn_mainAction);
            this.Controls.Add(this.tb_key);
            this.Controls.Add(this.tb_txtPath);
            this.Controls.Add(this.tb_imgPath);
            this.Controls.Add(this.btn_openTxtFileDialog);
            this.Controls.Add(this.btn_openImgFileDialog);
            this.Name = "MainViewUserControl";
            this.Size = new System.Drawing.Size(493, 391);
            this.panel_choice.ResumeLayout(false);
            this.panel_choice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_keyDescription;
        private System.Windows.Forms.Label lbl_txtPathDescription;
        private System.Windows.Forms.Label lbl_imgPathDescription;
        private System.Windows.Forms.Button btn_mainAction;
        private System.Windows.Forms.TextBox tb_key;
        private System.Windows.Forms.TextBox tb_txtPath;
        private System.Windows.Forms.TextBox tb_imgPath;
        private System.Windows.Forms.Button btn_openTxtFileDialog;
        private System.Windows.Forms.Button btn_openImgFileDialog;
        private System.Windows.Forms.Panel panel_choice;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton rBtn_LSB;
        private System.Windows.Forms.RadioButton rBtn_SimpleRandom;
        private System.Windows.Forms.RadioButton rBtn_Simple;
    }
}
