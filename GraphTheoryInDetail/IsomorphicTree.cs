using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            {
                adjacencyList[i] = new List<KeyValuePair<int, long>>();
            }
                
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
            // all bool values are false by default
            bool[] visited = new bool[amounOfVertices];
            int[] discTime = new int[amounOfVertices];
            int[] lowTime = new int[amounOfVertices];
            int[] parent = new int[amounOfVertices];
            bool[] articulationPoint = new bool[amounOfVertices]; // To store articulation points 
            List<int> articulationList=new List<int>();
            // Initialize parent
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
        public List<KeyValuePair<int, long>>[] GetGraphList() => adjacencyList;
        
        public List<int> AllTreeCenters()
        {
            return AllTreeCentersPrivate(adjacencyList);
        }

        public bool AreTreesIsomorphic(List<KeyValuePair<int, long>>[] tree1
            , List<KeyValuePair<int, long>>[] tree2)
        {
            if (EncodeTheTree(tree1) == EncodeTheTree(tree2))
                return true;

            return false;
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

        public List<KeyValuePair<int,long>>[] DirectedRootedTreeGenerator(List<KeyValuePair<int,long>>[] tree)
        {

            List<KeyValuePair<int,long>>[] rootedTreeAdjList = 
                new List<KeyValuePair<int, long>>[amounOfVertices];

            for (int i = 0; i < amounOfVertices; i++)
            {
                rootedTreeAdjList[i]=new List<KeyValuePair<int, long>>();
            }

            bool[] visited=new bool[amounOfVertices];
            int root =AllTreeCenters()[0] ;

            Stack<int> stack=new Stack<int>();
            stack.Push(root);

            while (stack.Count!=0)
            {
                int vertice = stack.Pop();

                if (!visited[vertice])
                {
                    visited[vertice] = true;
                    var toEdges = tree[vertice];

                    if (toEdges.Count!=0)
                    {
                        foreach (var edge in toEdges)
                        {
                            if (!visited[edge.Key])
                            {
                                stack.Push(edge.Key);
                                rootedTreeAdjList[vertice].Add(new KeyValuePair<int, long>(edge.Key,edge.Value));
                            }
                        }
                    }
                }
            }

            return rootedTreeAdjList;
        }

        public List<int> AllLeaves(List<KeyValuePair<int, long>>[] tree)
        {
            List<int> leaves=new List<int>();
            var tempList = DirectedRootedTreeGenerator(tree);
            for (int i = 0; i < amounOfVertices; i++)
            {
                if(tempList[i].Count==0)
                    leaves.Add(i);
            }

            return leaves;
        }

        private string EncodeTheTree(List<KeyValuePair<int, long>>[] tree)
        {
            int[] parent = new int[amounOfVertices];//to store parent nodes
            var rootedList = DirectedRootedTreeGenerator(tree);// a root tree list
            List<int> leafNodes = AllLeaves(tree);//leaves
            string[] myStringArray=new string[amounOfVertices];//a string array to store strings at indexes
            
            List<string>[] preliminaryResult=new List<string>[amounOfVertices];
            List<string> printOut=new List<string>();
                   
            string result = "";

            //initiate leaf nodes
            foreach (var leafNode in leafNodes)
                myStringArray[leafNode] = "()";
            
            //parent values by default -1
            //initiate the lists in array
            for (int i = 0; i < amounOfVertices; i++)
            {
                preliminaryResult[i]=new List<string>();
                parent[i] = -1;
            }
            
            //insert parents on rooted tree
            for (int i = 0; i < amounOfVertices; i++)
            {
                foreach (var keyPair in rootedList[i])
                {
                    parent[keyPair.Key] = i;
                }
            }

            foreach (var leafNode in leafNodes)
            {
                int k = leafNode;
                while (parent[k]!=-1)
                {
                    //traverse through up the tree
                    //from parent to parent , till
                    result = myStringArray[k];
                    k = parent[k];
                    
                    if (result != null)
                    {
                        result = "";
                        foreach (var childrenSum in rootedList[k])
                        {
                            result += myStringArray[childrenSum.Key];
                        }

                        result = "(" + result + ")";
                        preliminaryResult[k].Add(result);
                    }                  
                }
            }

            //sort the preliminaryList which stores strings that will be used for encoding
            var outcome=preliminaryResult.Where(x=>x.Count>0).OrderByDescending(x => x.Count).ToList();
            result = "";

            foreach (var res in outcome)
            {
                foreach (var x in res)
                {
                    result += x;
                }
            }


            return "(" + result + ")";
        }
    }
}
