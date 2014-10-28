using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSystem
{
    public partial class CreateDirectory : Form
    {
        public CreateDirectory()
        {
            InitializeComponent();
        }

        private void Comfirm_Click(object sender, EventArgs e)
        {
            string directoryName = "";
            int dotCount = 0;
            foreach (char ch in this.CreateDirectoryText.Text.Trim())
            {
                if (ch.Equals('.'))
                    ++dotCount;
            }
            if (String.IsNullOrWhiteSpace(this.CreateDirectoryText.Text.Trim()))
            {
                MessageBox.Show("目录名不能为空。");
            }
            else if (dotCount > 1)
                MessageBox.Show("目录名不能包含非法符号");
            else
            {
                directoryName = this.CreateDirectoryText.Text.Trim();
                if (directoryName.Length > 3)
                    MessageBox.Show("目录名长度不能超过3个字节");
                else if (directoryName.Contains(@"$") || directoryName.Contains(@".") || directoryName.Contains(@"/"))
                    MessageBox.Show("目录名不能包含特殊字符");
                   this.Close();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
