using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //the algithm is faster but less space efficient
    //by reason of heristic values should be stored beforehand
    class AStarAlgorithmWithEagerDijkstra
    {
        private List<KeyValuePair<int, double>>[] myGraph;
        public List<KeyValuePair<int, double>>[] GetGraph
        {
            get { return myGraph; }
        }
        public AStarAlgorithmWithEagerDijkstra(int numOfVertices)
        {
            myGraph = new List<KeyValuePair<int, double>>[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                myGraph[i] = new List<KeyValuePair<int, double>>();
            }
        }
        public void CreateDirectedEdges(int fromVer, int toVer, double weight)
        {
            myGraph[fromVer].Add(new KeyValuePair<int, double>(toVer, weight));
        }
        //the algorithm eliminates less likely patterns from the graph
        //only searches on heuristicList and returns the path
        public double AStarShortestPathAndDistance(List<KeyValuePair<int, double>>[] graphAdjList,
          int startNode, int endNode, out List<int> path)
        {
            int numOfVertices = graphAdjList.Length;
            //this must be precalculated in dynamic systems
            //a list which holds shortest distance from start to end , for every node
            List<double> heuristicList = ShortestDistanceListHeuristic(graphAdjList, endNode);

            if (endNode < 0 || endNode >= numOfVertices) throw new ArgumentOutOfRangeException("Invalid node index");
            if (startNode < 0 || startNode >= numOfVertices) throw new ArgumentOutOfRangeException("Invalid node index");

            bool[] visited = new bool[numOfVertices];
            double[] distance = new double[numOfVertices];
            //a similiar list like distance, which has double.PositiveInfinity value by default
            double[] heuristicNewDistance = new double[numOfVertices];

            List<int> resultPath = new List<int>();//path you visit
            int[] prevNode = new int[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                distance[i] = double.PositiveInfinity;
                heuristicNewDistance[i] = double.PositiveInfinity;
            }

            distance[startNode] = 0;

            SimplePriorityQueue<int, double> priorityQueue =
                new SimplePriorityQueue<int, double>();

            priorityQueue.Enqueue(startNode, heuristicList[startNode]);

            while (priorityQueue.Count != 0)
            {
                int indexofNode = priorityQueue.Dequeue();
                visited[indexofNode] = true;

                foreach (var keyValuePair in graphAdjList[indexofNode])
                {                  
                    double heuristicMinValue = heuristicList[keyValuePair.Key];

                    if (heuristicNewDistance[keyValuePair.Key] < heuristicMinValue)
                        continue;

                    if (visited[keyValuePair.Key])
                        continue;

                    double distNew = distance[indexofNode] + keyValuePair.Value;
                    double heurDistNew = distNew + heuristicList[keyValuePair.Key];

                    if (heurDistNew < heuristicNewDistance[keyValuePair.Key])
                    {
                        prevNode[keyValuePair.Key] = indexofNode;
                        distance[keyValuePair.Key] = distNew;
                        heuristicNewDistance[keyValuePair.Key] = heurDistNew;

                        //Priority Queue for heristicList values
                        if (!priorityQueue.EnqueueWithoutDuplicates(keyValuePair.Key, heurDistNew))
                            priorityQueue.UpdatePriority(keyValuePair.Key, heurDistNew);

                    }
                }

                if (indexofNode == endNode)
                {
                    for (int at = endNode; at != startNode; at = prevNode[at])
                        resultPath.Add(at);

                    if (!resultPath.Contains(startNode))
                        resultPath.Add(startNode);
                    if (!resultPath.Contains(endNode))
                        resultPath.Add(endNode);

                    resultPath.Reverse();

                    path = resultPath;
                    return distance[endNode];
                }

            }

            path = resultPath;
            return double.PositiveInfinity;
        }
        private double EagerDijkstraShortPath(List<KeyValuePair<int, double>>[] graphAdjList,
            int startNode, int endNode)
        {
            int numOfVertices = graphAdjList.Length;

            if (endNode < 0 || endNode >= numOfVertices) throw new ArgumentOutOfRangeException("Invalid node index");
            if (startNode < 0 || startNode >= numOfVertices) throw new ArgumentOutOfRangeException("Invalid node index");

            bool[] visited = new bool[numOfVertices];
            double[] distance = new double[numOfVertices];
            List<int> resultPath = new List<int>();//path you visit

            for (int i = 0; i < numOfVertices; i++)
            {
                distance[i] = double.PositiveInfinity;
            }

            distance[startNode] = 0;

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
                        distance[keyValuePair.Key] = distNew;
                        if (!priorityQueue.EnqueueWithoutDuplicates(keyValuePair.Key, distNew))
                            priorityQueue.UpdatePriority(keyValuePair.Key, distNew);

                    }
                }

                if (indexofNode == endNode)
                {
                    return distance[endNode];
                }

            }

            return double.PositiveInfinity;
        }

        //this returns shortest path values for each vertice
        private List<double> ShortestDistanceListHeuristic(List<KeyValuePair<int, double>>[] graphAdjList,int endNode)
        {
            List<double> shrDistance=new List<double>();
            for (int i = 0; i < graphAdjList.Length; i++)
            {
                shrDistance.Add(EagerDijkstraShortPath(graphAdjList,i,endNode));
            }

            return shrDistance;
        }

    }
}
