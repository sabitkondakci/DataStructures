using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class MyDynamicStack<T> 
    {
        private int size;
        private Node head;

        public MyDynamicStack()
        {
            this.head = null;
            this.size = 0;
        }
        private class Node
        {
            public T data;
            public Node next;

            public Node(T data)
            {
                this.data = data;
                this.next = null;
            }

        }
        public void Push(T element)  
        {
            Node node = new Node(element);
            if (head == null)
            {
                head = node;
                size++;
                return;
            }
            node.next = head;
            head = node;
            size++;
        }
        public T Pop()  
        {
            if (IsEmpty())    
            {
                return default;
            }

            T temp = head.data;

            if (head.next==null)    
            {
                head = null; 
            }
            else
            {
                head = head.next; 
            }

            size--;
            return temp;
        }
        public T Peek() => head == null ? default : head.data;
        public bool IsEmpty() => size == 0;
        public int Size() => size;
        public void Display()   
        {
            Node node = head;
            Console.Write("Stack:");
            while (node != null)
            {
                Console.Write(node.data);
                Console.Write(" ");
                node = node.next;
            }

        }

    }
}
