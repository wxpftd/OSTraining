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
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            char[] buffer = new char[10000];
            int startPiece = FileSystem.oneOpenFile.beginNum;
            int pos = 0;
            do
            {
                for (int i = 0; i < 72; ++i)
                {
                    buffer[pos] = Convert.ToChar(FileSystem.fe.cache[startPiece][i]);
                    ++pos;
                }
            }
            while ((startPiece = FileSystem.fe.fileTable[startPiece]) != 255);
            this.Content.Text = new string(buffer);
            if (FileSystem.oneOpenFile.flag == 0)
            {
                this.Content.Enabled = false;
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            char[] buffer = this.Content.Text.ToCharArray();
            int pieceCount = buffer.Length / 72;
            if (buffer.Length > pieceCount * 72)
            {
                ++pieceCount;
            }
            int pos = 0;
            int startPiece = FileSystem.oneOpenFile.beginNum;
            int lastPiece;
            for (int i = 1; i <= pieceCount; ++i)
            {
                int tempPos = 0;
                if (i == pieceCount)
                    tempPos = buffer.Length % 72;
                else
                    tempPos = 72;
                for (int j = 0; j < tempPos; ++j)
                {
                    FileSystem.fe.cache[startPiece][j] = Convert.ToByte(buffer[pos]);
                    ++pos;
                }
                lastPiece = startPiece;
                startPiece = FileSystem.fe.fileTable[startPiece];
                if (pieceCount > i && startPiece == 255)
                {   
                    int diskPiece; 
                    for (diskPiece = 0; diskPiece < 128; ++diskPiece )
                    {
                        if (FileSystem.fe.fileTable[diskPiece] == 0)
                        {
                            startPiece = diskPiece;
                            FileSystem.fe.fileTable[lastPiece] = Convert.ToByte(diskPiece);
                            FileSystem.fe.fileTable[diskPiece] = 255;
                            break;
                        }
                    }
                    if (diskPiece == 128)
                    {
                        MessageBox.Show("磁盘已满，你的内容放不下啦啦啦.");
                        return;
                    }
                }
            }
            FileSystem.fe.dataLand();
        }

        private void CloseFile_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] buffer = this.Content.Text.ToCharArray();
            int pieceCount = buffer.Length / 72;
            if (buffer.Length > pieceCount * 72)
            {
                ++pieceCount;
            }
            int pos = 0;
            int startPiece = FileSystem.oneOpenFile.beginNum;
            int lastPiece;
            for (int i = 1; i <= pieceCount; ++i)
            {
                int tempPos = 0;
                if (i == pieceCount)
                    tempPos = buffer.Length % 72;
                else
                    tempPos = 72;
                for (int j = 0; j < tempPos; ++j)
                {
                    FileSystem.fe.cache[startPiece][j] = Convert.ToByte(buffer[pos]);
                    ++pos;
                }
                lastPiece = startPiece;
                startPiece = FileSystem.fe.fileTable[startPiece];
                if (pieceCount > i && startPiece == 255)
                {
                    int diskPiece;
                    for (diskPiece = 0; diskPiece < 128; ++diskPiece)
                    {
                        if (FileSystem.fe.fileTable[diskPiece] == 0)
                        {
                            startPiece = diskPiece;
                            FileSystem.fe.fileTable[lastPiece] = Convert.ToByte(diskPiece);
                            FileSystem.fe.fileTable[diskPiece] = 255;
                            break;
                        }
                    }
                    if (diskPiece == 128)
                    {
                        MessageBox.Show("磁盘已满，你的内容放不下啦啦啦.");
                        return;
                    }
                }
            }
            FileSystem.fe.dataLand();
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
