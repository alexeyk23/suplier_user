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
        Semaphore semFull;//семафор потребителей
        Semaphore semEmpty;//семафор писателей
        int sizeBuffer;
        Mutex mut;
        DataGridView dgv;
        delegate void AddRowInDgv(object[] values);
        public SuperBuffer(int sizeBuffer, DataGridView dgv)
        {
            this.sizeBuffer = sizeBuffer;
            semEmpty = new Semaphore(sizeBuffer, sizeBuffer);
            semFull = new Semaphore(0, sizeBuffer);
            mut = new Mutex();
            this.dgv = dgv;
        }
        
        //попробуй взять ( для потребителя)
        public void Pop()
        {
            object[] values = new object[3];           
            semFull.WaitOne();//ждать семафора
            mut.WaitOne();//войти в критическую секцию
                values[0] = "";                          
                values[2] =Thread.CurrentThread.Name+" взял "+ buf.Dequeue();
                StringBuilder sb = new StringBuilder();
                foreach (int item in buf)
                {
                    sb.Append(item + " ");
                }
                values[1] = sb.ToString();
                addRow(values);      
            mut.ReleaseMutex();
            semEmpty.Release();            

        }
        //попробуй положить (для поставщика)
        public void Push() 
        {
            
            object[] values = new object[3];
            semEmpty.WaitOne();
            mut.WaitOne();
                values[2] = ""; 
                Random r = new Random();                      
                int a = r.Next(100); 
                buf.Enqueue(a);
                values[0] = "Писатель положил "+a.ToString();    
                StringBuilder sb = new StringBuilder();
                foreach (int item in buf)
                {
                    sb.Append(item + " ");
                }
                values[1] = sb.ToString();
                addRow(values);            
            mut.ReleaseMutex();
            semFull.Release();
        }
        //потокобезопасное добавдение в датагрид
        private void addRow(object[] v)
        {
            if (dgv.InvokeRequired)
            {
                AddRowInDgv dAdd = new AddRowInDgv(addRow);
                dgv.Invoke(dAdd, new object[]{v});
            }
            else
                dgv.Rows.Add(v);
        }
    }
}
