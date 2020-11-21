using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TreeAndHeaps
{
    // Rotation, Ağaçlarda döndürme işlemi dengesiz olan ağac formunu düzenlemek amacıyla yapılır
    // Herhangi bir diziyi ağac dizi türüne dönüştürmek için de bu yol izlenir.

    //Yöntem;                                                  1  "Grandparent"
    //set temp=grandparent's right child                         2  "Parent"
    //set grandparent's right child = temp left child              3 "Child"
    //set temp left child=grandparent                        2
    //use temp instead of grandparent     Sola Döndürme=>  1   3   "Son Hali"

    //AVL- Adelson-Velsky and Landis Tree, kendi kendini dengeleyen agaçlardır
    //Algoritmanın ismi keşfi yapan kişilerin isimlerini temsilen AVL olarak isimlendirilmiştir

    //Yöntem ve Kurallar;
    //Sağ ve Sol bağlantılar(node) arası yükseklik {-1,0,1} olabilir!
    //Bu durumu ihlal eden bir veri sisteme girerse: RightRotation,LeftRotation,RightLeftRotation,LeftRightRotation işlemleriyle denge yeniden sağlanmaya çalışılır
    class MyAVLTree<T,V> : TreeRotations<T,V> where T : IEquatable<T>, IComparable<T>
    {
        private int size;
        private Node tempNode;

        public MyAVLTree()
        {
            root = null;
            tempNode = null;
            size = 0;
        }
        //private class Node
        //{
        //    public T data;
        //    public Node left;
        //    public Node right;
        //    public Node parent;

        //    public Node(T element)
        //    {
        //        this.data = element;
        //        left = right = parent = null;
        //    }
        //}
        public int Size() => size;
        public void Add(T element,V value)
        {
             Node node = new Node(element,value);
             if (root==null)
             {
                 root = node;
                 size++;
                 return;
             }

             AddCheckNodes(root, node);
        }
        public V GetValueAt(T key)
        {
            if (ContainCheck(key, root))
            {
                if (tempNode != null)
                {
                    V nodeValue = tempNode.value;
                    tempNode = null;
                    return nodeValue;
                }
            }

            return default;
        }
        public T Remove(T key)
        {
            if (ContainCheck(key,root))
            {
                root=CheckRemove(key, root); // Silme işlemi BinaryTree ve AVLTree kurallarınca kaldırılır.
            }

            return key;
        }
        public T MaxKey()
        {
            Node traverseNode = root;
            while (traverseNode.right!=null)
            {
                traverseNode = traverseNode.right;
            }

            return traverseNode.data;
        }
        public T MinKey()
        {
            Node traverseNode = root;
            while (traverseNode.left != null)
            {
                traverseNode = traverseNode.left;
            }

            return traverseNode.data;
        }
        private Node CheckRemove(T key, Node rootNode)
        {
            Node testNode;
            if (rootNode == null)
            {
                return rootNode;
            }
            else if (key.CompareTo(rootNode.data) < 0)
            {
                rootNode.left = CheckRemove(key, rootNode.left);
                testNode = rootNode.left?.parent;
                testNode = rootNode;
            }
            else if (key.CompareTo(rootNode.data) > 0)
            {
                rootNode.right = CheckRemove(key, rootNode.right);
                testNode = rootNode.right?.parent;
                testNode = rootNode;
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
                    rootNode.data = MaxValueCheck(rootNode.left);
                    //Atama işlemi sonrası rootNode.right referansı rootNode.right.left veya rootNode.right.right ile değiştirilir.
                    rootNode.left = CheckRemove(rootNode.data, rootNode.left);
                }
            }
            //Kök, merkez değer geri döndürülür, işlemlerin rootNode.right veya roodNode.left üzerinden yapıldığına dikkat edin!
            return rootNode;
        }
        private T MaxValueCheck(Node rootNode)
        {

            T minValue = rootNode.data;
            //Belirlenen düğümün sağ kısmı taranır
            while (rootNode.right != null)
            {
                minValue = rootNode.right.data;
                rootNode = rootNode.right;
            }
            return minValue;
        }
        private bool ContainCheck(T element, Node node)
        {
            if (node == null)
            {
                tempNode = node;
                return false;
            }

            if (element.CompareTo(node.data) == 0)
            {
                tempNode = node;
                return true;
            }

            if (element.CompareTo(node.data) > 0)
                return ContainCheck(element, node.right);

            return ContainCheck(element, node.left);
        }
        private void AddCheckNodes(Node rootParent, Node newNode)
        {
          
            if (newNode.data.CompareTo(rootParent.data)>0)
            {
                if (rootParent.right==null)
                {
                    rootParent.right = newNode;
                    newNode.parent = rootParent;
                    newNode.IsLeftChild = false;
                    size++;
                    return;
                }
                 AddCheckNodes(rootParent.right,newNode);
            }
            else
            {
                if (rootParent.left == null)
                {
                    rootParent.left = newNode;
                    newNode.parent = rootParent;
                    newNode.IsLeftChild = true;
                    size++;
                    return;
                }

                AddCheckNodes(rootParent.left, newNode);
            }
            CheckBalance(newNode);
        }
        private void CheckBalance(Node newNode)
        {
            if (Math.Abs(Height(newNode.left)-Height(newNode.right))>1)
            {
                Rebalance(newNode);
                return;
            }

            if (newNode.parent==null)
            {
                return;
            }
            CheckBalance(newNode.parent);
        }
        private void Rebalance(Node newNode)
        {
            if ((Height(newNode.left)-Height(newNode.right))>1)
            {
                if (Height(newNode.left.left)>Height(newNode.left.right))
                {
                   newNode= RightRotation(newNode);
                }
                else
                {
                    newNode=LeftRightRotation(newNode);
                }
            }
            else
            {
                if (Height(newNode.right.left) > Height(newNode.right.right))
                {
                    newNode=LeftRotation(newNode);
                }
                else
                {
                    newNode=RightLeftRotation(newNode);
                }
            }

            if (newNode.parent==null)
            {
                root = newNode;
            }

        }
        private int Height(Node node)
        {
            int leftHeight = 0, rightHeight = 0;
            if (node == null)
            {
                return 0;
            }

            if (node != null)
            {
                leftHeight = Height(node.left) + 1;
                rightHeight = Height(node.right) + 1;
            }

            if (leftHeight > rightHeight)
            {
                return leftHeight;
            }

            return rightHeight;

        }
        #region PrintMethod
        private static class TreePrinter
        {
            class NodeInfo
            {
                public Node node;
                public string Text;
                public int StartPos;
                public int Size { get { return Text.Length; } }
                public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
                public NodeInfo Parent, Left, Right;
            }

            public static void Print(Node root, string textFormat = "0", int spacing = 2, int topMargin = 5, int leftMargin = 5)
            {
                if (root == null) return;
                int rootTop = Console.CursorTop + topMargin;
                var last = new List<NodeInfo>();
                var next = root;
                for (int level = 0; next != null; level++)
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
                        int top = rootTop + 2 * level;
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
                Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
            }

            private static void Print(string s, int top, int left, int right = -1)
            {
                Console.SetCursorPosition(left, top);
                if (right < 0) right = left + s.Length;
                while (Console.CursorLeft < right) Console.Write(s);
            }

        }
        public void Print()
        {
            TreePrinter.Print(root);
        }
        #endregion
    }
}
                                                