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
        }        
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
            SuperBuffer buf = new SuperBuffer(size, dgvAct);
            go = true;
            dgvAct.Rows.Clear();
            while (go)
            {
                Thread writer = new Thread(buf.Push);
                writer.Start();
                //случайно генерируем читателей 
                if (r.Next(2) == 0) //в 1/3 случае генерируем читателей
                {
                    int howmuch = r.Next(size);//сколько
                    Thread[] readers = new Thread[size];
                    for (int i = 0; i < howmuch; i++)
                    {
                        readers[i] = new Thread(buf.Pop);
                        readers[i].Name = "Читатель # " + i.ToString();
                        readers[i].Start();
                    }
                }
                Thread.Sleep(500);
                Application.DoEvents();               
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
