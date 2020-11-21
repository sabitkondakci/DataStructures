using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArrays
{
    class Array<T> : IEnumerable<T>
    {
        private T[] array;
        private int length = 0; // Ilk Belirlenen Array Boyutu
        private int capacity = 0; // Array Kapasitesi
        public Array(int capacity = 8)
        {
            if (capacity < 0)
                throw new Exception("Hatali Bir Kapasite Girildi! " + capacity + "Kapasite >= 0 Olmali!)");
            this.capacity = capacity;
            array = new T[this.capacity];
        }
        public int Size() => length; 
        public int Capacity() => capacity;
        public bool IsEmpty() => Size() == 0;  
        public T Get(int index) => array[index];
        public void Set(int index, T element) => array[index] = element;
        public T this[int index]  //(Get() Alternatifi) Dizin olusturucu kullanilarak istenilen deger okunabilir veya yazilabilir
        {
            get => array[index];
        }
        public void Clear()  //Array'de kayitli butun veriler silinir
        {
            for (int i = 0; i < capacity; i++)  
            {
                array[i] = default;   //Genel tipe ozgu default deger silinen verilerin yerine konur, default(int)=0 ve default(string) = null
            }
            length = 0;
            capacity = 8;
        }
        public void Add(T element)  //Array'e girilen degerler ilk tanimlanan kapasiteyi asarsa; kapasite 8,16,32,64,128 ... seklinde artar
        {
            if (length+1>capacity)      
            {
                capacity =(length-length%8)*2 ;
                capacity = capacity == 0 ? 8 : capacity;

                T[] _array = new T[capacity];
                for (int i = 0; i < length; i++)
                {
                    _array[i] = array[i];   //Yeni kopya Array olusturulur
                }

                array = _array;  // Ilk olusturulan Array'e aktarilir
            }

            array[length++] = element;
        }

        public T RemoveAt(int index) //Herhangi bir pozisyondaki veriyi Array den kaldirir
        {
            if (index >= length || index<0)
            {
                throw new IndexOutOfRangeException("Index "+length+" den kucuk olmali!");
            }

            capacity = (length - length % 8) * 2;    // Kaldirilan veriden sonra kapasite kucultulur.
            capacity = capacity == 0 ? 8 : capacity;

            T value = array[index];
            T[] _array = new T[length-1];
            for (int i = 0, j = 0; i < length; i++,j++)
            {
                if (i==index)
                {
                    j--;
                }
                else
                {
                    _array[j] = array[i];
                }
            }

            array = _array;
            length--;

            return value;
        }

        public bool Remove(T value) 
        {
            for (int i = 0; i < length; i++)
            {
                if (array[i].Equals(value))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;

        }

        public int IndexOf(T value)
        {
            for (int i = 0; i < length; i++)
            {
                if (array[i].Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T value)
        {
            return IndexOf(value) !=-1;
        }                   
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var var in array)
            {
                yield return var;
            }

        }   // Foreach dongusunde Array'i kullanabilmemiz icin IEnumerable interfacini uygulamamiz gerekir.
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
