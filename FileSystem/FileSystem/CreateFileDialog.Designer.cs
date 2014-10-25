namespace FileSystem
{
    partial class CreateFileDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.FileNameText = new System.Windows.Forms.TextBox();
            this.ReadOnly = new System.Windows.Forms.RadioButton();
            this.ReadAndOnly = new System.Windows.Forms.RadioButton();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入新建文件的名称";
            // 
            // FileNameText
            // 
            this.FileNameText.Location = new System.Drawing.Point(194, 27);
            this.FileNameText.Name = "FileNameText";
            this.FileNameText.Size = new System.Drawing.Size(100, 25);
            this.FileNameText.TabIndex = 1;
            // 
            // ReadOnly
            // 
            this.ReadOnly.AutoSize = true;
            this.ReadOnly.Location = new System.Drawing.Point(63, 73);
            this.ReadOnly.Name = "ReadOnly";
            this.ReadOnly.Size = new System.Drawing.Size(58, 19);
            this.ReadOnly.TabIndex = 2;
            this.ReadOnly.TabStop = true;
            this.ReadOnly.Text = "只读";
            this.ReadOnly.UseVisualStyleBackColor = true;
            // 
            // ReadAndOnly
            // 
            this.ReadAndOnly.AutoSize = true;
            this.ReadAndOnly.Location = new System.Drawing.Point(164, 73);
            this.ReadAndOnly.Name = "ReadAndOnly";
            this.ReadAndOnly.Size = new System.Drawing.Size(58, 19);
            this.ReadAndOnly.TabIndex = 4;
            this.ReadAndOnly.TabStop = true;
            this.ReadAndOnly.Text = "读写";
            this.ReadAndOnly.UseVisualStyleBackColor = true;
            // 
            // Confirm
            // 
            this.Confirm.Location = new System.Drawing.Point(63, 114);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(75, 23);
            this.Confirm.TabIndex = 5;
            this.Confirm.Text = "确认";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(194, 114);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // CreateFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 167);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.ReadAndOnly);
            this.Controls.Add(this.ReadOnly);
            this.Controls.Add(this.FileNameText);
            this.Controls.Add(this.label1);
            this.Name = "CreateFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateFileDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileNameText;
        private System.Windows.Forms.RadioButton ReadOnly;
        private System.Windows.Forms.RadioButton ReadAndOnly;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
    }
}