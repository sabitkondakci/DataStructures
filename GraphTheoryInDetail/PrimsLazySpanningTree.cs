using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace GraphTheoryInDetail
{
    class PrimsLazySpanningTree
    {
        public class Edges
        {
            public readonly int fromVer;
            public readonly int toVer;
            public readonly long cost;

            public Edges(int fromVer,int toVer,long cost)
            {
                this.fromVer = fromVer;
                this.toVer = toVer;
                this.cost = cost;
            }
        }

        private  List<Edges>[] myGraph;
        public List<Edges>[] GetGraph
        {
            get { return myGraph; }
        }
        public PrimsLazySpanningTree(int numOfVertices)
        {
            this.myGraph = new List<Edges>[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                myGraph[i] = new List<Edges>();
            }
        }
        public void CreateUnDirectedEdges(int fromVer, int toVer, long weight)
        {
            myGraph[fromVer].Add(new Edges(fromVer,toVer,weight));
            myGraph[toVer].Add(new Edges(toVer,fromVer,weight));
        }
        //method returns minimum path and total cost of the same path (long)
        public long PrimsSpanningTree(List<Edges>[] graphAdjList
            ,out List<KeyValuePair<int,int>> pathList,int startNode = 0)
        {
            //path will be stored
            //KeyValuePair<fromVer,toVer> 
            List<KeyValuePair<int,int> > path=new List<KeyValuePair<int, int>>();

            int N = graphAdjList.Length;
            int edgeCount = 0;//in order to check edgeCount!=N-1
            long mstCost = 0;//minimun spanning tree total cost
            bool[] visited=new bool[N];

            //using Priority_Queue; you may add it to your project as a NuGet
            SimplePriorityQueue<KeyValuePair<int,int>,long> priorityQueue=
                new SimplePriorityQueue<KeyValuePair<int,int>,long>();

            foreach (var edges in graphAdjList[startNode])
            {
                priorityQueue.Enqueue(new KeyValuePair<int, int>(edges.fromVer,edges.toVer)
                    ,edges.cost );
            }

            visited[startNode] = true;

            //if priorityQueue isn't empty 
            while (priorityQueue.Count!=0 && edgeCount!= N-1)
            {
                var cost = priorityQueue.GetPriority(priorityQueue.First);
                var edge = priorityQueue.Dequeue();
                //edge.Value refers to toVer on graphAdjList
                if (visited[edge.Value]) continue;
                //adding path , edge.Key :fromVer , endge.Value:toVer
                path.Add(new KeyValuePair<int, int>(edge.Key,edge.Value));
                
                visited[edge.Value] = true;

                mstCost += cost;
                edgeCount++;

                //add the values to priority queue again
                foreach (var e in graphAdjList[edge.Value])
                {
                    priorityQueue.Enqueue(new KeyValuePair<int, int>(e.fromVer,e.toVer)
                        ,e.cost );
                }

            }

            pathList = path;//out List<KeyValuePair<int,int>> pathList

            if (edgeCount != N - 1)//if edgeCount is any different than N-1 , there must be a cycle!
                return default;

            return mstCost;
        }
    }
}
