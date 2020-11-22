using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GraphStructure
{
    //Adjacency Matrix(nxn Matrix) - Adjacency List(Linked List)
    //Graph veri yapısı iki türde belirtilebilir
    //Kare matriks formunda bağlantılar,düğümler veya noktalar yazılabilir

    //    0 1 2 3 4    Adjacency Matrix             
    // 0 [0,1,0,1,0]                 1 --- 2
    // 1 [1,0,1,1,0]               / |   /  |
    // 2 [0,1,0,1,1]             0   |  /   | => edges , köşeler veya bağlantı yolları
    // 3 [1,1,1,0,1]               \ | /    |
    // 4 [0,0,1,1,0]                 3 --- 4

    // Depolama Alanı için karmaşıklık O(n^2) => 25 , n = 5 nokta veya düğüm sayısı
    //                     __ 
    // Adjacency List => 0|__| --1-->3
    //                   1|__| --0-->2-->3
    //                   2|__| --1-->3-->4          Depolama Karmaşası => O(n+2*Edges) => 5 + 2*7 = 19
    //                   3|__| --0-->1-->2-->4
    //                   4|__| --2-->3

    // Elinizdeki bağlantı noktaları yoğunsa Adjacency Matrix kullanılır
    // Az bağlantı sayılı veri kümeleri için Adjacency List kullanılır

    // Graph Traversal , BFS - DFS
    // Breadth First Search (Sığ öncelikli arama)
    // Depth First Search (Derinlik öncelikli arama)
    // BFS için Queue veri yapısı kullanılır
    // DFS için Stack veri yapısı kullanılır

    // Spanning Tree, Grafik veri yapısındaki tüm düğüm, elementleri minimum bağlantı sayısıyla içeren alt grafik kümeleridir.
    // n - 1 minimum subtree bağlantı sayısı , dört element için en az üç bağlantıya ihtiyaç var.
    // 
    //          a ------- b     Bağlantılar, a-b , a-c , c-d , b-d , a-d , b-c
    //          | .     . |
    //          |    .    |     Spanning Subgraph a-b, b-d, d-c : üç bağlantı!
    //          | .     . |
    //          c ------- d
    // Döngü oluşturmadan elde edilebilen, tüm element , düğümleri içeren en sade alt-ağaç Spanning Tree dir.
    // Complete Graph n^(n-2) adet Spanning Tree içerebilir
    // Max: Edges - n + 1 adet bağlantı silinirse Spanning Tree oluşur
    // Maximum Edges: n*(n - 1)/2

    //Directed Graph , Undirected Graph , Mixed Graph 
    //Degree Of A Vertex --> Her bir düğümün sahip olduğu bağlantı sayısı (Edge)
    //Directed Graph , Degree Of A Vertex --> Indegree (Düğüme doğru) - Outdegree (Düğümden dışa doğru)

    //Mother Vertex --> Grafikteki diğer düğümlerle bağlantı kurmuş olan düğümdür
    //Bir grafikte birden fazla anne düğüm olabilir
    class GraphList<K> where K : IComparable<K>
    {
        public enum TYPE //Bağlantıların yönlü veya yönsüz olma durumu
        {
            Directed,
            Undirected
        } 

        private List<Vertex> allVertices; //Tüm düğümleri içeren liste 
        private List<Edge> allEdges; //Tüm bağlantılar
        private TYPE type;

        public GraphList()
        {
            this.type = TYPE.Undirected;
            allEdges = new List<Edge>();
            allVertices = new List<Vertex>();
        } //İlk değerlerin atanmasını sağlar
        public GraphList(TYPE type)
        {
            this.type = type;
            allEdges = new List<Edge>();
            allVertices = new List<Vertex>();
        }//Tip belirtilen yapılandırıcı(Constructor)
        public GraphList(GraphList<K> graphList)
        {
            type = graphList.TypeGet();
            allEdges = new List<Edge>();
            allVertices = new List<Vertex>();

            foreach (var graphListAllVertex in graphList.GetVertices())
            {
                this.allVertices.Add(graphListAllVertex);
            }

            foreach (var graphListAllEdge in graphList.GetEdges())
            { 
                this.allEdges.Add(graphListAllEdge);
            }
        }//İstenlen GraphList nesnesinin kılonlanması işlemi
        public List<Vertex> GetVertices()
        {
            return allVertices;
        }
        public List<Edge> GetEdges()
        {
            return allEdges;
        }
        public List<Edge> GetUndirectedSingleEdges()
        {
            List<Edge> tempList=new List<Edge>();
            for (int i = 0; i < allEdges.Count(); i+=2)
            {
                //Undirected yapıda her değer iki kez yazılır, bu durumdan kurtulmak için for döngüsünden yararlandım
                tempList.Add(allEdges[i]);
            }

            //allEdges listesini değiştirmemek adına tempList oluşturdum
            return tempList;
        }
        private Vertex From(K value)
        {
            var check = GetVertices().Where(x => x.GetValue().CompareTo(value) == 0);
            if(check.Count()==0)
                throw new NullReferenceException("Check the value you're looking pointing to!");

            var from = check.First();
            return from;
        }//Edge yapılandırıcısına parametre olarak verilen Edge(cost,from,to) from terimi
        private Vertex To(K value)
        {
            var check = GetVertices().Where(x => x.GetValue().CompareTo(value) == 0);
            if (check.Count() == 0)
                throw new NullReferenceException("Check the value you're looking pointing to!");

            var to = check.First();
            return to;
        }//Edge yapılandırıcısına parametre olarak verilen to terimi
        private TYPE TypeGet() => this.type;
        public List<Edge> RemoveVertexCircularLoops()
        {
            List<Edge> tempEdges = allEdges;
            tempEdges.RemoveAll(x => x.From.Value.CompareTo(x.To.Value) == 0);
            return tempEdges;
        }//Düğümün kendi üstüne olan döngüsünü kaldırır
        public void AddVertex(K value, int weight)
        {
            Vertex newvVertex = new Vertex(value, weight);
            this.allVertices.Add(newvVertex);//Vertex nesnesi oluşturulur, oluşan nesne allVertices listesine eklenir
        }
        public void AddEdge(int cost, K from, K to)
        {
            Edge newEdge = new Edge(cost, From(from), To(to));
            if (type == TYPE.Undirected) //Undirected grafik olması durumunda ikinci bağlantının otomatik olarak eklenmesi
            {
                Edge newExtraEdge = new Edge(cost, To(to), From(from));
                this.allEdges.Add(newExtraEdge);
            }
            
            this.allEdges.Add(newEdge);
        }
        public int RemoveEdge(K from,K to)
        {
            try
            {
                Edge removeEdge = this.allEdges.Where(x => x.From == From(from) && x.To == To(to)).First();
                if (type==TYPE.Undirected) //Undirected olması halinde eklenen ikinci bağlantının kaldırılması
                {
                    //Predicate özelliğiyle allEdges içerisindeki Edge nesnesinin elemanları üzerinden arama yapabiliriz
                    //Undirected olaması durumunda x.From == To(to) ve x.To == From(from) ifadesi doğru olacağından ek bağıntılar silinir
                    Edge removeExtraEdge = this.allEdges.Where(x => x.From == To(to) && x.To == From(from)).First();
                    this.allEdges.RemoveAll(x => x == removeExtraEdge);
                }
                return this.allEdges.RemoveAll(x => x == removeEdge);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Exception Message on RemoveEdge({From(from).Value},{To(to).Value}) method: "+exception.Message+" from "+From(from).Value+" to "+To(to).Value );
                return default;
            }
        }
        public int RemoveVertex(K value)
        {
            try
            {
                Vertex removeEdge = this.allVertices.Where(x => x.Value.CompareTo(value) == 0).First();
                return this.allVertices.RemoveAll(x => x == removeEdge);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Exception Message on RemoveVertex({value}) method: " + exception.Message +
                                  " for value " + value);
                return default;
            }
            finally
            {
                //Düğümün ilgili bağlantıları kaldırılır
                this.allEdges.RemoveAll(x => x.From.Value.CompareTo(value)==0);
                this.allEdges.RemoveAll(x => x.To.Value.CompareTo(value) == 0);
            }
        }
        public Vertex CloneVertex(Vertex vertex)
        {
            Vertex newVertex=new Vertex(vertex);
            return newVertex;
        }
        public Edge CloneEdge(Edge edge)
        {
            Edge newEdge = new Edge(edge);
            return newEdge;
        }
        public int NumberOfVertices() => GetVertices().Count();
        public int NumberOfEdges()
        {
            if (type==TYPE.Undirected)
            {
                //Grafik undirected yapıdaysa her değeri iki kez "saymamak" için bu ifade kullanılır
                return GetUndirectedSingleEdges().Count();
            }
            else
            {
                return GetEdges().Count();
            }
        }
        public class Vertex
        {
            public bool IsVisited { get; private set; } //Prims ve diğer algoritmalar için gereken parametre
            public K Value { get; private set; }
            public int Weight { get; private set; }

            private List<Edge> edges;
            public Vertex(K value)
            {
                this.IsVisited = false;
                this.Value = value;
            }
            public Vertex(K value, int weight)
            {
                this.IsVisited = false;
                this.Value = value;
                this.Weight = weight;
            }
            public Vertex(Vertex vertex)
            {
                this.IsVisited = false;
                edges = new List<Edge>();
                Value = vertex.Value;
                Weight = vertex.Weight;
                foreach (var vertexEdge in vertex.edges)
                {
                    this.edges.Add(vertexEdge);
                }
            }
            public K GetValue() => Value;
            public int GetWeight() => Weight;

            private void SetWeight(int weight)
            {
                this.Weight = weight;
            }

            private void AddEdge(Edge edge)
            {
                this.edges.Add(edge);
            }

            public List<Edge> GetEdges() => this.edges;

            private Edge GetEdge(Vertex vertex)
            {
                foreach (var edge in edges)
                {
                    if (edge.To.Equals(vertex))
                        return edge;
                }

                return null;
            }

            private bool PathTo(Vertex vertex)
            {
                foreach (var edge in edges)
                {
                    if (edge.To.Equals(vertex))
                        return true;
                }

                return false;
            }

            public override int GetHashCode()
            {
                int code = this.Value.GetHashCode() + this.Weight + this.edges.Count();
                return 31 * code;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Vertex))
                    return false;

                Vertex vertex = (Vertex) obj;

                bool weightEquals = this.Weight == vertex.Weight;
                if (!weightEquals)
                    return false;

                bool edgesSizeEquals = this.edges.Count() == vertex.edges.Count();
                if (!edgesSizeEquals)
                    return false;

                bool valuesEquals = this.Value.Equals(vertex.Value);
                if (!valuesEquals)
                    return false;

                foreach (var edge in this.edges)
                {
                    foreach (var vertexEdge in vertex.edges)
                    {
                        if (edge.Cost != vertexEdge.Cost)
                            return false;
                    }
                }

                return true;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Value=").Append(Value).Append(" weight=").Append(Weight).Append("\n");
                if (this.edges != null)
                {
                    foreach (Edge edge in this.edges)
                        builder.Append("\t").Append(edge.ToString());
                }

                return builder.ToString();
            }
        }
        public class Edge
        {
            public bool IsVisited { get; private set; }
            public Vertex From { get; private set; }
            public Vertex To { get; private set; }
            public int Cost { get; private set; }

            public Edge(int cost, Vertex from, Vertex to)
            {
                if (from == null || to == null)
                    throw new NullReferenceException("Vertex can't be null");

                this.IsVisited = false;
                this.From = from;
                this.To = to;
                this.Cost = cost;
            }

            public Edge(Edge edge)
            {
                this.IsVisited = false;
                this.From = edge.From;
                this.To = edge.To;
                this.Cost = edge.Cost;
            }

            private int GetCost() => this.Cost;

            private void SetCost(int cost)
            {
                this.Cost = cost;
            }

            private Vertex GetFromVertex() => this.From;
            private Vertex GetToVertex() => this.To;

            public override int GetHashCode()
            {
                int cost = (this.Cost * (this.GetFromVertex().GetHashCode() * this.GetToVertex().GetHashCode()));
                return 31 * cost;
            }

            public override bool Equals(Object obj)
            {
                if (!(obj is Edge))
                    return false;

                Edge edge = (Edge) obj;

                bool costs = this.Cost == edge.Cost;
                if (!costs)
                    return false;

                bool from = this.From.Equals(edge.From);
                if (!from)
                    return false;

                bool to = this.To.Equals(edge.To);
                if (!to)
                    return false;

                return true;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("[ ").Append(From.Value).Append("(").Append(From.Weight).Append(") ").Append("]")
                    .Append(" -> ")
                    .Append("[ ").Append(To.Value).Append("(").Append(To.Weight).Append(") ").Append("]").Append(" = ")
                    .Append(Cost).Append("\n");
                return builder.ToString();
            }
        }
        public void SortedEdgesOnCost(List<Edge> edgeList)
        {
            //İkiz terimleri listeye almaz, bu nedenle Kruska teoremine uygun davranır
            //a-->b ---- b-->a , bağlantıların birini dikkate alır
            edgeList.Sort((x,y)=>x.Cost.CompareTo(y.Cost));
        }//Kruskal Algritması için sıralama metodu
        public override int GetHashCode()
        {
            int code = this.type.GetHashCode() + this.allVertices.Count() + this.allEdges.Count();
            foreach (Vertex v in allVertices)
                code *= v.GetHashCode();
            foreach (Edge e in allEdges)
                code *= e.GetHashCode();

            return 31 * code;
        }
        public override bool Equals(Object obj)
        {
            if (!(obj is GraphList<K>))
                return false;

            GraphList<K> graphList = (GraphList<K>) obj;

            bool typeEquals = this.type == graphList.type;
            if (!typeEquals)
                return false;

            bool verticesSizeEquals = this.allVertices.Count() == graphList.allVertices.Count();
            if (!verticesSizeEquals)
                return false;

            bool edgesSizeEquals = this.allEdges.Count() == graphList.allEdges.Count();
            if (!edgesSizeEquals)
                return false;

            //İkiz düğümler için gereken algoritma
            Object[] ov1 = this.allVertices.ToArray();
            Array.Sort(ov1);
            Object[] ov2 = graphList.allVertices.ToArray();
            Array.Sort(ov2);
            for (int i = 0; i < ov1.Length; i++)
            {
                Vertex v1 = (Vertex) ov1[i];
                Vertex v2 = (Vertex) ov2[i];
                if (!v1.Equals(v2))
                    return false;
            }

            //İkiz bağlantılar için
            Object[] oe1 = this.allEdges.ToArray();
            Array.Sort(oe1);
            Object[] oe2 = graphList.allEdges.ToArray();
            Array.Sort(oe2);
            for (int i = 0; i < oe1.Length; i++)
            {
                Edge e1 = (Edge) oe1[i];
                Edge e2 = (Edge) oe2[i];
                if (!e1.Equals(e2))
                    return false;
            }

            return true;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Vertex v in allVertices)
                builder.Append(v.ToString());

            return builder.ToString();
        }
        public void DisplayTheGraph()
        {
            Console.WriteLine("All Vertices");
            Console.WriteLine("-----------------");
            foreach (var vertex in GetVertices())
            {
                Console.WriteLine(vertex);
            }

            Console.WriteLine("-----------------");
            Console.WriteLine("All Edges");
            if (type==TYPE.Undirected)
            {
                foreach (var edge in GetUndirectedSingleEdges())
                {
                    Console.WriteLine(edge.From.Value + "-->" +
                                      edge.To.Value + " Cost: " + edge.Cost);
                }
            }
            else
            {
                foreach (var edge in GetEdges())
                {
                    Console.WriteLine(edge.From.Value + "-->" +
                                      edge.To.Value + " Cost: " + edge.Cost);
                }
            }
            
        }
    }
}
