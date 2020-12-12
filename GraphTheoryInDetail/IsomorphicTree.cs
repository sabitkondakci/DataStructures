using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //before diving into this subject please check the subjects of 
    //Tarjan' Algorithms ,
    //spanning from Tarjan's Strongly Connected Components to Articulation Points-Finding Bridges
    class IsomorphicTree
    {
        private int amounOfVertices;
        // Adjacency List 
        private List<KeyValuePair<int,long>>[] adjacencyList;
        private int time = 0;
        private const int empty = -1;
        public IsomorphicTree(int amountOfVertices)
        {
            this.amounOfVertices = amountOfVertices;
            adjacencyList = new List<KeyValuePair<int,long>>[amountOfVertices];
            //initialize and create list at every indeks of adjacencyList[i]
            for (int i = 0; i < amountOfVertices; ++i)
                adjacencyList[i] = new List<KeyValuePair<int,long>>();
        }

        //adding edges to the graph, this's an undirected graph
        public void CreateEdge(int fromEdge, int toEdge,long weight)
        {
            adjacencyList[fromEdge].Add(new KeyValuePair<int, long>(toEdge,weight)); 
            adjacencyList[toEdge].Add(new KeyValuePair<int, long>(fromEdge,weight)); 
        }

        // A recursive function that find articulation points using DFS 
        // nextVertex --> The vertex to be visited next 
        // visited[] --> keeps tract of visited vertices 
        // discTime[] --> Stores discovery times of visited vertices 
        // parent[] --> Stores parent vertices in DFS tree 
        // articulationPoint[] --> Store articulation points 

        //this is known as "Tarjan's Algorithm" for Articulation Points
        private void APUtility(int nextVertex, bool[] visited, int[] discTime,
                    int[] lowTime, int[] parent, bool[] articulationPoint)
        {
            // Count of children in DFS Tree 
            int children = 0;

            // Mark the current node as visited 
            visited[nextVertex] = true;

            // Initialize discovery time and low value 
            discTime[nextVertex] = lowTime[nextVertex] = ++time;

            // go through all vertices adjacent to this list
            foreach (var i in adjacencyList[nextVertex])
            {
                int v = i.Key; // v is current adjacent of nextVertex 

                // If v is not visited yet, then make it a child of nextVertex 
                // in DFS tree and recur for it 
                if (!visited[v])
                {
                    children++;
                    parent[v] = nextVertex;
                    APUtility(v, visited, discTime, lowTime, parent, articulationPoint);

                    // Check if the subtree rooted with v has  
                    // a connection to one of the ancestors of nextVertex 
                    lowTime[nextVertex] = Math.Min(lowTime[nextVertex], lowTime[v]);

                    // nextVertex is an articulation point in following cases 

                    // (1) nextVertex is root of DFS tree and has two or more chilren.
                    if (parent[nextVertex] == empty && children > 1)
                        articulationPoint[nextVertex] = true;

                    // (2) If nextVertex is not root and low value of one of its child 
                    // is more than discovery value of nextVertex. 
                    if (parent[nextVertex] != empty && lowTime[v] >= discTime[nextVertex])
                        articulationPoint[nextVertex] = true;
                } 
                // Update low value of nextVertex for parent function calls. 
                else if (v != parent[nextVertex])
                    lowTime[nextVertex] = Math.Min(lowTime[nextVertex], discTime[v]);
            }
        }

        // The function to do DFS traversal.  
        // It uses recursive function APUtility() 
        public List<int> ArticulationPoints()
        {
            // Mark all the vertices as not visited 
            bool[] visited = new bool[amounOfVertices];
            int[] discTime = new int[amounOfVertices];
            int[] lowTime = new int[amounOfVertices];
            int[] parent = new int[amounOfVertices];
            bool[] articulationPoint = new bool[amounOfVertices]; // To store articulation points 
            List<int> articulationList=new List<int>();
            // Initialize parent and visited, and  
            for (int i = 0; i < amounOfVertices; i++)
            {
                parent[i] = empty;
            }

            // Call the recursive helper function to find articulation 
            // points in DFS tree rooted with vertex 'i' 
            for (int i = 0; i < amounOfVertices; i++)
                if (visited[i] == false)
                    APUtility(i, visited, discTime, lowTime, parent, articulationPoint);

            // Now articulationPoint[] contains articulation points, print them 
            for (int i = 0; i < amounOfVertices; i++)
                if (articulationPoint[i] == true)
                    articulationList.Add(i);

            return articulationList;
        }

        public List<int> AllTreeCenters()
        {
            return AllTreeCentersPrivate(adjacencyList);
        }

        private List<int> AllTreeCentersPrivate(List<KeyValuePair<int,long>>[] myGraphList)
        {
            int length = myGraphList.Length;
            //using elimination method, in order to remove all leaf nodes
            int[] degree = new int[length];
            //leaves will be stored in this list
            List<int> leaves=new List<int>(length);

            for (int i = 0; i < length; i++)
            {

                degree[i] = myGraphList[i].Count;//amount of edges for a single vertice

                if (degree[i] <=1)
                {
                    leaves.Add(i);
                    degree[i] = 0;
                }

            }

            int count = leaves.Count;

            while (count<length)
            {
                List<int> tempNewLeaves = new List<int>();

                foreach (var leaf in leaves)
                {
                    foreach (var keyValuePair in myGraphList[leaf])
                    {
                        // as a node is removes, degree is reduces , which is
                        // amount of connections mainly
                        degree[keyValuePair.Key] = degree[keyValuePair.Key] - 1;
                        //another leaf not occurs, at second loop we'll eliminate them
                        if(degree[keyValuePair.Key]==1)
                            tempNewLeaves.Add(keyValuePair.Key);
                    }

                }

                // add this amount to count till it reaches to amountOfVertices
                count = count + tempNewLeaves.Count;
                leaves = tempNewLeaves;//the last nodes wiht 1 connection or 0 connection
            }

            return leaves;
        }

    }
}
