using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyHashTable
{
    public class ChainedHash<k, v>  
    {
        private int numElements, tableSize;
        private double maxLoadFactor;  //Tablo boyutunun ne zaman degisecegini gosterir
        private LinkedList<HashSetPair<k, v>>[] harray; 
        public ChainedHash(int tableSize)
        {
            this.tableSize = tableSize;
            harray= new LinkedList<HashSetPair<k, v>>[tableSize];
            for (int i = 0; i < tableSize; i++)     
            {
                harray[i]=new LinkedList<HashSetPair<k, v>>(); // Bos LinkedList ler olusturulur
            }

            maxLoadFactor = 0.75;   //numElements/Capacity of Hash Array ,Resize Factor
            
            numElements = 0;
        }
        private class HashSetPair<k, v>   //LinkedList'in aldigi veri tipi, liste HashSetPair nesnelerini alir.
        {
            public k key;
            public v value;

            public HashSetPair(k key,v value)
            {
                this.key = key;
                this.value = value;
            }
            public int CompareTo(HashSetPair<k, v> other, k key) //Referans karsilastirmasi yapmamak adina ozel bir karsilastirma fonksiyonu olusturulur.
            {                                                    //Comparer.Default.Compare kullanilarak degerler karsilastirilir.
                if (other==null)
                {
                    return 1;
                }

                return Comparer.Default.Compare((k)key, (k)other.key); // Karsilastirilmak istenen veri turu 'k' oldugu icin cast yapilir.
            }

        }
        public double LoadFactor() => numElements / tableSize;
        public int Size() => tableSize;
        public int NumberOfElements() => numElements;
        public void Resize(int newSize)  //Buyuyen dizinin yeniden boyutlanmasini saglar.
        {
            LinkedList<HashSetPair<k,v>>[] newSetPair= new LinkedList<HashSetPair<k, v>>[newSize];
            for (int i = 0; i < newSize; i++)    //Yeni bir dizi olusturulur, ici bos LinkedList nesneleri olusturulur.
            {
                newSetPair[i] = new LinkedList<HashSetPair<k, v>>();
            }

            foreach (var iterator in this.harray) //Eski listedeki degerler yeni listeye aktarilir.
            {
                foreach (var setPair in iterator)
                {
                    HashSetPair<k,v> newHashList= new HashSetPair<k, v>(setPair.key,setPair.value);
                    int hashValue = setPair.key.GetHashCode();
                    hashValue = hashValue & 0x7fffffff;
                    hashValue = hashValue % newSize;

                    newSetPair[hashValue].AddFirst(newHashList);
                }
            }

            harray = newSetPair; //Yeni dizinin referansi eskisine aktarilir.
            tableSize = newSize;
        }
        public bool Add(k key,v value)  //Listeye HasSetPair nesnelerini ekler.
        {
            if (LoadFactor()>maxLoadFactor)
            {
                Resize(tableSize*2);
            }          
            
            HashSetPair<k,v> hashSetPair= new HashSetPair<k, v>(key,value);
            int hashValue = key.GetHashCode();
            hashValue = hashValue & 0x7fffffff;
            hashValue = hashValue % tableSize;

            harray[hashValue].AddFirst(hashSetPair);
            numElements++;
            return true;
        }
        public bool Remove(k key) //Tek seferde bir key e sahip nesneyi siler.
        {

            int hashValue = key.GetHashCode();
            hashValue = hashValue & 0x7fffffff;
            hashValue = hashValue % tableSize;

            try                                  //Aranilan key in listedeki durumunu kontrol eder.
            {
                harray[hashValue].RemoveFirst();
                numElements--;
                return true;
            }
            catch 
            {
                return false;
            }
            
                              
            
        }
        public void RemoveAll(k key)
        {
            while (Remove(key))
            {
               // this removes all related keys' values 
            }
        }  //Girilen key e ait butun nesneleri siler.
        public v GetValue(k key)   //Belirli bir key e karsilik gelen degeri dondurur.
        {
            int hashValue = key.GetHashCode();
            hashValue = hashValue & 0x7fffffff;
            hashValue = hashValue % tableSize;

            foreach (var pair in harray[hashValue]) //Tek bir kontrol yapilir ve listedeki ilk key degerine karsilik gelen value dondurulur.
            {
                if (pair.CompareTo(pair,key)==0)
                {
                    return pair.value;
                }
            }

            return default;
        }
        public List<v> GetAllValues(k key)
        {
            int hashValue = key.GetHashCode();
            hashValue = hashValue & 0x7fffffff;
            hashValue = hashValue % tableSize;
            
            List<v> allValues = new List<v>();
            foreach (var pair in harray[hashValue])
            {
                if (pair.CompareTo(pair, key) == 0)
                {
                    allValues.Add(pair.value);
                }
            }

            return allValues;
        } //Tek bir key e ait butun degerleri List<v> olarak dondurur.
    }

    public static class ExtensionMethodsDesignedByMe
    {
        public static long HashCodePhoneNumber(this string data)
        {
            int g = 7;
            long hash = 0;
            string dataCut = data.Substring(1,10);
            for (int i = 0; i < dataCut.Length; i++)
            {
                int num = dataCut[i];
                hash += num * (long)Math.Pow(g, i);
            }

            return hash;
        }
    } //Extended Methods , bazi genisletilmis methodlari icerir
}
