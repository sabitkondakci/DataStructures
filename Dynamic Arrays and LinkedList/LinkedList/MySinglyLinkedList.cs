using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SinglyDoublyLinkedLists
{
    class MySinglyLinkedList<T>: ILinkedList<T> 
    where T : IEquatable<T>,IComparable<T>
    {
        private int size ;
        private Node head;
        private Node tail;

        public MySinglyLinkedList()
        {
            size = 0;
            head = null;
            tail = null;
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
        public void AddFirst(T element)   //Listeye soldan ekleme yapar
        {
            Node node = new Node(element);
            if (head==null)
            {
                head = tail = node;
                size++;
                return;
            }
            node.next = head;
            head = node;
            size++;
        }
        public void AddLast(T element)  //Listeye sagdan ekleme yapar
        {
            Node node = new Node(element);
            if (head==null)       //Liste bos durumdaysa head ve tail ayni nesneyi temsil eder, null
            {
                head= tail=node;
                size++;
                return;
            }
            
            tail.next = node;
            tail = node;
            size++;
        }
        public T RemoveFirst()   //Listedeki ilk elemanin kaldirir ve geriye kaldirilan degeri dondurur
        {
            
            if (IsEmpty())    //Listenin bos olup olmadigi kontrol edilir.
            {
                return default;
            }

            T temp = head.data;

            if (head==tail)     //head==tail durumu varsa Listede tek bir deger vardir
            {
                head = tail = null; //degerler null yapilarak kaldirma islemi tamamlanir
            }
            else
            {
                head = head.next;  //garbage collector stack de referansi olmayan ilk nesneyi listeden siler
            }

            size--;
            return temp;
        }
        public T RemoveLast()  //Listdeki son elemani kaldirir ve geriye silinen degeri dondurur
        {
            if (IsEmpty())   //Listenin doluluk durumu kontrol edilir
            {
                return default;
            }

            T data;

            if (head==tail)  //Listenin tek elemanli oldugu durumun kontrolu.
            {
                data = head.data;
                head = tail = null;
                size--;
                return data;
            }

            Node current = head, previous = null; //Current ve Previous olmak uzere iki pointer tanimlanir
            while (current!=tail)
            {
                previous = current;     //Pointerlar listede ardisira hareket eder
                current = current.next; //Ilk durumda previous=null , son durumda current.next=null
            }

            data = current.data;
            previous.next = null;    //previous=current isleminden sonra, previous.next=null yapilir
            tail = previous;         //previous son listedeki son eleman oldugundan tail e atama yapilir.
            size--;
            return data;
        }
        public T Remove(T element)
        {
            T data;

            if (IsEmpty())
                return default;

            Node current = head, previous = null; //Current ve Previous listede senkronize ilerler
            while (current!=null)   //Listenin tumu kontrol edilir, current=null durumu listenin tarandigi anlamini tasir.
            {
                if (current.data.CompareTo(element)==0)   //nesnelerin esitlik kontrolu yapilir.
                {
                    if (current==head)    //Aranan deger listenin basindaysa
                    {
                        return RemoveFirst();
                    }

                    if (current==tail)  //Aranan deger listenin sonundaysa
                    {
                        return RemoveLast();
                    }

                    data = current.data;
                    previous.next = current.next; // Aranan nesnenin referans degeri takip eden ifadenin referans degerine aktarilir
                    size--;
                    return data;
                }

                previous = current;      //Previous ve Current pointerlarla senkronize arama yapilir.
                current = current.next;
            }

            return default;
        }
        public void Reverse()
        {
            if(IsEmpty())
                throw new Exception("Liste Bos!");
            Node currentNode = head, nextNode = head, prevNode = null, tempHead = head;
            while (nextNode!=null)
            {
                nextNode = nextNode.next;
                currentNode.next = prevNode;
                prevNode = currentNode;
                currentNode = nextNode;
            }

            head = prevNode;
            tail = tempHead;
        }
        public T PeekFirst() => head == null ? default : head.data;
        public T PeekLast() => tail == null ? default : tail.data;
        public T GetValueAtPosition(int position)
        {
            if (head == null)
                throw new Exception("LinkedList is empty");
            if (position >= size)
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
        public void AddAtPosition(T data, int position)
        {
            if (head == null)
                throw new Exception("LinkedList is empty");
            if (position >= size || position<0)
            {
                throw new IndexOutOfRangeException("Index starts with '0'! and position can't be negative");
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
            tempNode.next = freshNode;
            size++;

        } //Pozisyon 0 dan baslar!
        public bool Contain(T element)
        {
            Node node = head;
            while (node!=null)
            {
                if (node.data.CompareTo(element)==0)
                {
                    return true;
                }

                node = node.next;
            }

            return false;
        }
        public int Size() => size;
        public bool IsEmpty() => size == 0;
        public void ShowList()   //Liste elemanlarini konsolda gosterir.
        {
            Node node = head;
            Console.Write("LinkedList:");
            while (node!=null)
            {
                Console.Write(node.data);
                Console.Write(" ");
                node = node.next;
            }
            
        }

    }
}
