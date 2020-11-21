using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglyDoublyLinkedLists
{
    class MyDoublyLinkedList<T> : ILinkedList<T> where T : IEquatable<T>, IComparable<T>
    {
        private int size;
        private Node head;
        private Node tail;

        private class Node
        {
            public T data;
            public Node next;
            public Node prev;

            public Node(T data)
            {
                this.data = data;
                this.next = null;
                this.prev = null;
            }
        }

        public void AddFirst(T element)
        {
            Node freshNode = new Node(element);
            if (IsEmpty())
            {
                head = tail = freshNode;
                size++;
                return;
            }

            freshNode.next = head;
            head.prev = freshNode;
            head = freshNode;
            size++;
        }

        public void AddLast(T element)
        {
            Node freshNode = new Node(element);
            if (IsEmpty())
            {
                head = tail = freshNode;
                size++;
                return;
            }

            tail.next = freshNode;
            freshNode.prev = tail;
            tail = freshNode;
            size++;
        }

        public void AddAtPositionFromHead(T data, int position)
        {
            if (head == null)
                throw new Exception("LinkedList is empty");
            if (position >= size || position<0)
            {
                throw new IndexOutOfRangeException("Index starts with '0'!");
            }

            if (position == 0)
            {
                AddFirst(data);
                return;
            }

            Node freshNode = new Node(data);
            Node tempNode = head;

            for (int i = 1; i < position; i++)
            {
                tempNode = tempNode.next;
            }

            freshNode.next = tempNode.next;
            tempNode.next.prev = freshNode;
            freshNode.prev = tempNode;
            tempNode.next = freshNode;
            size++;

        } //Pozisyon 0 dan baslar!

        public void AddAtPositionFromTail(T data, int position)
        {
            if (head == null)
                throw new Exception("LinkedList is empty");
            if (position >= size || position<0)
            {
                throw new IndexOutOfRangeException("Index starts with '0'!");
            }

            if (position == 0)
            {
                AddLast(data);
                return;
            }

            Node freshNode = new Node(data);
            Node tempNode = tail;

            for (int i = 1; i < position; i++)
            {
                tempNode = tempNode.prev;
            }

            freshNode.prev = tempNode.prev;
            tempNode.prev.next = freshNode;
            freshNode.next = tempNode;
            tempNode.prev = freshNode;
            size++;

        }

        public T GetValueAtPositionFromHead(int position)
        {
            if (head == null)
                throw new Exception("LinkedList is empty");
            if (position >= size || position<0)
            {
                throw new IndexOutOfRangeException("Index starts with '0'!");
            }

            Node tempNode = head;

            for (int i = 0; i < position; i++)
            {
                tempNode = tempNode.next;
            }

            return tempNode.data;
        }
        public T GetValueAtPositionFromTail(int position)
        {
            if (head == null)
                throw new Exception("LinkedList is empty");
            if (position >= size || position<0)
            {
                throw new IndexOutOfRangeException("Index starts with '0'!");
            }

            Node tempNode = tail;

            for (int i = 0; i < position; i++)
            {
                tempNode = tempNode.prev;
            }

            return tempNode.data;
        }
        public bool Contain(T element)
        {
            Node checkNode = head;
            while (checkNode!=null)
            {
                if (checkNode.data.CompareTo(element)==0)
                {
                    return true;
                }

                checkNode = checkNode.next;
            }

            return false;
        }
        public bool IsEmpty() => size == 0;
        public T PeekFirst() =>head==null?default: head.data;
        public T PeekLast() => tail == null ? default : tail.data;
        public T Remove(T element)
        {
            if (IsEmpty())
                return default;

            Node tempNode = head;
            while (tempNode != null)
            {
                if (tempNode.data.CompareTo(element) == 0) //nesnelerin esitlik kontrolu yapilir.
                {
                    if (tempNode == head) //Aranan deger listenin basindaysa
                    {
                        return RemoveFirst();
                    }

                    if (tempNode == tail) //Aranan deger listenin sonundaysa
                    {
                        return RemoveLast();
                    }

                    tempNode.prev.next = tempNode.next;
                    tempNode.next.prev = tempNode.prev;
                    return tempNode.data;

                }
                tempNode = tempNode.next;
            }

            return default;
        }
        public T RemoveFirst()
        {
            if (IsEmpty())    //Listenin bos olup olmadigi kontrol edilir.
            {
                return default;
            }

            T temp = head.data;

            if (head == tail)    
            {
                head = tail = null; 
            }
            else
            {
                head = head.next;
                head.prev = null;
            }

            size--;
            return temp;
        }
        public T RemoveLast()
        {
            if (IsEmpty())   //Listenin doluluk durumu kontrol edilir
            {
                return default;
            }
            T data = tail.data;
            if (head == tail)  //Listenin tek elemanli oldugu durumun kontrolu.
            {
                head = tail = null;
            }
            else
            {
                tail = tail.prev;
                tail.next = null;
            }
            
            size--;
            return data;
        }

        public void Reverse()
        {
            Node currentNode = head;
            Node nextNode=null;
            if (!IsEmpty() || head!=tail)
            {
                while (currentNode!=null)
                {
                    nextNode = currentNode.next;
                    currentNode.next = currentNode.prev;
                    currentNode.prev = nextNode;
                    currentNode = nextNode;
                }

                nextNode = tail;
                tail = head;
                head = nextNode;
            }
        }
        public void ShowList()
        {
            Node traverse = head;
            while (traverse != null)
            {
                Console.Write(traverse.data + " ");
                traverse = traverse.next;
            }

        }
        public int Size() => size;
    }
}

