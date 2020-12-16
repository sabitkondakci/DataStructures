using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    // for such sorting, ordering graph should be a directed acyclic kind
    // a topological sort algorithm which has a O(V+E) time complexity 
    // also it detects cycles with ease , via count variable
    class KahnAlgorithmTopologicalSort
    {
        private List<KeyValuePair<int, long>>[] myGraph;
        private List<int>[] children;
        public List<KeyValuePair<int, long>>[] GetGraph
        {
            get { return myGraph; }
        }
        public KahnAlgorithmTopologicalSort(int numOfVertices)
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

        //implication of Kahn's Algorithm
        public int[] TopologicalSort(List<KeyValuePair<int, long>>[] graphAdjList)
        {
            int numOfVertices = graphAdjList.Length;
            int[] inDegreeArr=new int[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                foreach (var keyValuePair in graphAdjList[i])
                {
                    inDegreeArr[keyValuePair.Key]++;
                }
            }

            //a queue to insert values in inDegreeArr, inDegreeArr[i] == 0
            Queue<int> myQueue=new Queue<int>();

            for (int i = 0; i < numOfVertices; i++)
            {
                if(inDegreeArr[i]==0)
                    myQueue.Enqueue(i);
            }

            int count = 0;
            int[] order=new int[numOfVertices];

            while (myQueue.Count!=0)
            {
                int at = myQueue.Dequeue();
                order[count++] = at;
                foreach (var keyValuePair in graphAdjList[at])
                {
                    inDegreeArr[keyValuePair.Key]--;
                    if(inDegreeArr[keyValuePair.Key]==0)
                        myQueue.Enqueue(keyValuePair.Key);
                }
            }

            if(count!=numOfVertices) //if there's a cycle in graph structure ,it leaves early
                throw new Exception("Graph contains cycle!");

            return order;
        }

    }
}
