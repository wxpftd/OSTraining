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
        public FileEntity fe;
        public FileSystem()
        {
            InitializeComponent();
            this.Start();
        }

        #region 系统开始运行
        private int Start()
        {
            fe = new FileEntity();
            fe.Init();
            fe.WrongHappen();
            for (int i = 0; i < 128; i++)
            {
                System.Windows.Forms.ListViewItem newListViewItem = new ListViewItem(new string[] {"#" + i, Convert.ToString(fe.fileTable[i])}, -1);
                this.listView1.Items.Add(newListViewItem);
            }
            if (fe.fileTable[0] == Convert.ToByte(254) || fe.fileTable[1] == Convert.ToByte(254) || fe.fileTable[2] == Convert.ToByte(254))
            {
                // TODO 弹出错误窗口
            }

            fe.destroy();
            return 0;
        }
        #endregion

        #region 刷新目录树
        private int treeViewFlush()
        {
            TreeNode rootNode = new TreeNode(@"/", IconIndexes.MyDirectory, IconIndexes.MyDirectory);
            rootNode.Tag = @"/";
            rootNode.Text = @"/";
            this.treeView1.Nodes.Add(rootNode);
            return 0;
        }

        #region 递归遍历目录树
        private int traFile()
        {
            return 0;
        }
        #endregion

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            fe.CreateFile(this.treeView1.SelectedNode.FullPath);
        }
        #endregion

    }

    class IconIndexes
    {
        public const int MyDirectory = 0;
        public const int MyFile = 1;
    }
}
