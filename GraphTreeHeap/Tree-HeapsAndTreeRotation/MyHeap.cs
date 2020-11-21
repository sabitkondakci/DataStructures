using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TreeAndHeaps
{
    // Heap
    //[2^(l+1) - 1] l. levelde toplam maksimum eleman sayisi, 1.level = 3 , 2.level=7 
    //[2^(l+1) - 1]==> 2^0 +2^1 +2^2 + ..... + 2^l

    // Parent is > Children  MAXHEAP
    // Parent is < Children  MINHEAP
    // Log2(n+1) - 1 yiginin ve agacin yuksekligini ifade eden genel formul
    // n:Belirli yukseklikteki eleman sayisi

    // Children: 2xParent + 1 , 2xParent +2 , Root = 0. index relationship
    // Parent  : Math.Floor((Position Of Child - 1)/2)    , Root=0. index relationship
    class MyHeap<T> 
    {
        private int lastPosition;    
        public T[] array;
        private T[] tempArray;
        private int counter;
        public MyHeap(int size)
        {
            array = new T[size];
            tempArray = new T[size];
            lastPosition = 0;
            counter = 0;
        }
        //Heap Dizi'yi farklı türlerde indeksleme işlemleri
        public T[] InOrderTraversal(int rootNode = 0)
        {
            //Left,Node,Right türünde indeksleme
            int trvLeftIndex = 2 * rootNode + 1;
            int trvRightIndex = 2 * rootNode + 2;
            if (trvLeftIndex < array.Length)
            {
                InOrderTraversal(trvLeftIndex); //Left
            }

            tempArray[counter] = array[rootNode]; //Node
            counter = (counter + 1) % array.Length;

            if (trvRightIndex < array.Length)
            {
                InOrderTraversal(trvRightIndex); //Right
            }

            return tempArray;
        }
        public T[] PreOrderTraversal(int rootNode=0)
        {
            //Node,Left,Right türünde indeksleme
            tempArray[counter] = array[rootNode]; //Node
            counter = (counter + 1) % array.Length;
            int trvLeftIndex = 2 * rootNode + 1;
            int trvRightIndex = 2 * rootNode + 2;
            if (trvLeftIndex<array.Length)
            {
                PreOrderTraversal(trvLeftIndex); //Left
            }
            
            if (trvRightIndex<array.Length )
            {
                PreOrderTraversal(trvRightIndex); //Right
            }

            return tempArray;
        }
        public T[] PostOrderTraversal(int rootNode = 0)
        {
            //Left,Right,Node türünde indeksleme
            int trvLeftIndex = 2 * rootNode + 1;
            int trvRightIndex = 2 * rootNode + 2;
            if (trvLeftIndex < array.Length)
            {
                PostOrderTraversal(trvLeftIndex); //Left
            }

            if (trvRightIndex < array.Length)
            {
                PostOrderTraversal(trvRightIndex); //Right
            }
            tempArray[counter] = array[rootNode]; //Node
            counter = (counter + 1) % array.Length;
            return tempArray;
        }
        public void Add(T value)  
        {
            array[lastPosition] = value; //Son indekse veri atanır
            TrickleUp(lastPosition);//Verinin bölgesel uygunluğu test edilir, düzeltme için Swap() metodu kullanılır.
            lastPosition++;
        }

        private void TrickleUp(int position) //Yukarı yönlü konumlandırma işlemi yapılır
        {
            if (position == 0)
                return;

            int parent = (position - 1) / 2; //Soy(Parent) kontrolü yapılır
            if (Comparison(array[position], array[parent]) > 0)
            {
                //array[position]>array[parent] durumunda Swap() metodu çağrılır.
                Swap(position, parent);
                TrickleUp(parent);
            }

            //Compare farkli tipteki verilerin karsilastirilmasinda kullanilir
            //Comparer<T> generic sinifina aittir, T : karsilastirilmak istenen veri tipi
            //Comparison metodu ustte belirtilen ifadeyi barindiran , islem kolayligi saglayan fonksiyondur.
        }

        private void Swap(int a, int b) //Array referans değer tipine sahiptir, referans değişimi
        {
            T temp = array[a];
            array[a] = array[b];  
            array[b] = temp;
        }

        public T Remove() 
        {
            T temp = array[0];
            Swap(0, lastPosition); // Kök düğümündeki değer son indeksle değiştirilir
            lastPosition--;   
            TrickleDown(0);   // Kök değerine atanan verinin bölgesel uygunluğu test edilir.
                              // Sıralama MAXHEAP türüne sahip olduğundan en büyük değer Kök düğümde depolanır.
            return temp;
        }

        private void TrickleDown(int parent)
        {

            int left = 2 * parent + 1;  //Soy'a ait sol indeks
            int right = 2 * parent + 2; //Soy'a ait sağ indeks

            if (left == lastPosition && Comparison(array[parent],array[left])<0)
            {
                Swap(parent, left); 
                return;
            }

            if (right == lastPosition && Comparison(array[parent],array[right])<0)
            {
                Swap(parent, right);
                return;
            }

            if (left >= lastPosition || right >= lastPosition)
            {
                return;
            }

            if (Comparison(array[left],array[right]) > 0 &&
                Comparison(array[parent],array[left]) < 0)
            {
                Swap(parent, left);
                TrickleDown(left);  
            }
            else if (Comparison(array[parent],array[right]) < 0)
            {
                Swap(parent, right);
                TrickleDown(right);
            }

        }

        private int Comparison(T x,T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        }

        public T[] SortedHeapList()
        {
            int tempPosition = lastPosition;
            T[] tempArray=new T[tempPosition];
            for (int i = 0; i < tempPosition; i++)
            {
                tempArray[i] = Remove();  //Tasarlanan heap MAXHEAP oldugundan, ilk deger her zaman en buyuktur.
                if (i>=1&& Comparison(tempArray[i],tempArray[i-1])>0)
                {
                     Swap(i,i-1);
                }
            }

            return tempArray;
        }  // nlogn Complexity (log 2 tabanında)

    }
}
