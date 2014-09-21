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
    public partial class Form1 : Form
    {
        public static delegate void ReplaceMethod();
        public Form1()
        {
            InitializeComponent();
        }

        private void MainFunction()
        {
            Random re = new Random();
            int[] address = new int[500];

            ReplaceMethod opt = new ReplaceMethod(OPT);
            ReplaceMethod fifo = new ReplaceMethod(FIFO);
            ReplaceMethod lru = new ReplaceMethod(LRU);

            for (int i = 0; i < 400; i++ )
            {
                address[i] = re.Next(0, 400);            
            }
            for (int i = 4; i <= 40; i++)
            {
                FrameAndMethodSeletion(i, opt);
                FrameAndMethodSeletion(i, fifo);
                FrameAndMethodSeletion(i, lru);
            }
            
        }

        private void FrameAndMethodSeletion(int i, ReplaceMethod rm)
        {
            
        }

        private static void OPT()
        {

        }

        private static void FIFO()
        {

        }

        private static void LRU()
        {

        }
    }
}
