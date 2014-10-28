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
    public partial class AttributesShow : Form
    {
        public int flag;
        public AttributesShow()
        {
            InitializeComponent();
        }

        public int Start(string fileName, int diskNum, int flag)
        {
            this.textBox1.Text = fileName;
            if (FileSystem.currentPath == @"/")
            {
                this.textBox2.Text = @"/" + fileName;
            }
            else
                this.textBox2.Text = FileSystem.currentPath + @"/" + fileName;
            this.textBox3.Text = Convert.ToString(diskNum);
            if (flag == 0)
                this.radioButton1.Checked = true;
            else
                this.radioButton2.Checked = true;
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                this.flag = 0;
            else
                this.flag = 1;
            this.Close();
        }
    }
}
