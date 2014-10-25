using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSystem
{
    public class FileEntity
    {
        #region 文件实体常用属性
        public FileStream file { get; private set; }
        public byte[] fileTable { get; private set; }
        public byte[] cache { get; private set; }
        public byte[][] currentMem { get; private set; }
        #endregion

        #region 初始化
        public FileEntity()
        {
        }

        public int Init()
        {
            // 初始化落地文件
            file = new FileStream(@"./filetemp", FileMode.OpenOrCreate);
            file.SetLength(128 * 64);
            file.Seek(0, SeekOrigin.Begin);
            if (file.Length == 0)
            {
                // 初始化位图
                fileTable = new byte[128];
                fileTable[0] = 255;
                fileTable[1] = 255;
                fileTable[2] = 255;
                for (int i = 0; i < 64; i+=8 )
                {
                    currentMem[2][i] = Convert.ToByte("$");
                }
                file.Write(fileTable, 0, 128);
            }
            else
            {
                // 载入位图
                fileTable = new byte[128];
                file.Read(fileTable, 0, 128);
                currentMem = new byte[128][];
                for (int i = 0; i < 128; i++)
                {
                    currentMem[i] = new byte[64];
                    file.Read(currentMem[i], 0, 64);
                }
            }
            file.Flush();
            return 0;
        }
        #endregion

        #region 释放资源
        public int destroy()
        {
            file.Flush();
            file.Close();
            return 0;
        }
        #endregion

        #region 产生随机错误
        public int WrongHappen()
        {
            int diskCount = 127;
            long tick = DateTime.Now.Ticks;
            Random ro = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int wrongCount = ro.Next(10);
            for (int i = 0; i <= wrongCount; i++ )
            {
                int roPos = ro.Next(diskCount);
                fileTable[roPos] = 254; // 254表示这块磁盘损坏，不能被调用
            }
            return 0;
        }
        #endregion

        #region 文件基本操作
        #region 建立文件
        public int CreateFile(string parentPath)
        {
            bool isSuccess = false;
            string fileName = "";
            string fileExpend = "";
            int fileProperty = 0;
            CreateFileDialog createFileDialog = new CreateFileDialog();
            createFileDialog.Show();
            if (createFileDialog.messageConfirm == System.Windows.Forms.DialogResult.OK)
            {
                fileName = createFileDialog.fileName;
                fileExpend = createFileDialog.fileExpend;
                fileProperty = createFileDialog.fileProperty;
            }
            
            if (parentPath == "/")
            {
                for (int i = 0; i < 64; i += 8)
                {
                    if (this.currentMem[2][i] == Convert.ToByte('$'))
                    {
                        isSuccess = true;
                        int pos = 0;
                        foreach (byte preNameChar in System.Text.Encoding.Default.GetBytes(fileName))
                        {
                            currentMem[2][pos++] = preNameChar;
                        }
                        foreach (byte preNameChar in System.Text.Encoding.Default.GetBytes(fileExpend))
                        {
                            currentMem[2][pos++] = preNameChar;
                        }
                        switch (fileProperty)
                        {
                            case 0:
                                currentMem[2][pos++] = 5;
                                break;
                            case 1:
                                currentMem[2][pos++] = 4;
                                break;
                        }
                        

                    }
                }
            }

            if (!isSuccess)
                return -1;
            return 0;
        }
        #endregion 

        #region 打开文件
        public int OpenFile(string fileName, char operateType)
        {
            return 0;
        }
        #endregion 

        #region 读文件
        public int ReadFile(string fileName, int length)
        {
            return 0;
        }
        #endregion

        #region 写文件
        public int WriteFile(string fileName, byte[] buffer, int length)
        {
            return 0;
        }
        #endregion

        #region 关闭文件
        public int CloseFile(string fileName)
        {
            return 0;
        }
        #endregion

        #region 显示文件内容
        public int TypeFile()
        {
            return 0;
        }
        #endregion

        #region 改变文件属性
        public int ChangeFile()
        {
            return 0;
        }
        #endregion

        #region 重命名
        public int Rename(string fileName, string newName)
        {
            return 0;
        }
        #endregion
        #endregion

        #region 目录操作
        #region 建立目录
        public int md(string dirName)
        {
            return 0;
        }
        #endregion

        #region 显示目录内容
        public int dir(string dirname)
        {
            return 0;
        }
        #endregion

        #region 删除空目录
        public int rd()
        {
            return 0;
        }
        #endregion 
        #endregion

    }

    #region 目录项
    class fileAttribute
    {
        public string fileName { get; set;} // 文件名(目录名)
        public byte fileType { get; set; } // 文件类型
        public byte fileProperty { get; set; } // 文件属性
        public byte beginPiece { get; set; } // 起始盘块
        public byte pieceLength { get; set; } // 盘块数目
    }
    #endregion

    #region 文件位置
    struct Pointer // 文件位置
    {
        int dnum;
        int bnum;
    }
    #endregion

    #region 打开文件信息
    struct OFile // 打开文件信息
    {
        string name; // 文件的绝对路径
        byte attribute; // 文件属性
        byte beginNum; // 起始盘块
        byte length; // 盘块长度
        byte flag; // 读写控制位
        Pointer read; // 读文件位置
        Pointer write; // 写文件位置
    }
    #endregion

    #region 打开文件登记表
    struct OpenFile // 打开文件登记表
    {
        OFile[] openFiles;
        int lenght;
        OpenFile(int MAXFILECOUNT = 10)
        {
            openFiles = new OFile[MAXFILECOUNT]; 
            lenght = 0;
            for (int i = 0; i<10; i++)
            {
                openFiles[i] = new OFile();
            }
        }
    }
    #endregion
}
