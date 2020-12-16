using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //before proceeding through the topic, check your graph if it's acyclic
    //Kahn's algorithm solves cycle hassle too
    class TopologicalSort
    {
        private List<KeyValuePair<int,long>>[] myGraph;
        private List<int>[] children;
        public List<KeyValuePair<int,long>>[]  GetGraph
        {
            get { return myGraph; }
        }
        public TopologicalSort(int numOfVertices)
        {
            myGraph=new List<KeyValuePair<int, long>>[numOfVertices];
            children=new List<int>[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                myGraph[i]=new List<KeyValuePair<int, long>>();
                children[i]=new List<int>();
            }
        }

        public void CreateDirectedEdges(int fromVer,int toVer,long weight)
        {
            myGraph[fromVer].Add(new KeyValuePair<int, long>(toVer,weight));
            children[toVer].Add(fromVer);
        }

        public int[] TopologialSort(List<KeyValuePair<int, long>>[] graph)
        {
            //check if your directed graph acyclic
            if(IsCyclic(graph))
                throw new Exception("Graph has to be directed acyclic!");

            int numberOfVertices = graph.Length;
            bool[] visited=new bool[numberOfVertices];

            //one of topological orderings
            int[] order=new int[numberOfVertices];
            int i = numberOfVertices - 1;

            //visit all vertices
            for (int at = 0; at < numberOfVertices; at++)
            {
                if (visited[at] == false)
                    DFS(i,at,visited,order,graph);
            }

            return order;
        }
        
        private int DFS(int i,int at, bool[] visited, int[] order, List<KeyValuePair<int, long>>[] myGraphInDfs)
        {
            visited[at] = true;
            var edges = myGraphInDfs[at];//check connections in recursion

            foreach (var keyValuePair in edges)
            {
                //if vertice hasn't been visited yet, recurse again
                if (visited[keyValuePair.Key] == false)
                    i = DFS(i, keyValuePair.Key, visited, order, myGraphInDfs);
            }

            order[i] = at;
            return i - 1;
        }

        #region CycleDetection //https://www.geeksforgeeks.org

        private bool IsCyclicUtil(int i, bool[] visited,
            bool[] recStack,List<KeyValuePair<int,long>>[] graphAdjList)
        {

            // Mark the current node as visited and  
            // part of recursion stack  
            if (recStack[i])
                return true;

            if (visited[i])
                return false;

            visited[i] = true;

            recStack[i] = true;
            List<KeyValuePair<int,long>> children = graphAdjList[i];

            foreach (var c in children)
                if (IsCyclicUtil(c.Key, visited, recStack,graphAdjList))
                    return true;

            recStack[i] = false;

            return false;
        }

        // Returns true if the graph contains a  
        // cycle, else false.  
        private bool IsCyclic(List<KeyValuePair<int,long>>[] graphAdjList)
        {
            int amountOfVertices = graphAdjList.Length;
            // Mark all the vertices as not visited and  
            // not part of recursion stack  
            bool[] visited = new bool[amountOfVertices];
            bool[] recStack = new bool[amountOfVertices];


            // Call the recursive helper function to  
            // detect cycle in different DFS trees  
            for (int i = 0; i < amountOfVertices; i++)
                if (IsCyclicUtil(i, visited, recStack,graphAdjList))
                    return true;

            return false;
        }

        #endregion
    }
}
