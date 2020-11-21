using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class DoubleEndedQueueArray<T>
    {
        //Double Ended Queue , DEQUE olarak da isimlendirilir.
        //Hem baştan hem de sondan ekleme, silme işlemlerinin yapılabildiği veri yapısıdır.
        //Dinamik olan tasarımı, doubly circular link list olarak isimlendirilir.
        //Head - Rear arasındaki değerler listeyi oluşturur.

        private T[] array;
        private int size;
        private int head;
        private int rear;
        public DoubleEndedQueueArray(int size)
        {
            this.size = size;
            array = new T[size];
            this.head = -1;
            this.rear = -1;
        }
        public void InsertToHead(T data)
        {
            if ((head == 0 && rear == size-1) || head==rear+1) // Baştan ekleme için doluluk durumu kontrölü
                throw new Exception("Double Ended Queue is full");

            else if (IsEmpty())
            {
                head = rear = 0;
                array[head] = data;
            }
            else if (head==0)
            {
                head = size - 1;
                array[head] = data;
            }
            else
            {
                head--;
                array[head] = data;
            }
        } //Array'a baştan ekleme yapar
        public void InsertToRear(T data)
        {
            if (IsEmpty()) // Kuyrukta hiç bir eleman yoksa, head ve rear indeks değerleri 0'a eşitlenir.
            {
                head = rear = 0;
                array[rear] = data;
            }
            else if (IsFull())
            {
                throw new Exception("Circular Queue is full");
            }
            else
            {
                rear = (rear + 1) % size; //back indeksi 1 arttırılır, son indekse gelince 0. indekse geri gidilir.
                array[rear] = data;
            }
        }//Array'a sondan ekleme yapar
        public T DeleteFromHead()
        {
            if (IsEmpty())
            {
                throw new Exception("Circular Queue is empty");
            }
            else if (head == rear) //(back + 1) % size == head ise kuyrukta tek eleman vardır.
            {
                T temp = array[head];
                head = rear = -1;
                return temp;
            }
            else
            {
                T temp = array[head];
                head = (head + 1) % size; // head indeksi 1 arttırılır, dizi sonunda işlem başa döner.
                return temp;
            }

        }
        public T DeleteFromRear()
        {
            T temp = array[head];

            if (IsEmpty())
            {
                throw new Exception("Circular Queue is empty");
            }
            else if (head == rear) //(back + 1) % size == head ise kuyrukta tek eleman vardır.
            {
                head = rear = -1;
            }
            else if (rear==0)
            {
                rear = size - 1;
            }
            else 
            {
                head--;
            }

            return temp;
        }
        public T Peek() => array[head];
        public T GetLast() => array[rear];
        public bool IsFull() => (rear + 1) % size == head;
        public bool IsEmpty() => head == -1;

        public void Display()
        {
            for (int i = head; i <= rear; i=(i+1)%size)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine();
        }
    }
}
