using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeAndHeaps
{
    class TreeRotations<K,V>
    {
        //Döndürme işlemleri belli durumların ihlali halinde uygulanır
        //Left-Left durumunda uyumsuzluk varsa, RightRotation uygulanır
        //Right-Right durumunda ise, LeftRotation
        //Right-Left ise , RightLeftRotation
        //Left-Right ise, LeftRightRotation
        protected Node root;
        protected class Node
        {
            public K data;
            public V value;
            public Node left;
            public Node right;
            public Node parent;
            public bool IsLeftChild;

            public Node(K element,V value)
            {
                this.value = value;
                this.data = element;
                left = right = parent = null;
                this.IsLeftChild = false;
            }
        }
        protected Node LeftRotation(Node grandParent)
        {
            if (grandParent.right!=null) // Null kontrolu sonrası işleme başlanır
            {
                Node temp = grandParent.right;      //       o   grandParent
                grandParent.right = temp.left;      //        \
                                                    //         o   temp
                if (grandParent.right != null)      //        / \
                {                                   //       o   o         
                    grandParent.right.parent = grandParent;
                    grandParent.right.IsLeftChild = false;
                }

                if (grandParent.parent == null)
                {
                    root = temp;
                    temp.parent = null;
                }
                else
                {
                    temp.parent = grandParent.parent;
                    if (grandParent.IsLeftChild)
                    {
                        temp.IsLeftChild = true;
                        temp.parent.left = temp;
                    }
                    else
                    {
                        temp.IsLeftChild = false;
                        temp.parent.right = temp;
                    }
                }

                temp.left = grandParent;
                grandParent.IsLeftChild = true;
                grandParent.parent = temp;
                return temp; 
            }

            return grandParent;
        }
        protected Node RightRotation(Node grandParent)
        {

            if (grandParent.left!=null)
            {
                Node temp = grandParent.left;               
                grandParent.left = temp.right;

                if (grandParent.left != null)
                {
                    grandParent.left.parent = grandParent;      //        o   grandParent
                    grandParent.left.IsLeftChild = true;        //       /
                }                                               //      o   temp
                                                                //     / \
                if (grandParent.parent == null)                 //    o   o         
                {
                    root = temp;
                    temp.parent = null;
                }
                else
                {
                    temp.parent = grandParent.parent;
                    if (grandParent.IsLeftChild)
                    {
                        temp.IsLeftChild = true;
                        temp.parent.left = temp;
                    }
                    else
                    {
                        temp.IsLeftChild = false;
                        temp.parent.right = temp;
                    }
                }

                temp.right = grandParent;
                grandParent.IsLeftChild = false;
                grandParent.parent = temp;
                return temp; 
            }

            return grandParent;
        }
        protected Node RightLeftRotation(Node node)
        {
            node=RightRotation(node.right);
            LeftRotation(node.parent);
            return node;
        }
        protected Node LeftRightRotation(Node node)
        {
            node=LeftRotation(node.left);
            RightRotation(node.parent);
            return node;
        }
    }
}
