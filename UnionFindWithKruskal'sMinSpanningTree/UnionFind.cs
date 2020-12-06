using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnionFindOperation
{
    class UnionFind
    {
        //I used a 2D array to store edges and their weights
        //Each element of array takes KeyValuePair object; key: edge identity , value:weight
        private KeyValuePair<int, double>[,] edges;

        //   0 ,  1,  2, 3,  4, 5
        //  [h1, h1, h1, 0, h3, h3 ]  =>unionArray h1,h2,h3 stored hashCodes which gives the edges unique identities
        // [0,1],[1,2] is connected to the same piece, [4,5] is a different part
        private int[] unionArray;
        public UnionFind(long amountOfVertices)
        {
            edges=new KeyValuePair<int, double>[amountOfVertices,amountOfVertices];
            unionArray=new int[amountOfVertices];
        }
        //Beforehand of Unify method , ConnectVertices should be used to fill the edges array, table
        public bool ConnectVertices(int a, int b, double weight)
        {
            //if there's no connection create one!
            if (edges[a, b].Key == 0 && edges[a, b].Value == 0)
            {
                var guid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                int hashCode = guid.GetHashCode();

                edges[a, b] = new KeyValuePair<int, double>(hashCode, weight);
                edges[b, a] = new KeyValuePair<int, double>(hashCode, weight);
                return true;
            }

            return false;
        }
        //the method creates sub groups of connections , if you keep adding it will end up one whole piece 
        // a and b vertices, desired to be joined
        //this method was designed for Kruskal's Algorithm so that cycles aren't allowed!
        public bool Unify(int a, int b)
        {
            //first , check if a connection exists between to vertices
            //KeyValuePair has a keyValue pair of [0,0] by default
            if (edges[a, b].Key != 0 && edges[a, b].Value != 0)
            {
                //unionArray has default values of 0
                //check if a value appended to it
                if (unionArray[a]==0 && unionArray[b]==0)
                {
                    //keys are unique edge identifiers so that it's safely used
                    unionArray[a] = edges[a, b].Key;
                    unionArray[b] = edges[a, b].Key;
                }
                else if (Find(a,b)) //check if a cycle exists
                {
                    return false;
                }
                else if (unionArray[a] != 0 && unionArray[b] == 0)
                {
                    unionArray[b] = unionArray[a];
                }
                else if (unionArray[a] == 0 && unionArray[b] != 0)
                {
                    unionArray[a] = unionArray[b];
                }
                else 
                {
                    //if a subgroup is attached to another one then alter other subgroups identities
                    //which mainly means creating a bigger subgroup 
                    int tempUnionValue = unionArray[a];

                    for (int i = 0; i < unionArray.Length; i++)
                    {
                        if (tempUnionValue == unionArray[i])
                            unionArray[i] = unionArray[b];
                    }
                }

                return true;
            }

            return false;
        }
        //Find method checks whether an endge belongs to a subgroup or not
        //this also prevents cycles

        //method solves kruska's minimum spanning tree problem 
        //it returns KeyValuePair which stands for nodes
        public List<KeyValuePair<int,int>> KruskaMinSpanningTree()
        {
            //a list to store key and values
            //key:edge[i,j].Value , list sorts on key!
            SortedList<double, KeyValuePair<int, int>> kruskaSet 
                = new SortedList<double, KeyValuePair<int, int>>(new DuplicateKeyHandler<double>());

            List<KeyValuePair<int, int>> resultOfKruska=
                new List<KeyValuePair<int, int>>();

            //adding values to the kruskaSet
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                for (int j = i + 1; j < edges.GetLength(1); j++)
                {
                    kruskaSet.Add(edges[i,j].Value,new KeyValuePair<int, int>(i,j));
                }
            }

            foreach (var element in kruskaSet.Values)
            {
                //if nodes can be joined , accept the pair as a part of minimum spanning tree
               if(Unify(element.Key, element.Value))
                    resultOfKruska.Add(new KeyValuePair<int, int>(element.Key,element.Value));
            }

            return resultOfKruska;
        }
        public bool Find(int a, int b) => unionArray[a] == unionArray[b];
        

    }

    //a unique solution for dublicate key entries
    public class DuplicateKeyHandler<K> : IComparer<K> where K : IComparable
    {
        public int Compare(K x, K y)
        {
            if (x.CompareTo(y)==0)
                return 1;  
            else
                return x.CompareTo(y);
        }
    }
}
