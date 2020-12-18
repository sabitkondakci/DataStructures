using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    class BellmanFordNegativeCycle
    {
        private List<KeyValuePair<int, double>>[] myGraph;
        public List<KeyValuePair<int, double>>[] GetGraph
        {
            get { return myGraph; }
        }
        public BellmanFordNegativeCycle(int numOfVertices)
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
        public double[] BellmanFord(List<KeyValuePair<int,double>>[] myAdjGraph)
        {
            int numberOfVertices = myAdjGraph.Length;
            double[] distance = new double[numberOfVertices];
            for (int i = 0; i < numberOfVertices; i++)
            {
                distance[i] = double.PositiveInfinity;
            }
            distance[0] = 0;

            // Only in the worst case does it take numberOfVertices-1 iterations for the Bellman-Ford
            // Another stopping condition is when we're unable to
            // relax an edge, this means we have reached the optimal solution early.
            
            // For each vertex, apply relaxation for all the edges
            for (int v = 0; v < numberOfVertices - 1 ; v++)
            {
                foreach (var edge in myAdjGraph[v])
                {
                    if (distance[v] + edge.Value < distance[edge.Key])
                    {
                        distance[edge.Key] = distance[v] + edge.Value;
                    }
                }
            }

            // Run algorithm a second time to detect which nodes are part
            // of a negative cycle. A negative cycle has occurred if we
            // can find a better path beyond the optimal solution.
          

            for (int v = 0; v < numberOfVertices - 1 ; v++)
            {
                foreach (var edge in myAdjGraph[v])
                {
                    if (distance[v] + edge.Value < distance[edge.Key])
                    {
                        distance[edge.Key] = double.NegativeInfinity;
                    }
                }
            }

            // Return the array containing the shortest distance to every node
            return distance;
        }
    }
}
