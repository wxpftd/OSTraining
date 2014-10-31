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
        public static FileEntity fe;
        public static string currentPath;
        public static OFile oneOpenFile;
        public FileSystem()
        {
            currentPath = "/";
            InitializeComponent();
            this.Start();
        }

        #region 系统开始运行
        private int Start()
        {
            fe = new FileEntity();
            fe.Init();
            
            if (fe.fileTable[0] == Convert.ToByte(254) || fe.fileTable[1] == Convert.ToByte(254) || fe.fileTable[2] == Convert.ToByte(254))
            {
                // TODO 弹出错误窗口
            }
            this.ViewFlush(currentPath);
            //fe.destroy();
            return 0;
        }
        #endregion

        #region 刷新文件视图
        private int ViewFlush(string fullPath)
        {
            this.listView2.BeginUpdate();
            FileAttribute[] fileAttributes = FindByFullPath(fullPath);
            if (fileAttributes == null)
                return -1;
            else
            {
                listView2.Clear();
                foreach (FileAttribute fa in fileAttributes)
                {
                    if (fa.fileName1 == '$')
                        continue;
                    ListViewItem lvi = FileAttributeToIcon(fa);
                    this.listView2.Items.Add(lvi);
                }
                this.FullPath.Text = currentPath;
                this.listView2.EndUpdate();
                return 0;
            }
        }

        #region 按链接寻找目录项
        private FileAttribute[] FindByFullPath(string fullPath)
        {
            if (fullPath == @"/")
            {
                FileAttribute[] fileAttributes = new FileAttribute[8];
                PieceToFileAttributes(fe.cache[2], fileAttributes);
                currentPath = fullPath;
                return fileAttributes;
            }
            else
            {
                Queue<string> parts = new Queue<string>(fullPath.Split(new char[] { '/' }).ToList());
                parts.Dequeue();
                FileAttribute[] fileAttributes = new FileAttribute[8];
                byte[] currentPiece = fe.cache[2];
                while (parts.Count > 0)
                {
                    string partName = parts.Dequeue();
                    PieceToFileAttributes(currentPiece, fileAttributes);
                    if (parts.Count == 0)
                    {
                        char[] nameChars = new char[3];
                        foreach (FileAttribute fa in fileAttributes)
                        {
                            nameChars[0] = fa.fileName1;
                            nameChars[1] = fa.fileName2;
                            nameChars[2] = fa.fileName3;
                            string name = new string(nameChars);
                            name = name.Trim();
                            if (name == partName)
                            {
                                if (fa.fileOrDirctory == 0)
                                {
                                    // 弹出文件编辑窗口
                                    //MessageBox.Show("打开" + listView2.SelectedItems[0].Text);
                                    return FindByFullPath(currentPath);
                                }
                                else
                                {
                                    FileAttribute[] retFileAttributes = new FileAttribute[8];
                                    PieceToFileAttributes(fe.cache[fa.beginPiece], retFileAttributes);
                                    currentPath = fullPath;
                                    return retFileAttributes;
                                }
                            }
                        }
                    }
                    foreach (FileAttribute fa in fileAttributes)
                    {
                        char[] nameChars = new char[3];

                        if (fa.fileOrDirctory == 1)
                        {
                            nameChars[0] = fa.fileName1;
                            nameChars[1] = fa.fileName2;
                            nameChars[2] = fa.fileName3;
                            string name = new string(nameChars);
                            name = name.Trim();
                            if (name == partName)
                            {
                                currentPiece = fe.cache[fa.beginPiece];
                            }
                        }
                    }
                }
                // return
            }
            return null;
        }
        #endregion

        #region 递归遍历目录树
        private int traFile()
        {
            return 0;
        }
        #endregion

        #region 将磁盘块转成对象数组
        private int PieceToFileAttributes(byte[] piece, FileAttribute[] fileAttributes)
        {
            int fileOrder = 0;
            for (int i = 0; i < 72; i += 9)
            {
                byte[] oneAttribute = new byte[9];
                for (int j = 0; j < 9; ++j)
                {
                    oneAttribute[j] = piece[i + j];
                }
                fileAttributes[fileOrder] = new FileAttribute();
                fileAttributes[fileOrder].ToFileAttribute(oneAttribute);
                ++fileOrder;
            }
            return 0;
        }
        #endregion

        #region 将对象转成图标
        private ListViewItem FileAttributeToIcon(FileAttribute fileAttribute)
        {
            ListViewItem lvi = new ListViewItem();
            string name = "";
            if (fileAttribute.fileOrDirctory == 0)
            {
                lvi.ImageIndex = 1;
                string fileNameTemp = "";
                fileNameTemp += fileAttribute.fileName1;
                fileNameTemp += fileAttribute.fileName2;
                fileNameTemp += fileAttribute.fileName3;
                name += fileNameTemp.Trim();
                if (!String.IsNullOrWhiteSpace(Convert.ToString(fileAttribute.fileType1)))
                {
                    string fileExpendTemp = "";
                    fileExpendTemp += '.';
                    fileExpendTemp += fileAttribute.fileType1;
                    fileExpendTemp += fileAttribute.fileType2;
                    name += fileExpendTemp.Trim();
                }
            }
            else
            {
                lvi.ImageIndex = 0;
                name += fileAttribute.fileName1;
                name += fileAttribute.fileName2;
                name += fileAttribute.fileName3;
                name = name.Trim();
            }
            lvi.Text = name;
            return lvi;
        }
        #endregion

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            CreateFileDialog cf = new CreateFileDialog();
            cf.ShowDialog();
            ViewFlush(currentPath);
        }
        #endregion

        private void CreateDirectoryButton_Click(object sender, EventArgs e)
        {
            CreateDirectoryDialog cd = new CreateDirectoryDialog();
            cd.ShowDialog();
            ViewFlush(currentPath);
        }

        private void transfer_Click(object sender, EventArgs e)
        {
            this.ViewFlush(this.FullPath.Text);
        }

        private void ForWard_Click(object sender, EventArgs e)
        {
            if (currentPath != @"/")
            {
                string lastPath;
                int length = currentPath.LastIndexOf('/');
                if (length > 0)
                    lastPath = currentPath.Substring(0, length);
                else
                    lastPath = @"/";
                currentPath = lastPath;
                ViewFlush(currentPath);
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count > 0)
            {
                if (currentPath == @"/")
                {
                    if (listView2.SelectedItems[0].ImageIndex == 0)
                        ViewFlush(currentPath + listView2.SelectedItems[0].Text);
                    else
                    {
                        oneOpenFile = new OFile();
                        FileAttribute fa = fe.FindFileByFullPath(currentPath, listView2.SelectedItems[0].Text);
                        oneOpenFile.beginNum = fa.beginPiece;
                        oneOpenFile.flag = fa.isReadOnly;
                        Editor editor = new Editor();
                        editor.Show();
                    }
                }
                else
                {
                    if (listView2.SelectedItems[0].ImageIndex == 0)
                        ViewFlush(currentPath + @"/" + listView2.SelectedItems[0].Text);
                    else
                    {
                        oneOpenFile = new OFile();
                        FileAttribute fa = fe.FindFileByFullPath(currentPath, listView2.SelectedItems[0].Text);
                        oneOpenFile.beginNum = fa.beginPiece;
                        oneOpenFile.flag = fa.isReadOnly;
                        Editor editor = new Editor();
                        editor.Show();
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileTable ft = new FileTable();
            ft.Show();
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count > 0)
            {
                FileAttribute[] fileAttributes = FindByFullPath(currentPath);
                string name = "";
                foreach (FileAttribute fa in fileAttributes)
                {
                    name = "";
                    name += fa.fileName1;
                    name += fa.fileName2;
                    name += fa.fileName3;
                    name = name.Trim();
                    if (name == listView2.SelectedItems[0].Text)
                    {
                        fe.fileTable[fa.beginPiece] = 0;
                        fe.cache[fa.beginPiece] = new byte[72];
                        fa.fileName1 = '$';
                        break;
                    }
                }
                int currentPos = fe.FindDiskPiece(currentPath);
                fe.AttributesInCache(currentPos, fileAttributes);
                fe.dataLand();
            }
            ViewFlush(currentPath);
       }

        private void ReNameButton_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count > 0)
            {
                FileAttribute[] fileAttributes = FindByFullPath(currentPath);
                string name = "";
                foreach (FileAttribute fa in fileAttributes)
                {
                    name = "";
                    name += fa.fileName1;
                    name += fa.fileName2;
                    name += fa.fileName3;
                    name = name.Trim();
                    if (name == listView2.SelectedItems[0].Text)
                    {
                        ReName rn = new ReName();
                        rn.ShowDialog();
                        if (rn.messageConfirm == System.Windows.Forms.DialogResult.OK)
                        {
                            string newNameTemp = rn.newName;
                            fa.fileName1 = newNameTemp[0];
                            fa.fileName2 = newNameTemp[1];
                            fa.fileName3 = newNameTemp[2];
                        }
                        break;
                    }
                }
                int currentPos = fe.FindDiskPiece(currentPath);
                fe.AttributesInCache(currentPos, fileAttributes);
                fe.dataLand();
            }
            ViewFlush(currentPath);
        }

        private void PropertyButton_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count > 0)
            {
                FileAttribute[] fileAttributes = FindByFullPath(currentPath);
                string name = "";
                foreach (FileAttribute fa in fileAttributes)
                {
                    name = "";
                    name += fa.fileName1;
                    name += fa.fileName2;
                    name += fa.fileName3;
                    name = name.Trim();
                    if (name == listView2.SelectedItems[0].Text)
                    {
                        AttributesShow attributesShow = new AttributesShow();
                        attributesShow.Start(this.listView2.SelectedItems[0].Text, fa.beginPiece, fa.isReadOnly);
                        attributesShow.ShowDialog();
                        fa.isReadOnly = Convert.ToByte(attributesShow.flag);
                        break;
                    }
                }
                int currentPos = fe.FindDiskPiece(currentPath);
                fe.AttributesInCache(currentPos, fileAttributes);
                fe.dataLand();
            }
        }

    }

    class IconIndexes
    {
        public const int MyDirectory = 0;
        public const int MyFile = 1;
    }
}
