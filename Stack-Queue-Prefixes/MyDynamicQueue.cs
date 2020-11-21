using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class MyDynamicQueue<T> 
    {
        private int size;
        private Node head;
        private Node tail;

        public MyDynamicQueue()
        {
            this.head = null;
            this.tail = null;
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

        public void Enqueue(T element)  
        {
            Node freshNode=new Node(element);
            if (head==null)
            {
                head = tail = freshNode;
                size++;
            }
            else
            {
                tail.next = freshNode;
                tail = freshNode;
                size++;
            }
        }

        public T Dequeue()
        {
            Node tempNode = head;
            if (size>1)
            {
                head = head.next;
                size--;
            }
            else if(head==tail)
            {
                head = tail = null;
                size = 0;
            }

            return tempNode.data;
        }

        public T Peek() => head!=null?head.data:default;

        public T GetLast() => tail!=null?tail.data:default;
      
        public bool IsEmpty() => size == 0;
        public int Size() => size;

        public void DisplayQueue()
        {
            Node traverseNode = head;
            while (traverseNode!=null)
            {
                Console.Write(traverseNode.data+" ");
                traverseNode = traverseNode.next;
            }

            Console.WriteLine();
        }
    }
}
