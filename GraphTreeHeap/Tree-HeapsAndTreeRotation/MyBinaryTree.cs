using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSharp.DataStructures.RootedTreeSpace;

namespace TreeAndHeaps
{
    //[2^(l+1) - 1] l. levelde toplam maksimum eleman sayısı, 1.level = 3 , 2.level=7 
    //[2^(l+1) - 1]==> 2^0 +2^1 +2^2 + ..... + 2^l
    //l. levelde maksimum eleman sayısı, 2^l , 1.level=2, 2.level=4 ,3.level=8 vs.

    //max-height = n-1 ,            n: Toplam eleman sayısı
    //min-height =log2^(n+1) - 1

    //Level of a tree = Height of a tree , Ağacın seviyesi ve yüksekliği aynı anlamı taşır.
    //Level of a node = Depth of a node  , Düğümün seviyesi ve derinliği aynı anlamı taşır.
    //        o         Level 0
    //      /   \       
    //     o     o      Level 1
    //    / \   / \
    //   o   o o   o    Level 2 

    //Full Binary Tree; her bir düğüm ya iki ya da sıfır çocuğa sahiptir.
    //      o
    //     / \
    //    o   o             Number of leaf node = Number of internal nodes + 1
    //       / \            Max-node: 2^(l+1) - 1           Max-height=(n-1)/2
    //      o   o           Min-node: 2l + 1                Min-height=log2^(n+1) - 1
    //
    //
    //Complete Binary Tree: Her bir levelin en sola yatkın olması koşuluyla doldurulması gereken ağaçlardır.
    //      o                                   o                                      o
    //     / \                                 / \                                   /   \  
    //    o   o  <= Kabul edilebilir.         o   o     <= Kabul edilemez.          o     o   <= Kabul edilebilir.
    //   / \                                     / \                               / \   /
    //  o   o                                   o   o                             o   o o 
    //
    // Max-node: 2^(l+1) - 1         Min-node: 2^l
    // Max-height=log2^n             Min-height=log2^(n+1) - 1
    //
    //Perfect Binary Tree: Yaprak düğümler hariç tüm düğümlerin iki çocuğa sahip olduğu ve tüm seviyelerin eşit, tümüyle dolu olduğu ağaçlardır.
    //       o                  
    //     /   \                Max-node: 2^(l+1) - 1         Min-node: 2^(l+1) - 1
    //    o     o               Max-height=log2^(n+1) - 1     Min-height=log2^(n+1) - 1
    //   / \   / \
    //  o   o o   o

    class MyBinaryTree<T> where T:IEquatable<T>,IComparable<T>
    {
        private List<T> tempList;
        private int size;
        private Node root;
        public MyBinaryTree()
        {
            size = 0;
            root = null;
            tempList=new List<T>();
        }
        private class Node
        {
            public T data;
            public Node left;
            public Node right;

            public Node(T data)
            {
                this.data = data;
                this.left = null;
                this.right = null;
            }

        }
        public void Add(T data)
        {
            if (root==null)
            {
                root=new Node(data);
            }
            else
            {
                //Girilen değer BST formatına uygun olarak ağaca yerleştirilir
                //Özçağrılı (Recursive) işlem başlatılır.
                CheckAdd(data,root);
            }

            size++;
        }
        public T Remove(T data)
        {
            if (Contains(data))
            { 
               root=CheckRemove(data, root); // Silme işlemi BST kurallarınca kaldırılır.
            }

            return data;
        }
        public T MaxValue()
        {
            Node checkNode = root;
            while (checkNode.right!=null)
            {
                checkNode = checkNode.right;
            }

            return checkNode.data;
        } //En sağdaki değeri döndürür.
        public T MinValue()
        {
            Node checkNode = root;
            while (checkNode.left != null)
            {
                checkNode = checkNode.left;
            }

            return checkNode.data;
        }//En soldaki değeri döndürür
        public int Size() => size;
        public bool IsEmpty() => size == 0;
        public bool Contains(T data) => CheckContains(data, root);

