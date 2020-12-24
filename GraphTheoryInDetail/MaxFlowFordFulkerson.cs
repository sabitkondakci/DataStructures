using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //a greedy approach, it finds non optimal path
    class MaxFlowFordFulkerson
    {
        protected internal class Edge
        {
            public int fromVer, toVer;
            public Edge residual;
            public long capacity, flow;

            public Edge(int fromVer,int toVer,long capacity)
            {
                this.fromVer = fromVer;
                this.toVer = toVer;
                this.capacity = capacity;
            }

            public bool IsResidual() => capacity == 0;
            public long RemainingCapacity() => capacity - flow;
            public void AugmentTheNodes(long bottleNeck)
            {
                flow += bottleNeck;
                residual.flow -= bottleNeck;
            }
        }

        private long INFINITY = long.MaxValue;
        private bool[] visited;
        private bool solved;
        private long maxFlow;
        private List<Edge>[] graph;
        private int n, s, t;// n: number of nodes , s : source , t : sink - target
        public MaxFlowFordFulkerson(int n, int s, int t)
        {
            this.n = n;
            this.s = s;
            this.t = t;
            this.visited=new bool[n];
            CreateEmptyFlowGraph();
        }
        private void CreateEmptyFlowGraph()
        {
            graph = new List<Edge>[n];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new List<Edge>();
            }
        }
        public void CreateDirectedEdges(int fromVer, int toVer, long capacity)
        {
            if(capacity<=0)
                throw new NoNullAllowedException("0 capacity isn't allowed");

            Edge edgeOne=new Edge(fromVer,toVer,capacity);//casual edge
            Edge edgeTwo=new Edge(toVer,fromVer,0);//residual edge

            edgeOne.residual = edgeTwo;
            edgeTwo.residual = edgeOne;

            graph[fromVer].Add(edgeOne);
            graph[toVer].Add(edgeTwo);
        }
        public List<Edge>[] GetGraph()
        {
            ExecutePlan();
            return graph;
        }
        public long GetMaxFlow()
        {
            ExecutePlan();
            return maxFlow;
        }
        private void ExecutePlan()
        {
            if(solved) return;
            solved = true;
            Solve();
        }
        private void Solve()
        {
            for (long f = DFS(s, INFINITY); f != 0; f = DFS(s, INFINITY))
                maxFlow += f;
        }
        private long DFS(int vertex, long flow)
        {
            if (vertex == t) return flow;

            visited[vertex] = true;

            foreach (var edge in graph[vertex])
            {
                if (!visited[edge.toVer]&&edge.RemainingCapacity() > 0)
                {
                    long bottleNeck =
                        DFS(edge.toVer, flow);

                    if (bottleNeck > 0)
                    {
                        edge.AugmentTheNodes(bottleNeck);
                        return bottleNeck;
                    }
                }
            }

            return default;
        }
    }
}
