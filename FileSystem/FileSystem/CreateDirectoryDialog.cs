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
    public partial class CreateDirectoryDialog : Form
    {
        public string dirName { get; set; }
        public CreateDirectoryDialog()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
           dirName = this.DirectoryName.Text;
            while (dirName.Length < 3)
                dirName += " ";
            FileSystem.fe.md(FileSystem.currentPath, dirName);
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
