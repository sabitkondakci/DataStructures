using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //Eulerian Circuit , Undirected Graph:
    // every vertex has an even degree , degree : edges 

    //Eulerian Circuit , Directed Graph:
    // every vertex has equal indegree and outdegree

    //Eulerian Path, Undirected Graph:
    // either every vertex has even degree or exactly two vertices have odd degree
    // and two odd degree is start-end vertex

    //Eulerian Path, Directed Graph:
    // at most one vertex has outdegree-indegree=1 , the node is a starting node
    // and at most one vertex has indegree-outdegree=1 , the node is an end node
    // all ther vertices have eqaul amount of indegrees and outdegrees
    class EulerianPathAlgorithm
    {
        private List<KeyValuePair<int, long>>[] myGraph;
        private int numOfNodes;
        private int edgeCount;//in order to check path.Count ~ edgeCount relation;
        private int[] indegree;//edges to a vertex directed into
        private int[] outdegree;//edges to a vertex directed outwards 
        private LinkedList<int> path;//a path to be stored
        public List<KeyValuePair<int, long>>[] GetGraph
        {
            get { return myGraph; }
        }
        public EulerianPathAlgorithm(int numOfVertices)
        {
            this.myGraph = new List<KeyValuePair<int, long>>[numOfVertices];
            this.indegree=new int[numOfVertices];
            this.outdegree=new int[numOfVertices];

            this.path=new LinkedList<int>();
            this.numOfNodes = numOfVertices;
            this.edgeCount = 0;
            for (int i = 0; i < numOfVertices; i++)
            {
                myGraph[i] = new List<KeyValuePair<int, long>>();
            }
        }
        public void CreateDirectedEdges(int fromVer, int toVer, long weight)
        {
            myGraph[fromVer].Add(new KeyValuePair<int, long>(toVer, weight));
        }
        public LinkedList<int> EulerianPath(List<KeyValuePair<int,long>>[] graphAdjList)
        {
            //if path doesn't exists at all then no Eulerian path/circuit will be found
            if (!PathExistsCheck())
                return null;

            for (int i = 0; i < numOfNodes; i++)
            {
                foreach (var edge in graphAdjList[i])
                {
                    indegree[edge.Key]++;
                    outdegree[i] = outdegree[i] + 1;
                    edgeCount++;//should be same with path.Count at the end!
                }
            }

            // a unique depth first search algorithm
            DFS(StartVertice());

            if (edgeCount == 0 || path.Count != edgeCount + 1)
                return null;

            return path;
        }
        private int StartVertice()
        {
            int startNode = 0;
            for (int i = 0; i < numOfNodes; i++)
            {
                //if there's a vertice outdegree[i]-indegree[i]==1
                //then pick that vertice as a starting node
                if (outdegree[i] - indegree[i] == 1)
                    return i;
                if (outdegree[i] > 0) // if not , take last vertice of i which has outdegree  
                    startNode = i;
            }

            return startNode;
        }
        private void DFS(int at)
        {
            while (outdegree[at]!=0)
            {
                // decrease outdegree
                // rather using visited[] , we made use of outdegree[at]
                int next = myGraph[at].ElementAt(--outdegree[at]).Key;
                DFS(next);
            }

            path.AddFirst(at);
        }
        private bool PathExistsCheck()
        {
            int startNodes=0, endNodes=0;
            for (int i = 0; i < numOfNodes ; i++)
            {
                //| indegree[i]-outdegree[i] | == 1 , only valid condition
                if (indegree[i] - outdegree[i] > 1 || outdegree[i] - indegree[i] > 1)
                    return false;
                else if (outdegree[i] - indegree[i] == 1)
                    startNodes++;
                else if (indegree[i] - outdegree[i] == 1)
                    endNodes++;
            }

            return (endNodes == 0 && startNodes == 0)
                   || (endNodes == 1 && startNodes == 1);
        }
    }
}