        //PreOrder: node, left ,right
        public List<T> PreOrderTraversal()
        {
            List<T> volatileList = new List<T>();
            PreTrav(root);

            foreach (var num in tempList)
            {
                volatileList.Add(num);
            }

            tempList.Clear();
            return volatileList;
        }
        //PostOrder: left, right, node
        public List<T> PostOrderTraversal()
        {
            List<T> volatileList = new List<T>();
            PostTrav(root);

            foreach (var num in tempList)
            {
                volatileList.Add(num);
            }

            tempList.Clear();
            return volatileList;
        }
        //InOrder: left, node, right
        public List<T> InOrderTraversal()
        {
            List<T> volatileList = new List<T>();
            InOrdTrav(root);

            foreach (var num in tempList)
            {
                volatileList.Add(num);
            }

            tempList.Clear();
            return volatileList;
        }
        private void PreTrav(Node rootNode)
        {
            if (rootNode == null)
                return;

            tempList.Add(rootNode.data);
            PreTrav(rootNode.left);
            PreTrav(rootNode.right);

        }
        private void PostTrav(Node rootNode)
        {
            if (rootNode == null)
                return;

            PostTrav(rootNode.left);
            PostTrav(rootNode.right);
            tempList.Add(rootNode.data);
        }
        private void InOrdTrav(Node rootNode)
        {
            if (rootNode == null)
                return;

            InOrdTrav(rootNode.left);
            tempList.Add(rootNode.data);
            InOrdTrav(rootNode.right);

        }
        private Node CheckRemove(T data, Node rootNode)
        {
            if (rootNode == null)
            {
                return rootNode;
            }
            else if (data.CompareTo(rootNode.data) < 0)
            {
                rootNode.left = CheckRemove(data, rootNode.left);
            }
            else if (data.CompareTo(rootNode.data) > 0)
            {
                rootNode.right = CheckRemove(data, rootNode.right);
            }
            else
            {
                if (rootNode.left == null) //Sol boş ise
                {
                    return rootNode.right;
                }
                else if (rootNode.right == null) //Sağ boş ise
                {
                    return rootNode.left;
                }
                else //Sağ ve sol düğümler doluysa, rootNode.right düğümünde en küçük değer aranır.
                {
                    //En küçük değer rootNode.data 'ya atanır.
                    rootNode.data = MinValueCheck(rootNode.right);
                    //Atama işlemi sonrası rootNode.right referansı rootNode.right.left veya rootNode.right.right ile değiştirilir.
                    rootNode.right = CheckRemove(rootNode.data, rootNode.right);
                }
            }
            //Kök, merkez değer geri döndürülür, işlemlerin rootNode.right veya roodNode.left üzerinden yapıldığına dikkat edin!
            return rootNode;
        }
        private T MinValueCheck(Node rootNode)
        {
            T minValue = rootNode.data;
            //Belirlenen düğümün sol kısmı taranır
            while (rootNode.left != null)
            {
                minValue = rootNode.left.data;
                rootNode = rootNode.left;
            }
            return minValue;
        }
        private void CheckAdd(T data,Node rootNode)
        {
            //Girilen değer merkez(root) değerden büyükse ifade sağ tarafa alınır
            if (data.CompareTo(rootNode.data)>0)
            {
                if (rootNode.right==null)
                {
                    rootNode.right=new Node(data);
                    return;
                }
                CheckAdd(data,rootNode.right);
            }
            else //Girilen ifade merkez,kök değerinden küçük veya eşitse 
            {
                if (rootNode.left==null)
                {
                    rootNode.left=new Node(data);
                    return;
                }
                CheckAdd(data,rootNode.left);
            }
        }
        private bool CheckContains(T data,Node node)
        {
            if (node == null)
                return false;
            if (data.CompareTo(node.data) == 0)
                return true;
            if (data.CompareTo(node.data) > 0)
                return CheckContains(data, node.right);

            return CheckContains(data, node.left);
        }
        #region PrintMethods

        private static class TreePrinter
        {
            private class NodeInfo
            {
                public Node node;
                public string Text;
                public int StartPos;
                public int Size => Text.Length;

                public int EndPos
                {
                    get => StartPos + Size;
                    set => StartPos = value - Size;
                }

                public NodeInfo Parent, Left, Right;
            }

            public static void Print(Node rootNode, string textFormat = "0", int spacing = 1, int topMargin = 2,
                int leftMargin = 2)
            {
                if (rootNode == null) return;
                var rootNodeTop = Console.CursorTop + topMargin;
                var last = new List<NodeInfo>();
                var next = rootNode;
                for (var level = 0; next != null; level++)
                {
                    var item = new NodeInfo { node = next, Text = next.data.ToString() };
                    if (level < last.Count)
                    {
                        item.StartPos = last[level].EndPos + spacing;
                        last[level] = item;
                    }
                    else
                    {
                        item.StartPos = leftMargin;
                        last.Add(item);
                    }

                    if (level > 0)
                    {
                        item.Parent = last[level - 1];
                        if (next == item.Parent.node.left)
                        {
                            item.Parent.Left = item;
                            item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                        }
                        else
                        {
                            item.Parent.Right = item;
                            item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                        }
                    }

                    next = next.left ?? next.right;
                    for (; next == null; item = item.Parent)
                    {
                        var top = rootNodeTop + 2 * level;
                        Print(item.Text, top, item.StartPos);
                        if (item.Left != null)
                        {
                            Print("/", top + 1, item.Left.EndPos);
                            Print("_", top, item.Left.EndPos + 1, item.StartPos);
                        }

                        if (item.Right != null)
                        {
                            Print("_", top, item.EndPos, item.Right.StartPos - 1);
                            Print("\\", top + 1, item.Right.StartPos - 1);
                        }

                        if (--level < 0) break;
                        if (item == item.Parent.Left)
                        {
                            item.Parent.StartPos = item.EndPos + 1;
                            next = item.Parent.node.right;
                        }
                        else
                        {
                            if (item.Parent.Left == null)
                                item.Parent.EndPos = item.StartPos - 1;
                            else
                                item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                        }
                    }
                }

                Console.SetCursorPosition(0, rootNodeTop + 2 * last.Count - 1);
            }

            private static void Print(string s, int top, int left, int right = -1)
            {
                Console.SetCursorPosition(left, top);
                if (right < 0) right = left + s.Length;
                while (Console.CursorLeft < right) Console.Write(s);
            }
        }

        public virtual void Print()
        {
            TreePrinter.Print(root);
        }

        #endregion

    }
}
