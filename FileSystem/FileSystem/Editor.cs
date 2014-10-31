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
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            char[] buffer = new char[72];
            for (int i = 0; i < 72; ++i)
            {
                buffer[i] = Convert.ToChar(FileSystem.fe.cache[FileSystem.oneOpenFile.beginNum][i]);
            }
            this.Content.Text = new string(buffer);
            if (FileSystem.oneOpenFile.flag == 0)
            {
                this.Content.Enabled = false;
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            char[] buffer = this.Content.Text.ToCharArray();
            int pos = 0;
            foreach (char ch in buffer)
            {
                FileSystem.fe.cache[FileSystem.oneOpenFile.beginNum][pos] = Convert.ToByte(ch) ;
                ++pos;
            }
            FileSystem.fe.dataLand();
        }

        private void CloseFile_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] buffer = this.Content.Text.ToCharArray();
            int pos = 0;
            foreach (char ch in buffer)
            {
                FileSystem.fe.cache[FileSystem.oneOpenFile.beginNum][pos] = Convert.ToByte(ch);
                ++pos;
            }
            FileSystem.fe.dataLand();
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
