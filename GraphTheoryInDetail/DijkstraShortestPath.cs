using System;
using Priority_Queue;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    // shortest path for undirected graphs , non negative edge weights
    // O(E*log(V)) time complexity
    class DijkstraShortestPath
    {
        private List<KeyValuePair<int, double>>[] myGraph;
        public List<KeyValuePair<int, double>>[] GetGraph
        {
            get { return myGraph; }
        }
        public DijkstraShortestPath(int numOfVertices)
        {
            myGraph = new List<KeyValuePair<int, double>>[numOfVertices];
           
            for (int i = 0; i < numOfVertices; i++)
            {
                myGraph[i] = new List<KeyValuePair<int, double>>();
            }
        }
        public void CreateUnDirectedEdges(int fromVer, int toVer, double weight)
        {
            myGraph[fromVer].Add(new KeyValuePair<int, double>(toVer, weight));
            myGraph[toVer].Add(new KeyValuePair<int, double>(fromVer, weight));
        }
        public double LazyDijkstraShortPath(List<KeyValuePair<int,double>>[] graphAdjList,
            int startNode,int endNode,out bool[] path)
        {
            int numOfVertices = graphAdjList.Length;
            bool[] visited = new bool[numOfVertices];
            double[] distance = new double[numOfVertices];
            bool[] resultPath=new bool[numOfVertices];//path you visit

            for (int i = 0; i < numOfVertices; i++)
            {
                distance[i] = double.PositiveInfinity;
            }

            distance[startNode] = 0;

            resultPath[startNode] = true;
            resultPath[endNode] = true;

            SimplePriorityQueue<int,double> priorityQueue=
                new SimplePriorityQueue<int,double>();

            priorityQueue.Enqueue(startNode,0.0);

            while (priorityQueue.Count!=0)
            {
                int indexofNode = priorityQueue.Dequeue();
                visited[indexofNode] = true;

                foreach (var keyValuePair in graphAdjList[indexofNode])
                {
                    double minValue = keyValuePair.Value;

                    if (distance[keyValuePair.Key] < minValue)
                        continue;

                    if (visited[keyValuePair.Key])
                        continue;

                    double distNew = distance[indexofNode] + keyValuePair.Value;

                    if (distNew < distance[keyValuePair.Key])
                    {
                        resultPath[indexofNode] = true;
                        distance[keyValuePair.Key] = distNew;
                        priorityQueue.Enqueue(keyValuePair.Key, distNew);
                    }
                    
                }

                if (indexofNode == endNode)
                {
                    path = resultPath;
                    return distance[endNode];
                }
                    
            }

            path = resultPath;
            return double.PositiveInfinity;
        }
        public double EagerDijkstraShortPath(List<KeyValuePair<int, double>>[] graphAdjList,
            int startNode, int endNode,out bool[] path)
        {
            int numOfVertices = graphAdjList.Length;
            bool[] visited = new bool[numOfVertices];
            double[] distance = new double[numOfVertices];
            bool[] resultPath = new bool[numOfVertices];//path you visit

            for (int i = 0; i < numOfVertices; i++)
            {
                distance[i] = double.PositiveInfinity;
            }

            distance[startNode] = 0;

            resultPath[startNode] = true;
            resultPath[endNode] = true;

            SimplePriorityQueue<int, double> priorityQueue =
                new SimplePriorityQueue<int, double>();

            priorityQueue.Enqueue(startNode, 0.0);

            while (priorityQueue.Count != 0)
            {
                int indexofNode = priorityQueue.Dequeue();
                visited[indexofNode] = true;

                foreach (var keyValuePair in graphAdjList[indexofNode])
                {
                    double minValue = keyValuePair.Value;

                    if (distance[keyValuePair.Key] < minValue)
                        continue;

                    if (visited[keyValuePair.Key])
                        continue;

                    double distNew = distance[indexofNode] + keyValuePair.Value;

                    if (distNew < distance[keyValuePair.Key])
                    {
                        resultPath[indexofNode] = true;
                        distance[keyValuePair.Key] = distNew;
                        if(!priorityQueue.EnqueueWithoutDuplicates(keyValuePair.Key, distNew))
                            priorityQueue.UpdatePriority(keyValuePair.Key,distNew);
                    }
                }

                if (indexofNode == endNode)
                {
                    path = resultPath;
                    return distance[endNode];
                }
                    
            }

            path = resultPath;
            return double.PositiveInfinity;
        }
    }
}
