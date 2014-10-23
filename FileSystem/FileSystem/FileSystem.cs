using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FileSystem
{
    public partial class FileSystem : Form
    {
        public FileSystem()
        {
            InitializeComponent();
            FileEntity fe = new FileEntity();
            fe.Init();
            fe.WrongHappen();
            for (int i = 0; i < 128; i++)
            {
                System.Windows.Forms.ListViewItem newListViewItem = new ListViewItem(new string[] {"#" + i, Convert.ToString(fe.fileTable[i])}, -1);
                this.listView1.Items.Add(newListViewItem);
            }
            fe.destroy();
        }
    }
}
