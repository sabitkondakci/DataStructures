using System;
using System.Collections.Generic;

namespace SortingAlgorithms
{
    // Heap
    // Parent is > Children  MAXHEAP
    // Parent is < Children  MINHEAP
    // Log2(n+1) - 1 yiginin ve agacin yuksekligini ifade eden genel formul
    // n:Belirli yukseklikteki eleman sayisi

    // Children: 2xParent + 1 , 2xParent +2 , Root = 0. index relationship
    // Parent  : Math.Floor((Position Of Child - 1)/2)    , Root=0. index relationship
    class MyBinaryHeap<T>
    {
        private  int lastPosition;    // Heap'in derinligi
        private  T[] array;
        public MyBinaryHeap(int size) // Log2(size+1) - 1 = agacin belirlenen yuksekligi
        {
            array = new T[size];
            lastPosition = 0;
        }
        
        public void Add(T value)  
        {
            array[lastPosition] = value; //Son indekse veri yerlestirilir
            TrickleUp(lastPosition);//Verinin konulan yere uygunluguna bakilir, uygun degilse Swap()'la degistirilir
            lastPosition++;
        }
        
        public T[] Heapify_Max(T[] heapArray)
        {
            if(heapArray==null)
                throw new NullReferenceException("Heapify_Max(T[] heapArray)");
            int i = 0;
            while (i < heapArray.Length )
            {
                HeapifyTrickleUpMax(heapArray,i);
                i++;
            }

            return heapArray;
        } // Heap Tree turunde olmayan siradan diziyi MaxHeap Dizi'ye donusturur.
        
        public T[] Heapify_Min(T[] heapArray) // Heap Tree turunde olmayan siradan diziyi  MinHeap Dizi'ye donusturur.
        {
            if (heapArray == null)
                throw new NullReferenceException("Heapify_Min(T[] heapArray)");

            int i = 0;
            while (i < heapArray.Length)
            {
                HeapifyTrickleUpMin(heapArray, i);
                i++;
            }

            return heapArray;
        }
        
        private void TrickleUp(int position) //Uste dogru pozisyon degistirme, Cocuk'tan Ebeveyne(Child,Parent)
        {
            if (position == 0)
                return;

            int parent = (position - 1) / 2; //Cocugun hangi ebeveyne ait olduguna bakilir
            if (Comparison(array[position],array[parent])>0)
            {   //array[position]>array[parent] ise yerleri degistirilir.
                Swap(position, parent);
                TrickleUp(parent);  //Fonksiyon tekrar cagrilarak islem tekrarlanir, ta ki array[pozisyon]!>array[parent] olana dek islem devam eder
            }

            //Compare farkli tipteki verilerin karsilastirilmasinda kullanilir
            //Comparer<T> generic sinifina aittir, T : karsilastirilmak istenen veri tipi
            //Comparison metodu ustte belirtilen ifadeyi barindiran , islem kolayligi saglayan fonksiyondur.
        }
        
        private void HeapifyTrickleUpMax(T[] heapArray,int position) //Uste dogru pozisyon degistirme, Cocuk'tan Ebeveyne(Child,Parent)
        {
            if (position == 0)
                return;

            int parent = (position - 1) / 2; 
            if (Comparison(heapArray[position], heapArray[parent]) > 0)
            {   
                HeapifySwap(heapArray,position, parent);
                HeapifyTrickleUpMax(heapArray,parent);  
            }

        }
        
        private void HeapifyTrickleUpMin(T[] heapArray, int position) //Uste dogru pozisyon degistirme, Cocuk'tan Ebeveyne(Child,Parent)
        {
            if (position == 0)
                return;

            int parent = (position - 1) / 2;
            if (Comparison(heapArray[position], heapArray[parent]) < 0)
            {
                HeapifySwap(heapArray, position, parent);
                HeapifyTrickleUpMin(heapArray, parent);
            }

        }
        
        private void Swap(int a, int b) //Dizi bir nesne oldugu icin referans degerleri degistirilir.
        {
            T temp = array[a];
            array[a] = array[b];  
            array[b] = temp;
        }
        
        private void HeapifySwap(T[] heapArray,int a, int b)
        {
            T temp = heapArray[a];
            heapArray[a] = heapArray[b];
            heapArray[b] = temp;
        }
        
        public T Poll()  //0.Indeks nesnesini kaldirir.
        {
            T temp = array[0];
            Swap(0, lastPosition); // Kaldirma islemi 0. indeksteki sayi son indekse tasinarak yapilir
            lastPosition--;    //lastPosition-- azalilarak son pozisyondaki nesneye ait referans bosta kalir.
            TrickleDown(0);   // Ebeveyn'den Cocuga dogru 0. indeksteki deger heap kurallarina uyumlu bir sekilde tasinir.
            return temp;
        }
        
        public T RemoveAt(int key)  //Breadh First Search yapidaki indeks secilir.
        {
            T temp = array[key];
            Swap(key, lastPosition); 
            lastPosition--;    //lastPosition-- azalilarak son pozisyondaki nesneye ait referans bosta kalir ve garbage collector tarafindan silinir.
            TrickleDown(key);   // Ebeveyn'den Cocuga dogru key. indeksteki deger heap kurallarina uyumlu bir sekilde tasinir.
            return temp;
        }
        
        private void TrickleDown(int parent)
        {
            int left = 2 * parent + 1;  //Ebeveyn'e ait sol indeks
            int right = 2 * parent + 2; //Ebeveyn'e ait sag indeks

            if (left == lastPosition && Comparison(array[parent], array[left]) < 0)
            {
                Swap(parent, left);
                return;
            }

            if (right == lastPosition && Comparison(array[parent], array[right]) < 0)
            {
                Swap(parent, right);
                return;
            }

            if (left > lastPosition || right > lastPosition)
            {
                return;
            }

            if (Comparison(array[left], array[right]) > 0 && Comparison(array[parent], array[left]) < 0)
            {
                Swap(parent, left);
                TrickleDown(left);  //Metod tekrar cagrilarak islem tekrarlanir.
            }
            else 
            {
                Swap(parent, right);
                TrickleDown(right);
            }

        }
        
        private int Comparison(T x,T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        } //Generic yapidaki nesneleri karsilastirmak icin kullanilan metod. 
        
        public T[] SortedHeapList()
        {
            int internalLastPosition = lastPosition;
            T[] tempArray=new T[internalLastPosition];
            for (int i = 0; i < internalLastPosition; i++)
            {
                tempArray[i] = Poll();  //Tasarlanan heap MAXHEAP oldugundan, ilk deger her zaman en buyuktur.
            }

            return tempArray;
        } // nlogn (log 2 tabaninda) zaman karmasasi olan SortedHeapList metodu

    }
}
