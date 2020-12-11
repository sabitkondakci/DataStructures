using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{ 
    //the class uses adjacency list to store edges and vertices
    //this is a sample of directed graph
    class DFSAdjacencyDirectedGraph
    {
        private List<int> leafNodeList;
        //an adjacency list, which is a dictionary basically
        //key is the edge and List<Edges> is a list store connections from key
        //[key:1,value:[new Edges(toEdge,cost),new Edges(toEdge,cost) ... ]] and such
        private Dictionary<int, List<Edges>> myGraphDictionary;
        public DFSAdjacencyDirectedGraph()
        {
            myGraphDictionary=new Dictionary<int, List<Edges>>();
            leafNodeList=new List<int>();
        }
        //my connectors , edges
        class Edges
        {
            public bool visited { get; set; } //if it visited or not, false by default
            public int toEdge { get; private set; } //edge that I desire to connect
            public double cost { get; private set; }

            public Edges(int toEdge, double cost)
            {
                this.visited = false;
                this.toEdge = toEdge;
                this.cost = cost;
            }
        }
        public void CreateDirectedEdges(int fromEdge,int toEdge, double cost)
        {
            List<Edges> edges;
            //if myGraphDictionary contains a key of fromEdge then 
            //keep adding previously created list 
            //myGraphDictionary[fromEdge].Add(new Edges(toEdge,cost));
            if (!myGraphDictionary.ContainsKey(fromEdge))
            {
                edges = new List<Edges>();
                edges.Add(new Edges(toEdge,cost));
                myGraphDictionary.Add(fromEdge,edges);
            }
            else
            {
                myGraphDictionary[fromEdge].Add(new Edges(toEdge,cost));
            }
            
        }

        public int TreeCenter()
        {
            return TreeCenterPrivate(myGraphDictionary);
        }
        private int TreeCenterPrivate(Dictionary<int, List<Edges>> myGraph)
        {
            return myGraph.OrderByDescending(x=>x.Value.Count).First().Key;
        }

        //if graph structure contains no cycle ,
        //then it is supposed to be a tree
        //use this method only if your graph contains no cycle
        public List<int> LeafNodeListForTreeStructure(int startNode)
        {
            DFS(myGraphDictionary, startNode);
            return leafNodeList;
        }
        public List<int> DepthFirstSearchResult(int startingIndex)
        {
            return DFS(myGraphDictionary,startingIndex);
        }

        private List<int> DFS(Dictionary<int, List<Edges>> myGraph, int startNode)
        {
            if(myGraph==null)
                throw new ArgumentNullException();

            List<int> resList = new List<int>(); // a list to store DFS traverse
            Stack<int> stack = new Stack<int>();

            stack.Push(startNode);
            resList.Add(startNode);

            while (stack.Count != 0)
            {
                Edges nextNode;
                try
                {
                    // take the first false case by order
                    //if there is no then nextNode=null in catch block
                     nextNode = myGraph[stack.Peek()].First(x => x.visited == false);
                }
                catch (Exception exception) 
                    when (exception is KeyNotFoundException || exception is InvalidOperationException)
                {
                    //if exception is a KeyNotFoundException ,
                    //it's a very sign of a leaf node of a tree structure
                    if(exception is KeyNotFoundException)
                        leafNodeList.Add(stack.Peek());

                     nextNode = null;
                }

                //if nextNode == null it means that there's nowhere to move
                //so that Pop()
                //if resList.Contains(nextNode.toEdge)'s true ,
                //it is already in the list so that invoke Pop()
                if (nextNode != null && !resList.Contains(nextNode.toEdge))
                {
                    nextNode.visited = true;
                    stack.Push(nextNode.toEdge);
                    resList.Add(nextNode.toEdge);
                }
                else
                    stack.Pop();
            }

            return resList;
        }

    }
}
