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
        int size = 5;//размер буфера
        bool go = true;
        private void btnGO_Click(object sender, EventArgs e)
        {
            Thread main = new Thread(run);
            main.Start();
        }
        void run()
        {
            SuperBuffer buf = new SuperBuffer(countReader, size, dgvAct);
            int w = 20;
            go = true;
            dgvAct.Rows.Clear();
            while (go)
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
                Thread.Sleep(500);
                Application.DoEvents();
                w--;
            }        
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            go= false;
        }
        //прокрутка в конец датагрида
        private void dgvAct_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgvAct.FirstDisplayedScrollingRowIndex = dgvAct.Rows.Count - 1;
        }

    }
}
