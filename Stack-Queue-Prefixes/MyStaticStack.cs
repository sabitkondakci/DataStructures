using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class MyStaticStack<T>
    {
        private int size;
        private T[] array;
        private int counter;

        public MyStaticStack(int volume)
        {
            this.size = volume;
            array=new T[volume];
            this.counter = 0;
        }
        public void Push(T data)
        {
            if (counter < size && counter >=0)
            {
                array[counter] = data;
                counter++;
            }
            else
            {
                throw new StackOverflowException($"Stack is full, MaxSize is {size}");
            }
        }
        public T Pop()
        {
            if (counter > 0)
            {
                --counter;
                return array[counter];
            }

            throw new Exception("Stack is empty");
        }
        public T Peek()
        {
            if (counter>0)
            {
                return array[counter - 1];
            }

            return default;
        }
        public bool IsEmpty() => counter == 0;

        public void Display()
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write(array[i]+" ");
            }

            Console.WriteLine();
        }

        public bool IsFull() => counter == size;

        public int Size() => size;

    }
}
