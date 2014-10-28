﻿using System;
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
            for (int i = 0; i < 128; i++)
            {
                System.Windows.Forms.ListViewItem newListViewItem = new ListViewItem(new string[] { "#" + i, Convert.ToString(fe.fileTable[i]) }, -1);
                this.listView1.Items.Add(newListViewItem);
            }
            if (fe.fileTable[0] == Convert.ToByte(254) || fe.fileTable[1] == Convert.ToByte(254) || fe.fileTable[2] == Convert.ToByte(254))
            {
                // TODO 弹出错误窗口
            }
            this.ViewFlush(currentPath);
            //fe.destroy();
            return 0;
        }
        #endregion

        #region 刷新目录树和文件视图
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

        #region 按链接寻找文件
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
                                    MessageBox.Show("打开" + listView2.SelectedItems[0].Text);
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

                string fileExpendTemp = "";
                fileExpendTemp += fileAttribute.fileType1;
                fileExpendTemp += fileAttribute.fileType2;
                if (!String.IsNullOrWhiteSpace(fileExpendTemp))
                {
                    fileExpendTemp += '.';
                    fileExpendTemp += fileAttribute.fileType1;
                    fileExpendTemp += fileAttribute.fileType2;
                    name += fileExpendTemp;
                }
            }
            else
            {
                lvi.ImageIndex = 0;
                name += fileAttribute.fileName1;
                name += fileAttribute.fileName2;
                name += fileAttribute.fileName3;
            }
            lvi.Text = name;
            return lvi;
        }
        #endregion

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            CreateFileDialog cf = new CreateFileDialog();
            cf.Show();
            cf.BeginInvoke(new Action(() => { ViewFlush(currentPath); }));
        }
        #endregion

        private void CreateDirectoryButton_Click(object sender, EventArgs e)
        {

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
                        ViewFlush(currentPath + listView2.SelectedItems[0].Text);
                else
                {
                    ViewFlush(currentPath + @"/" + listView2.SelectedItems[0].Text);
                }
            }
        }

    }

    class IconIndexes
    {
        public const int MyDirectory = 0;
        public const int MyFile = 1;
    }
}
