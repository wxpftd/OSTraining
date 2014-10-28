using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace FileSystem
{
    public class FileEntity
    {
        #region 文件实体常用属性
        public FileStream file { get; private set; }
        public byte[] fileTable { get; private set; }
        public byte[][] cache { get; private set; }
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
            file.Seek(0, SeekOrigin.Begin);
            cache = new byte[128][];
            for (int i = 0; i < 128; ++i)
            {
                cache[i] = new byte[72];
            }
            if (file.Length == 0)
            {
                // 初始化位图
                fileTable = new byte[128];
                fileTable[0] = 255;
                fileTable[1] = 255;
                fileTable[2] = 255;
                for (int i = 0; i < 72; i += 9)
                {
                    FileAttribute oneFileAttribute = new FileAttribute();
                    oneFileAttribute.fileName1 = '$';
                    int pos = 0;
                    foreach (byte attribute in oneFileAttribute.ToByte())
                    {
                        cache[2][i + pos] = attribute;
                        ++pos;
                    }
                    pos = 0;
                }
                byte[] testFileAttribute = new FileAttribute('F', 'T', 'D', '\0', '\0', 1, 0, 3, 1).ToByte();
                for (int i = 0; i < 9; ++i )
                {
                    cache[2][i] = testFileAttribute[i];
                }
                for (int i = 0; i < 72; i += 9)
                {
                    FileAttribute oneFileAttribute = new FileAttribute();
                    oneFileAttribute.fileName1 = '$';
                    int pos = 0;
                    foreach (byte attribute in oneFileAttribute.ToByte())
                    {
                        cache[3][i + pos] = attribute;
                        ++pos;
                    }
                    pos = 0;
                }
 
                this.fileTable[3] = 255;
                this.dataLand();
            }
            else
            {
                // 载入位图
                fileTable = new byte[128];
                file.Read(fileTable, 0, 128);
                for (int i = 2; i < 128; i++)
                {
                    cache[i] = new byte[72];
                    file.Read(cache[i], 0, 72);
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
            for (int i = 0; i <= wrongCount; i++)
            {
                int roPos = ro.Next(diskCount);
                fileTable[roPos] = 254; // 254表示这块磁盘损坏，不能被调用
            }
            return 0;
        }
        #endregion

        #region 文件基本操作
        #region 建立文件
        public int CreateFile(string parentPath, string fileName, string fileExpend, int fileProperty)
        {
            if (subFileCount(parentPath) < 8)
            {
                int currentPos = FindDiskPiece(parentPath);
                FileAttribute[] fileAttributes = FindByFullPath(parentPath);
                foreach (FileAttribute fa in fileAttributes)
                {
                    if (fa.fileName1 == '$')
                    {
                        char[] cffileName = fileName.ToCharArray();
                        fa.fileName1 = cffileName[0];
                        fa.fileName2 = cffileName[1];
                        fa.fileName3 = cffileName[2];
                        char[] cffileExpend = fileExpend.ToCharArray();
                        fa.fileType1 = cffileExpend[0];
                        fa.fileType2 = cffileExpend[1];
                        fa.fileOrDirctory = 0;
                        fa.isReadOnly = Convert.ToByte(fileProperty);
                        fa.pieceLength = 1;
                        for (int i = 2; i < 128; ++i)
                        {
                            if (fileTable[i] == 0)
                            {
                                fileTable[i] = 255;
                                fa.beginPiece = Convert.ToByte(i);
                                for (int j = 0; j < 72; j += 9)
                                {
                                    FileAttribute oneFileAttribute = new FileAttribute();
                                    oneFileAttribute.fileName1 = '$';
                                    int pos = 0;
                                    foreach (byte attribute in oneFileAttribute.ToByte())
                                    {
                                        cache[currentPos][j + pos] = attribute;
                                        ++pos;
                                    }
                                    pos = 0;
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                AttributesInCache(currentPos, fileAttributes);
                dataLand();
                return 0;
            }
            return -1;
        }

        #region 返回文件夹内已用目录项的个数
        public int subFileCount(string parentPath)
        {
            FileAttribute[] fileAttributes = FindByFullPath(parentPath);
            int count = 0;
            foreach (FileAttribute fa in fileAttributes)
            {
                if (fa.fileName1 != '$')
                    ++count;
            }
            return count;
        }
        #endregion

        #region 返回路径所在的磁盘块中
        public int FindDiskPiece(string fullPath)
        {
            if (fullPath == @"/")
            {
                return 2;
            }
            else
            {
                Queue<string> parts = new Queue<string>(fullPath.Split(new char[] { '/' }).ToList());
                parts.Dequeue();
                FileAttribute[] fileAttributes = new FileAttribute[8];
                byte[] currentPiece = FileSystem.fe.cache[2];
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
                                    return fa.beginPiece;
                                }
                                else
                                {
                                    return fa.beginPiece;
                                }
                            }
                        }
                    }
                    else
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
                                    currentPiece = FileSystem.fe.cache[fa.beginPiece];
                                }
                            }
                        }
                }
                // return
            }
            return -1;
        }
        #endregion

        #region 将修改后的目录项数组装进cache
        public int AttributesInCache(int currentPos, FileAttribute[] fileAttributes)
        {
            int cacheInnerPos = 0;
            for (int i = 0; i < 8; ++i)
            {
                foreach (byte oneByte in fileAttributes[i].ToByte())
                {
                    cache[currentPos][cacheInnerPos] = oneByte;
                    cacheInnerPos++;
                }
            }
            return 0;
        }
        #endregion

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
        public int md(string parentPath, string dirName)
        {
            if (subFileCount(parentPath) < 8)
            {
                int currentPos = FindDiskPiece(parentPath);
                FileAttribute[] fileAttributes = FindByFullPath(parentPath);
                foreach (FileAttribute fa in fileAttributes)
                {
                    if (fa.fileName1 == '$')
                    {
                        char[] cffileName = dirName.ToCharArray();
                        fa.fileName1 = cffileName[0];
                        fa.fileName2 = cffileName[1];
                        fa.fileName3 = cffileName[2];
                        fa.fileOrDirctory = 1;
                        fa.isReadOnly = 1;
                        fa.pieceLength = 1;
                        for (int i = 2; i < 128; ++i)
                        {
                            if (fileTable[i] == 0)
                            {
                                fileTable[i] = 255;
                                fa.beginPiece = Convert.ToByte(i);
                                for (int j = 0; j < 72; j += 9)
                                {
                                    cache[i][j] = Convert.ToByte('$');
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                AttributesInCache(currentPos, fileAttributes);
                dataLand();
                return 0;
            }
            return -1;
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

        #region 按链接寻找文件项
        public FileAttribute FindFileByFullPath(string parentPath, string fileName)
        {
            FileAttribute[] fileAttributes = FindByFullPath(parentPath);
            char[] nameChars = new char[3];
            foreach (FileAttribute fa in fileAttributes)
            {
                nameChars[0] = fa.fileName1;
                nameChars[1] = fa.fileName2;
                nameChars[2] = fa.fileName3;
                string name = new string(nameChars);
                name = name.Trim();
                if (name == fileName)
                {
                    if (fa.fileOrDirctory == 0)
                    {
                        // 弹出文件编辑窗口
                        return fa;
                    }
                }
            }
            return FindByFullPath(FileSystem.currentPath)[0];
        }
        #endregion

        #region 按链接寻找目录项
        private FileAttribute[] FindByFullPath(string fullPath)
        {
            if (fullPath == @"/")
            {
                FileAttribute[] fileAttributes = new FileAttribute[8];
                PieceToFileAttributes(FileSystem.fe.cache[2], fileAttributes);
                FileSystem.currentPath = fullPath;
                return fileAttributes;
            }
            else
            {
                Queue<string> parts = new Queue<string>(fullPath.Split(new char[] { '/' }).ToList());
                parts.Dequeue();
                FileAttribute[] fileAttributes = new FileAttribute[8];
                byte[] currentPiece = FileSystem.fe.cache[2];
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
                                    return FindByFullPath(FileSystem.currentPath);
                                }
                                else
                                {
                                    FileAttribute[] retFileAttributes = new FileAttribute[8];
                                    PieceToFileAttributes(FileSystem.fe.cache[fa.beginPiece], retFileAttributes);
                                    FileSystem.currentPath = fullPath;
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
                                currentPiece = FileSystem.fe.cache[fa.beginPiece];
                            }
                        }
                    }
                }
                // return
            }
            return null;
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

        #region 数据持久化
        public int dataLand()
        {
            file.Seek(0, SeekOrigin.Begin);
            file.Write(fileTable, 0, 128);
            for (int i = 2; i < 128; ++i)
            {
                file.Write(cache[i], 0, 72);
            }
            return 0;
        }
        #endregion

    }

    #region 目录项
    public class FileAttribute
    {
        public char fileName1 { get; set; } // 文件名(目录名)
        public char fileName2 { get; set; } // 文件名(目录名)
        public char fileName3 { get; set; } // 文件名(目录名)
        public char fileType1 { get; set; } // 文件类型
        public char fileType2 { get; set; } // 文件类型
        public byte fileOrDirctory { get; set; } // 文件目录标志位
        public byte isReadOnly { get; set; } // 只读权限或者读写权限
        public byte beginPiece { get; set; } // 起始盘块
        public byte pieceLength { get; set; } // 盘块数目

        public FileAttribute()
        {

        }
        public FileAttribute(char fileName1, char fileName2, char fileName3, char fileType1, char fileType2, byte fileOrDirctory, byte isReadOnly, byte beginPiece, byte pieceLength)
        {
            this.fileName1 = fileName1;
            this.fileName2 = fileName2;
            this.fileName3 = fileName3;
            this.fileType1 = fileType1;
            this.fileType2 = fileType2;
            this.fileOrDirctory = fileOrDirctory;
            this.isReadOnly = isReadOnly;
            this.beginPiece = beginPiece;
            this.pieceLength = pieceLength;
        }

        public byte[] ToByte()
        {
            byte[] objectByte = new byte[9];
            objectByte[0] = Convert.ToByte(this.fileName1);
            objectByte[1] = Convert.ToByte(this.fileName2);
            objectByte[2] = Convert.ToByte(this.fileName3);
            objectByte[3] = Convert.ToByte(this.fileType1);
            objectByte[4] = Convert.ToByte(this.fileType2);
            objectByte[5] = this.fileOrDirctory;
            objectByte[6] = this.isReadOnly;
            objectByte[7] = this.beginPiece;
            objectByte[8] = this.pieceLength;
            return objectByte;
        }

        public int ToFileAttribute(byte[] objectByte)
        {
            fileName1 = Convert.ToChar(objectByte[0]);
            fileName2 = Convert.ToChar(objectByte[1]);
            fileName3 = Convert.ToChar(objectByte[2]);
            fileType1 = Convert.ToChar(objectByte[3]);
            fileType2 = Convert.ToChar(objectByte[4]);
            fileOrDirctory = objectByte[5];
            isReadOnly = objectByte[6];
            beginPiece = objectByte[7];
            pieceLength = objectByte[8];
            return 0;
        }
    }
    #endregion

    #region 文件位置
    public class Pointer // 文件位置
    {
        int dnum;
        int bnum;
    }
    #endregion

    #region 打开文件信息
    public class OFile // 打开文件信息
    {
        public string name; // 文件的绝对路径
        public byte attribute; // 文件属性
        public byte beginNum; // 起始盘块
        public byte length; // 盘块长度
        public byte flag; // 读写控制位
        public Pointer read; // 读文件位置
        public Pointer write; // 写文件位置
    }
    #endregion

    #region 打开文件登记表
    public class OpenFile // 打开文件登记表
    {
        OFile[] openFiles;
        int lenght;

        public OpenFile()
        {

        }
        OpenFile(int MAXFILECOUNT = 10)
        {
            openFiles = new OFile[MAXFILECOUNT];
            lenght = 0;
            for (int i = 0; i < 10; i++)
            {
                openFiles[i] = new OFile();
            }
        }
    }
    #endregion
}
