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
    public partial class FileTable : Form
    {
        public FileTable()
        {
            InitializeComponent();
            for (int i = 0; i < 128; i++)
            {
                System.Windows.Forms.ListViewItem newListViewItem = new ListViewItem(new string[] { "#" + i, Convert.ToString(FileSystem.fe.fileTable[i]) }, -1);
                this.listView1.Items.Add(newListViewItem);
            }
        }
    }
}
