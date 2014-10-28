namespace FileSystem
{
    partial class CreateDirectory
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
            this.CreateDirectoryText = new System.Windows.Forms.TextBox();
            this.Comfirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = " 请输入文件名";
            // 
            // CreateDirectoryText
            // 
            this.CreateDirectoryText.Location = new System.Drawing.Point(154, 33);
            this.CreateDirectoryText.Name = "CreateDirectoryText";
            this.CreateDirectoryText.Size = new System.Drawing.Size(100, 25);
            this.CreateDirectoryText.TabIndex = 1;
            // 
            // Comfirm
            // 
            this.Comfirm.Location = new System.Drawing.Point(59, 87);
            this.Comfirm.Name = "Comfirm";
            this.Comfirm.Size = new System.Drawing.Size(75, 23);
            this.Comfirm.TabIndex = 2;
            this.Comfirm.Text = "确定";
            this.Comfirm.UseVisualStyleBackColor = true;
            this.Comfirm.Click += new System.EventHandler(this.Comfirm_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(154, 87);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // CreateDirectory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 161);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Comfirm);
            this.Controls.Add(this.CreateDirectoryText);
            this.Controls.Add(this.label1);
            this.Name = "CreateDirectory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建目录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CreateDirectoryText;
        private System.Windows.Forms.Button Comfirm;
        private System.Windows.Forms.Button Cancel;
    }
}