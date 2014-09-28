using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace supplier_user
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        int countReader = 5;
        Random r = new Random();
        int size = 5;
        private void btnGO_Click(object sender, EventArgs e)
        {
            SuperBuffer buf = new SuperBuffer(countReader,size,dgvAct);
            int w = 20;
            while (w>0)
            {
                Thread writer = new Thread(buf.Push);
                writer.Start();
                //случайно генерируем читателей 
                int howmuch = r.Next(countReader);//сколько
                Thread[] readers = new Thread[countReader];
                for (int i = 0; i < howmuch; i++)
                {
                    readers[i] = new Thread(buf.Pop);
                    readers[i].Name = "Читатель # " + i.ToString();
                    readers[i].Start();
                }
                Application.DoEvents();
                w--;
            }                     
        }
    }
}
