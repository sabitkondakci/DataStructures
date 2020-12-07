using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace FenwickTreeImplementation
{
    class FenwickTree<T> where T:struct
    {
        private T[] fenwickTree { get; set; }
        public FenwickTree(T[] arraySample)
        {
            fenwickTree=new T[arraySample.Length+1];
            arraySample.CopyTo(fenwickTree,1);
            CreateFenwickTree(fenwickTree);
        }
        
        //this method stands for Least Significant Bit
        //i & -i is another choice of bitwise operation
        private int LSB(int i)=> i & ~(i - 1);
        private void CreateFenwickTree(T[] fenwickTree)
        {
            int N = fenwickTree.Length;
            for (int i = 0; i < N; i++)
            {
                int j = i + LSB(i);
                if (j < N)
                    fenwickTree[j] = (dynamic)fenwickTree[j] + fenwickTree[i];
            }
        }
        // Sum between range [1,index] , both inclusive
        //index starts from 1!
        public T PrefixSum(int index)
        {
            if(index>=fenwickTree.Length)
                throw new IndexOutOfRangeException();

            T resultSum = default;
            while (index!=0)
            {
                resultSum= (dynamic)resultSum + fenwickTree[index];
                index -= LSB(index);
            }

            return resultSum;
        }
        //Sum between range [i,j] , both inclusive
        //index starts from 1!
        public T RangeSum(int i, int j) => (dynamic)PrefixSum(j) - PrefixSum(i - 1);

        //if you use a dynamic list you shoud update the values on fenwickTree[]
        private void AddToList(int i, T x)
        {
            while (i<fenwickTree.Length)
            {
                fenwickTree[i] = (dynamic)fenwickTree[i] + x;
                i = i + LSB(i);
            }
        }

    }
}
