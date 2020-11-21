using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class MyCircularQueueArray<T>
    {
        //head=>[a,b,c,d]<=back
        //back=(back+1)%size  __________ head=(head+1)%size     {indeks düzeni}

        private T[] array;
        private int size;
        private int head;
        private int back;
        public MyCircularQueueArray(int size)
        {
            this.size = size;
            array=new T[size];
            this.head = -1;
            this.back = -1;
        }
        public void Enqueue(T data)
        {
            if (IsEmpty()) // Kuyrukta hiç bir eleman yoksa, head ve back indeks değerleri 0'a eşitlenir.
            {
                head = back = 0;
                array[back] = data;
            }
            else if (IsFull()) 
            {
                throw new Exception("Circular Queue is full");
            }
            else
            {
                back = (back + 1) % size; //back indeksi 1 arttırılır.
                array[back] = data;
            }
        }
        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new Exception("Circular Queue is empty");
            }
            else if (head==back) //(back + 1) % size == head ise kuyrukta tek eleman vardır.
            {
                T temp = array[head];
                head = back = -1;
                return temp;
            }
            else
            {
                T temp = array[head];
                head=(head+1)%size; // head indeksi 1 arttırılır.
                return temp;
            }

        }
        public T Peek() => array[head];
        public bool IsFull() => (back + 1) % size == head;
        public bool IsEmpty() => head == -1;

        public void Display()  
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write(array[i]+" ");
            }

            Console.WriteLine();
        }
    }
}
