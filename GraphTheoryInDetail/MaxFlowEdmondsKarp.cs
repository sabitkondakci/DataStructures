using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //an optimized approximation to max flow network problem
    class MaxFlowEdmondsKarp
    {
        protected internal class Edges
        {
            public int fromVer;
            public int toVer;
            public long capacity;

            public Edges(int fromVer, int toVer, long capacity)
            {
                this.fromVer = fromVer;
                this.toVer = toVer;
                this.capacity = capacity;
            }
        }

        private List<Edges>[] graph;
        public List<Edges>[] GetGraph
        {
            get { return graph; }
        }
        public MaxFlowEdmondsKarp(int numberOfVertices)
        {
            this.graph=new List<Edges>[numberOfVertices];

            for (int i = 0; i < numberOfVertices; i++)
            {
                graph[i]=new List<Edges>();
            }
        }
        //method returns maximum flow
        public void CreateDirectedEdges(int fromVer, int toVer, int capacity)
        {
            //add values in sorted order, in terms of capacity value
            graph[fromVer].Add(new Edges(fromVer,toVer,capacity));
            graph[fromVer].Sort(new EdgeComparer());

            graph[toVer].Add(new Edges(toVer,fromVer,0));
            graph[toVer].Sort(new EdgeComparer());
        }
        public long MaxFlowSolver(List<Edges>[] graph,int s,int t)
        {
            if (graph.Length == 0) //check if graph is empty or not!
                return default;

            long maxFlow = 0; // maxFlow to be returned
            int N = graph.Length;
            Queue<int> queue=new Queue<int>();

            for (int i = 0; i < N; i++)
            {
                bool haltChecker = true; // check if there's an edge to be visited which is capacity>0
                foreach (var check in graph[s])
                {
                    if (check.capacity > 0)
                        haltChecker = false;
                }

                if (haltChecker) break;//if there's no value bigger than 0 , then break

                bool[] visited = new bool[N];//this will be refreshed for each loop
                int[] parent = new int[N];//this will be refreshed for each loop

                queue.Enqueue(s);
                visited[s] = true;

                while (queue.Count != 0)// a loop to fill queue and parent
                {
                    var edge = queue.Dequeue();
                    
                    foreach (var edgeObject in graph[edge])
                    {
                        if (!visited[edgeObject.toVer] && edgeObject.capacity>0)
                        {
                            visited[edgeObject.toVer] = true;
                            parent[edgeObject.toVer] = edge;
                            queue.Enqueue(edgeObject.toVer);
                        }
                    }
                }

                long resFlow = long.MaxValue;

                int at = t;
                while (at!=s)
                {
                    //method throws an exception at last loop
                    try
                    {
                        long cap1 = graph[parent[at]].First(x => x.toVer == at).capacity;
                        at = parent[at];
                        if (cap1 < resFlow)
                            resFlow = cap1;
                    }
                    catch
                    {
                        return maxFlow;
                    }
                    
                }

                maxFlow += resFlow;

                at = t;
                while (at != s)
                {
                    var capRes= graph[at].First(x => x.toVer == parent[at]);
                    var cap1 = graph[parent[at]].First(x => x.toVer == at);
                    
                    //reconstruct the graph in capacity, following parent path
                    cap1.capacity -=resFlow;
                    capRes.capacity += resFlow;
                    at = parent[at];
                }
            }

            return maxFlow;
        }
        class EdgeComparer:IComparer<Edges> //List.Sort() takes IComparer
        {
            public int Compare(Edges x, Edges y)
            {
                return x.capacity.CompareTo(y.capacity);
            }
        }
    }

}
