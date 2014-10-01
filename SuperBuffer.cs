using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace supplier_user
{
    class SuperBuffer
    {
        Queue<int> buf = new Queue<int>();
        Semaphore semReader;
        int countReader;//количество читателей
        int sizeBuffer;
        Mutex mut;
        DataGridView dgv;
        public SuperBuffer(int countReader,int sizeBuffer, DataGridView dgv)
        {
            this.countReader = countReader;
            this.sizeBuffer = sizeBuffer;
            semReader = new Semaphore(countReader, countReader);
            mut = new Mutex();
            this.dgv = dgv;
        }
        //попробуй взять ( для потребителя)
        public void Pop()
        {
            object[] values = new object[3];
           
            semReader.WaitOne();//ждать семафора
            mut.WaitOne();//войти в критическую секцию
            values[0] = "";
            StringBuilder sb = new StringBuilder();            
            if (buf.Count != 0)                
                values[2] =Thread.CurrentThread.Name+" взял "+ buf.Dequeue();                
            else                
                values[2] = Thread.CurrentThread.Name+" не смог взять";
            foreach (int item in buf)
            {
                sb.Append(item + " ");
            }
            values[1] = sb.ToString();
            dgv.Rows.Add(values);            
            mut.ReleaseMutex();
            semReader.Release();            

        }
        //попробуй положить (для поставщика)
        public void Push() 
        {
            
            object[] values = new object[3];
            mut.WaitOne();
            values[2] = "";            
            if (buf.Count != sizeBuffer)
            {
                Random r = new Random();                   
                //поставщик кладет случайное колмичество 
                int n = r.Next(sizeBuffer - buf.Count);
                n = n == 0 ? 1 : n;
                values[0] = "Писатель положил ";
                while (n>0)
                {
                    int a = r.Next(100);
                    buf.Enqueue(a);
                    n--;
                    values[0]+= a.ToString()+" ";
                }
               
            }
            else            
                values[0] = "Писатель ничего не положил";
            StringBuilder sb = new StringBuilder();
            foreach (int item in buf)
            {
                sb.Append(item + " ");
            }
            values[1] = sb.ToString();
            dgv.Rows.Add(values);          
            mut.ReleaseMutex();
        }
             
    }
}
