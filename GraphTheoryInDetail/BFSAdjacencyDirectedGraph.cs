using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //the class uses adjacency list to store edges and vertices
    //this is a sample of directed graph
    class BFSAdjacencyDirectedGraph
    {
        //an adjacency list, which is a dictionary basically
        //key is the vertice and List<Edges> is a list store connections from key to 
        //[key:1,value:[new Edges(toEdge,cost),new Edges(toEdge,cost) ... ]] and such
        private Dictionary<int, List<Edges>> myGraphDictionary;
        public BFSAdjacencyDirectedGraph()
        {
            myGraphDictionary = new Dictionary<int, List<Edges>>();
        }

        //my edges
        class Edges
        {
            public bool visited { get; set; }
            public int toEdge { get; private set; }
            public double cost { get; private set; }

            public Edges(int toEdge, double cost)
            {
                this.visited = false;
                this.toEdge = toEdge;
                this.cost = cost;
            }
        }
        public void CreateDirectedEdges(int fromEdge, int toEdge, double cost)
        {
            List<Edges> edges;
            //if myGraphDictionary contains a key of fromEdge then 
            //keep adding previously created list 
            //myGraphDictionary[fromEdge].Add(new Edges(toEdge,cost));
            if (!myGraphDictionary.ContainsKey(fromEdge))
            {
                edges = new List<Edges>();
                edges.Add(new Edges(toEdge, cost));
                myGraphDictionary.Add(fromEdge, edges);
            }
            else
            {
                myGraphDictionary[fromEdge].Add(new Edges(toEdge, cost));
            }
        }
        public List<int> BreadthFirstSearchResult(int startingIndex)
        {
            return BFS(myGraphDictionary, startingIndex);
        }
        private List<int> BFS(Dictionary<int, List<Edges>> myGraph, int startNode)
        {
            if (myGraph == null)
                throw new ArgumentNullException();

            List<int> resList = new List<int>(); // a list to store BFS traverse
            Queue<int> queue = new Queue<int>();

            queue.Enqueue(startNode);
            resList.Add(startNode);


            while (queue.Count != 0)
            {
                int nextNode = queue.Dequeue();

                try
                {
                    //if myGraph[nextNode] doesn't exist , keep while loop by ignoring the case
                    foreach (var edge in myGraph[nextNode])
                    {
                        edge.visited = true;
                        queue.Enqueue(edge.toEdge);
                        //if item is already in list , don't add it
                        if (!resList.Contains(edge.toEdge))
                            resList.Add(edge.toEdge);
                    }

                }
                catch
                {
                    // ignored
                }
            }

            return resList;
        }
    }
}
