using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PageReplacement
{
    public partial class PageReplacement : Form
    {
        private static int[] addressStream;
        Queue<double> optResult;
        Queue<double> fifoResult;
        Queue<double> lruResult;
        public delegate int ReplaceMethod(int frameCount);
        public PageReplacement()
        {
            InitializeComponent();
            addressStream = new int[500];
            MainFunction();
            reflesh();
        }

        #region 主函数
        private void MainFunction()
        {
            optResult = new Queue<double>();
            fifoResult = new Queue<double>();
            lruResult = new Queue<double>();
            Random re = new Random();

            ReplaceMethod opt = new ReplaceMethod(OPT);
            ReplaceMethod fifo = new ReplaceMethod(FIFO);
            ReplaceMethod lru = new ReplaceMethod(LRU);

            for (int i = 0; i < 400; i++ )
            {
                //addressStream[i] = i % 40;
                addressStream[i] = re.Next(0, 399) % 40;            
            }
            // 统计每个算法的缺页率
            for (int i = 4; i <= 40; i++)
            {
                optResult.Enqueue((FrameAndMethodSeletion(i, opt) - i) / 400.0);
                fifoResult.Enqueue((FrameAndMethodSeletion(i, fifo) - i) / 400.0);
                lruResult.Enqueue((FrameAndMethodSeletion(i, lru) - i) / 400.0); 
            }
        }
        #endregion

        #region 委托函数
        private int FrameAndMethodSeletion(int i, ReplaceMethod rm)
        {
            return rm(i); 
        }
        #endregion

        #region OPT置换算法
        private static int OPT(int frameCount)
        {
            int frameLack = 0;
            int[] frame = new int[frameCount];
            for (int i = 0; i < frameCount; i++ )
            {
                frame[i] = -1;
            }
            for (int pos = 0; pos < 400; pos++)
            {
                bool isContain = false;
                isContain = IsPageInFrame(ref frame, frameCount, pos); 
                if (!isContain)
                {
                    frameLack++;
                    bool posEmpty = false;
                    for (int i = 0; i < frameCount; i++)
                    {
                        if (frame[i] == -1)
                        {
                            frame[i] = addressStream[pos];
                            posEmpty = true;
                            break;
                        }
                    }
                    if (!posEmpty)
                    {
                        int farOrNerver = -1;
                        //淘汰永不使用或下次访问距当前时间最长的页面
                        for (int i = 0; i < frameCount; i++)
                        {
                            for (int j = pos; j < 400; j++)
                            {
                                if (frame[i] == addressStream[j])
                                {
                                    if (farOrNerver < j)
                                    {
                                        farOrNerver = j;
                                    }
                                    break;
                                }
                            }
                            if (farOrNerver == -1)
                            {
                                frame[i] = addressStream[pos];
                                break;
                            }
                        }
                        if (farOrNerver != -1)
                        {
                            for (int i = 0; i < frameCount; i++)
                            {
                                if (frame[i] == addressStream[farOrNerver])
                                {
                                    frame[i] = addressStream[pos];
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return frameLack;
        }
        #endregion

        #region FIFO算法
        private static int FIFO(int frameCount)
        {
            int frameLack = 0;
            int currentFrame = 0;
            int[] frame = new int[frameCount];
            for (int i = 0; i < frameCount; i++ )
            {
                frame[i] = -1;
            }
            for (int pos = 0; pos < 400; pos++ )
            {
                currentFrame %= frameCount;
                bool isContain = false;
                isContain = IsPageInFrame(ref frame, frameCount, pos);
                if (!isContain)
                {
                    frameLack++;
                    //淘汰最先进的页
                    frame[currentFrame++] = addressStream[pos]; 
                }
            }
            return frameLack;
        }
        #endregion

        #region LRU算法
        private static int LRU(int frameCount)
        {
            int[] frame = new int[frameCount];
            for (int i = 0; i<frameCount; i++)
            {
                frame[i] = -1;
            }
            int frameLack = 0;
            for (int pos = 0; pos < 400; pos ++ )
            {
                bool isContain = false;
                isContain = IsPageInFrame(ref frame, frameCount, pos);
                if (!isContain)
                {
                    frameLack++;
                    bool isPosEmpty = false;
                    for (int i = 0; i < frameCount; i++)
                    {
                        if (frame[i] == -1)
                        {
                            frame[i] = addressStream[pos];
                            isPosEmpty = true;
                            break;
                        }
                    }
                    if (!isPosEmpty)
                    {
                        int disPos = 400;
                        int changePos = 0; 
                        for (int i = 0; i < frameCount; i++)
                        {
                            for (int j = pos - 1; j >= 0; j--)
                            {
                                if (frame[i] == addressStream[j])
                                {
                                    if (disPos > j)
                                    {
                                        changePos = i;
                                        disPos = j;
                                    }
                                    break;
                                }
                            }
                        }
                        frame[changePos] = addressStream[pos];
                    }
                }
            }
            return frameLack;
        }
        #endregion

        #region 帧内是否包含下一个页
        private static bool IsPageInFrame(ref int[] frame, int frameCount, int pos)
        {
            for (int i = 0; i < frameCount; i++)
            {
                if (frame[i] == addressStream[pos])
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 刷新页面,展示新的数据结果 
        private void reflesh()
        {
            // 删除旧数据
            this.listView1.Items.Clear();

            // 添加数据展示表格
            for (int row = 4; row <= 40; row++)
            {
                System.Windows.Forms.ListViewItem newListViewItem = new ListViewItem(new string[] { "#" + row, Convert.ToString(optResult.Dequeue()), Convert.ToString(fifoResult.Dequeue()), Convert.ToString(lruResult.Dequeue()) }, -1);
                this.listView1.Items.Add(newListViewItem);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            MainFunction();
            reflesh();
        }

    }
}
