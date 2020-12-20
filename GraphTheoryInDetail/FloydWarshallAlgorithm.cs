using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    //the algorithm is only used for small amount of vertices
    //time complexity : O(n^3) which is n^2(from dijkstra) x n(for every vertice)
    //a 3D array is the best solution 
    // A[i,j] = Math.Min(A[i,j], A[i,k] + A[k,j])
    class FloydWarshallAlgorithm
    {
        public double[,] my2DGraph { get; }
        public int N { get; }
        public FloydWarshallAlgorithm(int numOfVertices)
        {
            N = numOfVertices;
            my2DGraph=new double[numOfVertices,numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                for (int j = 0; j < numOfVertices; j++)
                {
                    my2DGraph[i,j] = double.PositiveInfinity;
                }

                my2DGraph[i, i] = 0;
            }
        }

        public void CreateDirectedEdges(int fromNode,int endNode,double cost)
        {
            my2DGraph[fromNode, endNode] = cost;
        }

        //inserted parameter of my2DGraph will change at the end
        //so that use a copy of your graph
        //return true or false if graph contains a negative cycle
        public bool FloydWarshallForEachNode(double[,] my2DGraph)
        {
            int numOfNodes = my2DGraph.GetLength(0);
            bool negCycle = false;

            for (int k = 0; k < numOfNodes; k++)
            {
                for (int i = 0; i < numOfNodes; i++)
                {
                    for (int j = 0; j < numOfNodes; j++)
                    {
                        my2DGraph[i, j] = Math.Min(my2DGraph[i, j],
                            my2DGraph[i, k] + my2DGraph[k, j]);
                    }
                }
            }

            //negative cycles
            for (int k = 0; k < numOfNodes; k++)
            {
                for (int i = 0; i < numOfNodes; i++)
                {
                    for (int j = 0; j < numOfNodes; j++)
                    {
                        if (my2DGraph[i, j] > my2DGraph[i, k] + my2DGraph[k, j])
                        {
                            my2DGraph[i, j] = double.NegativeInfinity;
                            negCycle = true;
                        }
                    }
                }
            }

            return negCycle;
        }

    }
}
