using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinglyDoublyLinkedLists
{
    interface ILinkedList<T>
    {
        void AddFirst(T element);
        void AddLast(T element);
        T RemoveFirst();
        T RemoveLast();
        T Remove(T element);
        T PeekFirst();
        T PeekLast();
        bool Contain(T element);
        int Size();
        bool IsEmpty();
        void ShowList();


    }
}
