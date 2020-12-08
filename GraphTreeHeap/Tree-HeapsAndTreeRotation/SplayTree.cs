using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TreeAndHeaps
{
    // Kendi kendini dengeleyen Binary Search Tree Modeli
    //Roughly Balanced Tree , Kabaca Dengelenmiş Ağaç türüdür

    //Yayılma ağacı, yakın zamanda erişilen öğelere yeniden hızlı erişmemizi sağlayan ağaç türüdür
    //O(logn) zaman karmaşıklığına sahiptir, Search,Insert,Update,Delete
    //Her bir işlem sonrası yayılım (Splay) gerçekleştirilir


    class SplayTree<K,V>:TreeRotations<K,V> where K:IEquatable<K>,IComparable<K>
    {
        protected Node TempNode { get; private set; }
        public int size { get;private set; }
        public void Add(K key,V value)
        {
            if (root==null)
            {
                root = new Node(key,value);
            }
            else
            {
                AddCheck(key,value,root);
            }

            size++;
        }
        public K Remove(K key)
        {
            if (ContainCheck(key, root))
            {
                root = CheckRemove(key, root); // Silme işlemi BinaryTree ve SplayTree kurallarınca kaldırılır.
            }

            return key;
        }
        public V GetValueAt(K key)
        {
            if (ContainCheck(key, root))
            {
                if (TempNode != null)
                {
                    V nodeValue = TempNode.value;
                    TempNode = null;
                    return nodeValue;
                }
            }

            return default;
        }
        public K MaxKey()
        {
            Node traverseNode = root;
            while (traverseNode.right != null)
            {
                traverseNode = traverseNode.right;
            }

            return traverseNode.data;
        }
        public K MinKey()
        {
            Node traverseNode = root;
            while (traverseNode.left != null)
            {
                traverseNode = traverseNode.left;
            }

            return traverseNode.data;
        }
        private void AddCheck(K key,V value,Node rootNode)
        {
            if (key.CompareTo(rootNode.data)>0)
            {
                if (rootNode.right==null)
                {
                    rootNode.right=new Node(key,value);
                    rootNode.right.IsLeftChild = false;
                    rootNode.right.parent = rootNode;
                    TreeSplayer(rootNode.right);
                }
                else
                {
                    AddCheck(key,value,rootNode.right);
                }
            }
            else
            {
                if (rootNode.left==null)
                {
                    rootNode.left=new Node(key,value);
                    rootNode.left.IsLeftChild = true;
                    rootNode.left.parent = rootNode;
                    TreeSplayer(rootNode.left);
                }
                else
                {
                    AddCheck(key,value,rootNode.left);
                }
            }

            
        }
        private void TreeSplayer(Node rootNode)
        {
            if (rootNode.parent!=null)
            {
                bool checkLeftChild = rootNode.parent == root ? true : rootNode.parent.IsLeftChild;
                bool checkRightChild = rootNode.parent == root ? true : !rootNode.parent.IsLeftChild;

                if (rootNode.IsLeftChild && checkLeftChild)
                {
                    while (rootNode != root)
                    {
                        if (rootNode.parent.right!=null)
                        {
                            RightRotation(rootNode.parent);
                        }
                        else
                        {
                            RightRotation(rootNode.parent.parent == null ? rootNode.parent :rootNode.parent.parent);
                        }
                        TreeSplayer(rootNode);
                    }
                }
                else if (rootNode.IsLeftChild && !rootNode.parent.IsLeftChild)
                {
                    while (rootNode != root)
                    {
                        rootNode = RightRotation(rootNode.parent);
                        LeftRotation(rootNode.parent);
                        TreeSplayer(rootNode);
                    }
                }
                else if (!rootNode.IsLeftChild && checkRightChild)
                {
                    while (rootNode != root)
                    {
                        if (rootNode.parent.left != null)
                        {
                            LeftRotation(rootNode.parent);
                        }
                        else
                        {
                            LeftRotation(rootNode.parent.parent == null ? rootNode.parent : rootNode.parent.parent);
                        }
                        TreeSplayer(rootNode);
                    }
                }
                else
                {
                    while (rootNode != root)
                    {
                        rootNode = LeftRotation(rootNode.parent);
                        RightRotation(rootNode.parent);
                        TreeSplayer(rootNode);
                    }
                }
            }
        }
        private bool ContainCheck(K element, Node node)
        {
            if (node == null)
            {
                TempNode = node;
                return false;
            }

            if (element.CompareTo(node.data) == 0)
            {
                TempNode = node;
                return true;
            }

            if (element.CompareTo(node.data) > 0)
                return ContainCheck(element, node.right);

            return ContainCheck(element, node.left);
        }
        private Node CheckRemove(K key, Node rootNode)
        {
            if (rootNode == null)
            {
                return rootNode;
            }
            else if (key.CompareTo(rootNode.data) < 0)
            {
                rootNode.left = CheckRemove(key, rootNode.left);
            }
            else if (key.CompareTo(rootNode.data) > 0)
            {
                rootNode.right = CheckRemove(key, rootNode.right);
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
        private K MinValueCheck(Node rootNode)
        {

            K minValue = rootNode.data;
            //Belirlenen düğümün sol kısmı taranır
            while (rootNode.left != null)
            {
                minValue = rootNode.left.data;
                rootNode = rootNode.left;
            }
            return minValue;
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
