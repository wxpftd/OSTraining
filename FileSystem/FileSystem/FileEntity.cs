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
        public FileStream file { get; private set; }
        public byte[] fileTable { get; private set; }
        public byte[] cache { get; private set; }
        public byte[] currentMem { get; private set; }

        public FileEntity()
        {
        }
        public int Init()
        {
            // 初始化落地文件
            file = new FileStream(@"./filetemp", FileMode.OpenOrCreate);
            file.Seek(0, SeekOrigin.Begin);
            if (file.Length == 0)
            {
                // 初始化位图
                fileTable = new byte[128];
                fileTable[0] = 255;
                fileTable[1] = 255;
                file.Write(fileTable, 0, 128);
            }
            else
            {
                // 载入位图
                fileTable = new byte[128];
                file.Read(fileTable, 0, 128);
            }
            file.Flush();
            return 0;
        }

        public int destroy()
        {
            file.Flush();
            file.Close();
            return 0;
        }

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
    }

    class fileAttribute
    {
        public string fileName { get; set;} // 文件名(目录名)
        public byte fileType { get; set; } // 文件类型
        public byte fileProperty { get; set; } // 文件属性
        public byte beginPiece { get; set; } // 起始盘块
        public byte pieceLength { get; set; } // 盘块数目
    }

    struct Pointer // 文件位置
    {
        int dnum;
        int bnum;
    }

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

    class fileStatus
    {
    }
}
