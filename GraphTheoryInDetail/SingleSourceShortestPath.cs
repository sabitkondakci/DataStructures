using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    class SingleSourceShortestPath
    {
        private List<KeyValuePair<int, long>>[] myGraph;
        private List<int>[] children;
        public List<KeyValuePair<int, long>>[] GetGraph
        {
            get { return myGraph; }
        }
        public SingleSourceShortestPath(int numOfVertices)
        {
            myGraph = new List<KeyValuePair<int, long>>[numOfVertices];
            children = new List<int>[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                myGraph[i] = new List<KeyValuePair<int, long>>();
                children[i] = new List<int>();
            }
        }
        public void CreateDirectedEdges(int fromVer, int toVer, long weight)
        {
            myGraph[fromVer].Add(new KeyValuePair<int, long>(toVer, weight));
            children[toVer].Add(fromVer);
        }
        public long[] ShortestPath(List<KeyValuePair<int, long>>[] graphAdjList,int startNode)
        {
            int numOfVertices = graphAdjList.Length;
            int[] topologicalSort = TopologicalSort(graphAdjList);
            long[] distance=new long[numOfVertices];//distance arrray, this will be updated in every loop

            distance[startNode] = 0;

            for (int i = 0; i < numOfVertices; i++)
            {
                int indexOfNode = topologicalSort[i];
                List<KeyValuePair<int, long>> adjEdges = graphAdjList[indexOfNode];

                if (adjEdges != null)//if an edge list exists
                {
                    foreach (var keyValuePair in adjEdges)
                    {
                        long refreshedDistance = distance[indexOfNode] + keyValuePair.Value;

                        if (distance[keyValuePair.Key] == 0)
                            distance[keyValuePair.Key] = refreshedDistance;
                        else
                            distance[keyValuePair.Key] = Math.Min(distance[keyValuePair.Key], refreshedDistance);

                    }
                }

            }

            return distance;
        }
        //Kahn's Topological Sort Algorithm
        private int[] TopologicalSort(List<KeyValuePair<int, long>>[] graphAdjList)
        {
            int numOfVertices = graphAdjList.Length;
            int[] inDegreeArr = new int[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                foreach (var keyValuePair in graphAdjList[i])
                {
                    inDegreeArr[keyValuePair.Key]++;
                }
            }

            //a queue to insert values in inDegreeArr, inDegreeArr[i] == 0
            Queue<int> myQueue = new Queue<int>();

            for (int i = 0; i < numOfVertices; i++)
            {
                if (inDegreeArr[i] == 0)
                    myQueue.Enqueue(i);
            }

            int count = 0;
            int[] order = new int[numOfVertices];

            while (myQueue.Count != 0)
            {
                int at = myQueue.Dequeue();
                order[count++] = at;
                foreach (var keyValuePair in graphAdjList[at])
                {
                    inDegreeArr[keyValuePair.Key]--;
                    if (inDegreeArr[keyValuePair.Key] == 0)
                        myQueue.Enqueue(keyValuePair.Key);
                }
            }

            if (count != numOfVertices) //if there's a cycle in graph structure ,it leaves early
                throw new Exception("Graph contains cycle!");

            return order;
        }
    }
}
