using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using CSharp.DataStructures.BPlusTreeSpace;

namespace TreeAndHeaps
{
    class MyBPlusTree<K,V> where K:IEquatable<K>,IComparable<K>
    {
        private int minKeySize;
        private int minChildrenSize;
        private int maxKeySize;
        private int maxChildrenSize;

        private Node root;
        private int size;

        public MyBPlusTree(int keySize)
        {
            this.minKeySize = keySize;
            this.minChildrenSize = minKeySize + 1;
            this.maxKeySize = 2 * minKeySize;
            this.maxChildrenSize = maxKeySize + 1;
            this.root = null;
            this.size = 0;
        }

        private class Node:IComparer<Node>
        {
            public K[] keys { get; private set; }
            public SortedList<K,V> values { get; private set; }

            public  int keySize;
            public  Node[] children;
            public  int childrenSize;
            public  Node parent;

            public Node(Node newParent, int maxKeySize, int maxChildrenSize)
            {
                parent = newParent;
                keys = new K[maxKeySize+1];
                keySize = 0;
                children = new Node[maxChildrenSize + 1];
                values=new SortedList<K,V>(maxChildrenSize + 1);
                childrenSize = 0;
                parent = null;
            }

            public K GetKey(int indeks) => keys[indeks];

            public V GetValue(K key)
            {
                V value = default;
                values.TryGetValue(key, out value);
                return value;
            }
            
            public  int IndexOf(K keyValue)
            {
                for (int i = 0; i < keySize; i++)
                {
                    if (keys[i].CompareTo(keyValue)==0) 
                        return i;
                }
                return -1;
            }

            public  void AddKeyValue(K keyValue,V value)
            {
                values.Add(keyValue, value);
                keys[keySize++] = keyValue;
                Array.Sort(keys,0,keySize-1);
            }
            public  K RemoveKey(K keyValue)
            {
                K uninstalled = default;
                bool exist = false;
                if (keySize == 0) 
                    return default;

                for (int i = 0; i < keySize; i++)
                {
                    if (keys[i].CompareTo(keyValue)==0)
                    {
                        exist = true;
                        uninstalled = keys[i];
                    }
                    else if (exist)
                    {
                       // Anahtarları aşağıya doğru kaydırırı, yapraklara doğru
                        keys[i - 1] = keys[i];
                    }
                }
                if (exist)
                {
                    keySize--;
                    keys[keySize] = default;
                }
                return uninstalled;
            }
            public bool RemoveVlaue(K key)
            {
                return values.Remove(key);
            }
            public  K RemoveKey(int index)
            {
                if (index >= keySize)
                    return default;
                K value = keys[index];
                for (int i = index + 1; i < keySize; i++)
                {
                    // Kalan anahtar, indeksleri aşağıya kaydırır
                    keys[i - 1] = keys[i];
                }
                keySize--;
                keys[keySize] = default;
                return value;
            }

            public  int NumberOfKeys() => keySize;

            public Node GetChild(int index)
            {
                if (index >= childrenSize)
                    return null;
                return children[index];
            }

            public int IndexOf(Node child)
            {
                for (int i = 0; i < childrenSize; i++)
                {
                    if (children[i].Equals(child))
                        return i;
                }
                return -1;
            }

            public bool AddChild(Node child)
            {
                child.parent = this;
                children[childrenSize++] = child;
                Array.Sort(children, childrenSize-1,Compare(children[0],children[0]));
                return true;
            }

            public bool RemoveChild(Node child)
            {
                bool found = false;
                if (childrenSize == 0)
                    return found;
                for (int i = 0; i < childrenSize; i++)
                {
                    if (children[i].Equals(child))
                    {
                        found = true;
                    }
                    else if (found)
                    {
                        //Anahtarları aşağıya kaydırır
                        children[i - 1] = children[i];
                    }
                }
                if (found)
                {
                    childrenSize--;
                    children[childrenSize] = null;
                }
                return found;
            }

            public Node RemoveChild(int index)
            {
                if (index >= childrenSize)
                    return null;
                Node value = children[index];
                children[index] = null;
                for (int i = index + 1; i < childrenSize; i++)
                {                   
                    children[i - 1] = children[i];
                }
                childrenSize--;
                children[childrenSize] = null;
                return value;
            }
            public int NumberOfChildren()
            {
                return childrenSize;
            }

            public int Compare(Node x, Node y)
            {
                return x.GetKey(0).CompareTo(y.GetKey(0));
            }

            public override String ToString()
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("keys=[");
                for (int i = 0; i < NumberOfKeys(); i++)
                {
                    K value = GetKey(i);
                    builder.Append(value);
                    if (i < NumberOfKeys() - 1)
                        builder.Append(", ");
                }
                builder.Append("]\n");

                if (parent != null)
                {
                    builder.Append("parent=[");
                    for (int i = 0; i < parent.NumberOfKeys(); i++)
                    {
                        K value = parent.GetKey(i);
                        builder.Append(value);
                        if (i < parent.NumberOfKeys() - 1)
                            builder.Append(", ");
                    }
                    builder.Append("]\n");
                }

                if (children != null)
                {
                    builder.Append("keySize=").Append(NumberOfKeys()).Append(" children=").Append(NumberOfChildren()).Append("\n");
                }

                return builder.ToString();
            }
        }

        public bool Add(K key,V value)
        {
            if (root == null)
            {
                root = new Node(null, maxKeySize, maxChildrenSize);
                root.AddKeyValue(key,value);
            }
            else
            {
                Node node = root;
                while (node != null)
                {
                    if (node.NumberOfChildren() == 0)
                    {
                        node.AddKeyValue(key,value);
                        if (node.NumberOfKeys() <= maxKeySize)
                        {                            
                            break;
                        }
                       // Ayırma işlemi uygulanır
                        Split(node);
                        break;
                    }
                                      
                    K lesser = node.GetKey(0);
                    if (key.CompareTo(lesser) <= 0)
                    {
                        node = node.GetChild(0);
                        continue;
                    }
          
                    int numberOfKeys = node.NumberOfKeys();
                    int last = numberOfKeys - 1;
                    K greater = node.GetKey(last);
                    if (key.CompareTo(greater) > 0)
                    {
                        node = node.GetChild(numberOfKeys);
                        continue;
                    }

                    //İç düğümlerin taranması
                    for (int i = 1; i < node.NumberOfKeys(); i++)
                    {
                        K prev = node.GetKey(i - 1);
                        K next = node.GetKey(i);
                        if (key.CompareTo(prev) > 0 && key.CompareTo(next) <= 0)
                        {
                            node = node.GetChild(i);
                            break;
                        }
                    }
                }
            }

            size++;

            return true;
        }

        public K Remove(K key)
        {
            K removed;
            Node node = GetNode(key);
            removed = Remove(key, node);
            return removed;
        }
        public void Clear()
        {
            root = null;
            size = 0;
        }

        public V GetValueAtKey(K key)
        {
            if(GetNode(key)!=null)
                return GetNode(key).GetValue(key);

            return default;
        }
        private K Remove(K key, Node node)
        {
            if (node == null) 
                return default;

            K removed;
            int index = node.IndexOf(key);
            removed = node.RemoveKey(key);
            if (node.NumberOfChildren() == 0)
            {
                // leaf node
                if (node.parent != null && node.NumberOfKeys() < minKeySize)
                {
                    this.Combined(node);
                }
                else if (node.parent == null && node.NumberOfKeys() == 0)
                {                   
                    root = null;
                }
            }
            else
            {
                // İç düğümler
                Node lesser = node.GetChild(index);
                Node greatest = this.GetGreatestNode(lesser);
                K replacekey = this.RemoveGreatestValue(greatest);
                node.AddKeyValue(replacekey,default);
                if (greatest.parent != null && greatest.NumberOfKeys() < minKeySize)
                {
                    this.Combined(greatest);
                }
                if (greatest.NumberOfChildren() > maxChildrenSize)
                {
                    this.Split(greatest);
                }
            }

            size--;
            node.RemoveVlaue(key);
            return removed;
        }
        private K RemoveGreatestValue(Node node)
        {
            K value=default;
            if (node.NumberOfKeys() > 0)
            {
                value = node.RemoveKey(node.NumberOfKeys() - 1);
            }
            return value;
        }
        public bool Validate()
        {
            if (root == null) 
                return true;
            return ValidateNode(root);
        }
        public bool Contains(K key)
        {
            bool returnValue = false;
            Node node = GetNode(key);
            for (int i = 0; i < node.keySize; i++)
            {
                returnValue = node.keys[i].CompareTo(key) == 0;
                if (returnValue)
                    break;
            }

            return returnValue;
        }
        public int Size()
        {
            return size;
        }
        private Node GetNode(K key)
        {
            Node node = root;
            while (node != null)
            {
                K lesser = node.GetKey(0);
                if (key.CompareTo(lesser) < 0)
                {
                    if (node.NumberOfChildren() > 0)
                        node = node.GetChild(0);
                    else
                        node = null;
                        break;
                }

                int numberOfKeys = node.NumberOfKeys();
                int last = numberOfKeys - 1;
                K greater = node.GetKey(last);
                if (key.CompareTo(greater) > 0)
                {
                    if (node.NumberOfChildren() > numberOfKeys)
                        node = node.GetChild(numberOfKeys);
                    else
                        node = null;
                    break;
                }

                for (int i = 0; i < numberOfKeys; i++)
                {
                    K currentValue = node.GetKey(i);
                    if (currentValue.CompareTo(key) == 0)
                    {
                        return node;
                    }

                    int next = i + 1;
                    if (next <= last)
                    {
                        K nextValue = node.GetKey(next);
                        if (currentValue.CompareTo(key) < 0 && nextValue.CompareTo(key) > 0)
                        {
                            if (next < node.NumberOfChildren())
                            {
                                node = node.GetChild(next);
                                break;
                            }
                            return node;
                        }
                    }
                }
            }
            return node;
        }
        private void Split(Node nodeToSplit)
        {
            Node node = nodeToSplit;
            int numberOfKeys = node.NumberOfKeys();
            int medianIndex = numberOfKeys / 2;
            K medianValue = node.GetKey(medianIndex);

            Node left = new Node(null, maxKeySize, maxChildrenSize);
            for (int i = 0; i < medianIndex; i++)
            {
                left.AddKeyValue(node.GetKey(i),node.GetValue(node.GetKey(i)));
            }
            if (node.NumberOfChildren() > 0)
            {
                for (int j = 0; j <= medianIndex; j++)
                {
                    Node c = node.GetChild(j);
                    left.AddChild(c);
                }
            }

            Node right = new Node(null, maxKeySize, maxChildrenSize);
            for (int i = medianIndex + 1; i < numberOfKeys; i++)
            {
                right.AddKeyValue(node.GetKey(i),node.GetValue(node.GetKey(i)));
            }
            if (node.NumberOfChildren() > 0)
            {
                for (int j = medianIndex + 1; j < node.NumberOfChildren(); j++)
                {
                    Node c = node.GetChild(j);
                    right.AddChild(c);
                }
            }

            if (node.parent == null)
            {
                // new root, height of tree is increased
                Node newRoot = new Node(null, maxKeySize, maxChildrenSize);
                newRoot.AddKeyValue(medianValue,default);
                node.parent = newRoot;
                root = newRoot;
                node = root;
                node.AddChild(left);
                node.AddChild(right);
            }
            else
            {            
                Node parent = node.parent;
                parent.AddKeyValue(medianValue,default);
                parent.RemoveChild(node);
                parent.AddChild(left);
                parent.AddChild(right);

                if (parent.NumberOfKeys() > maxKeySize) 
                    Split(parent);
            }
        }
        private bool ValidateNode(Node node)
        {
            int keySize = node.NumberOfKeys();
            if (keySize > 1)
            {
                // Make sure the keys are sorted
                for (int i = 1; i < keySize; i++)
                {
                    K p = node.GetKey(i - 1);
                    K n = node.GetKey(i);
                    if (p.CompareTo(n) > 0)
                        return false;
                }
            }
            int childrenSize = node.NumberOfChildren();
            if (node.parent == null)
            {
                // root
                if (keySize > maxKeySize)
                {                   
                    return false;
                }
                else if (childrenSize == 0)
                {                    
                    return true;
                }
                else if (childrenSize < 2)
                {                   
                    return false;
                }
                else if (childrenSize > maxChildrenSize)
                {
                    return false;
                }
            }
            else
            {              
                if (keySize < minKeySize)
                {
                    return false;
                }
                else if (keySize > maxKeySize)
                {
                    return false;
                }
                else if (childrenSize == 0)
                {
                    return true;
                }
                else if (keySize != (childrenSize - 1))
                {                                    
                    return false;
                }
                else if (childrenSize < minChildrenSize)
                {
                    return false;
                }
                else if (childrenSize > maxChildrenSize)
                {
                    return false;
                }
            }

            Node first = node.GetChild(0);
            
            if (first.GetKey(first.NumberOfKeys() - 1).CompareTo(node.GetKey(0)) > 0)
                return false;

            Node last = node.GetChild(node.NumberOfChildren() - 1);
           
            if (last.GetKey(0).CompareTo(node.GetKey(node.NumberOfKeys() - 1)) < 0)
                return false;

            
            for (int i = 1; i < node.NumberOfKeys(); i++)
            {
                K p = node.GetKey(i - 1);
                K n = node.GetKey(i);
                Node c = node.GetChild(i);
                if (p.CompareTo(c.GetKey(0)) > 0)
                    return false;
                if (n.CompareTo(c.GetKey(c.NumberOfKeys() - 1)) < 0)
                    return false;
            }

            for (int i = 0; i < node.childrenSize; i++)
            {
                Node c = node.GetChild(i);
                bool valid = this.ValidateNode(c);
                if (!valid)
                    return false;
            }

            return true;
        }
        private int GetIndexOfPreviousValue(Node node, K key)
        {
            for (int i = 1; i < node.NumberOfKeys(); i++)
            {
                K t = node.GetKey(i);
                if (t.CompareTo(key) >= 0)
                    return i - 1;
            }
            return node.NumberOfKeys() - 1;
        }
        private int GetIndexOfNextValue(Node node, K key)
        {
            for (int i = 0; i < node.NumberOfKeys(); i++)
            {
                K t = node.GetKey(i);
                if (t.CompareTo(key) >= 0)
                    return i;
            }
            return node.NumberOfKeys() - 1;
        }
        private Node GetGreatestNode(Node nodeToGet)
        {
            Node node = nodeToGet;
            while (node.NumberOfChildren() > 0)
            {
                node = node.GetChild(node.NumberOfChildren() - 1);
            }
            return node;
        }
        private bool Combined(Node node)
        {
            Node parent = node.parent;
            int index = parent.IndexOf(node);
            int indexOfLeftNeighbor = index - 1;
            int indexOfRightNeighbor = index + 1;

            Node rightNeighbor =null ;
            int rightNeighborSize = -minChildrenSize;
            if (indexOfRightNeighbor < parent.NumberOfChildren())
            {
                rightNeighbor = parent.GetChild(indexOfRightNeighbor);
                rightNeighborSize = rightNeighbor.NumberOfKeys();
            }

            
            if (rightNeighbor != null && rightNeighborSize > minKeySize)
            {
                
                K removeValue = rightNeighbor.GetKey(0);
                int prev = GetIndexOfPreviousValue(parent, removeValue);
                K parentValue = parent.RemoveKey(prev);
                K neighborValue = rightNeighbor.RemoveKey(0);
                node.AddKeyValue(parentValue,default);
                parent.AddKeyValue(neighborValue,default);
                if (rightNeighbor.NumberOfChildren() > 0)
                {
                    node.AddChild(rightNeighbor.RemoveChild(0));
                }
            }
            else
            {
                Node leftNeighbor = null;
                int leftNeighborSize = -minChildrenSize;
                if (indexOfLeftNeighbor >= 0)
                {
                    leftNeighbor = parent.GetChild(indexOfLeftNeighbor);
                    leftNeighborSize = leftNeighbor.NumberOfKeys();
                }

                if (leftNeighbor != null && leftNeighborSize > minKeySize)
                {
                   
                    K removeValue = leftNeighbor.GetKey(leftNeighbor.NumberOfKeys() - 1);
                    int prev = GetIndexOfNextValue(parent, removeValue);
                    K parentValue = parent.RemoveKey(prev);
                    K neighborValue = leftNeighbor.RemoveKey(leftNeighbor.NumberOfKeys() - 1);
                    node.AddKeyValue(parentValue,default);
                    parent.AddKeyValue(neighborValue,default);
                    if (leftNeighbor.NumberOfChildren() > 0)
                    {
                        node.AddChild(leftNeighbor.RemoveChild(leftNeighbor.NumberOfChildren() - 1));
                    }
                }
                else if (rightNeighbor != null && parent.NumberOfKeys() > 0)
                {
                    // Can't borrow from neighbors, try to combined with right neighbor
                    K removeValue = rightNeighbor.GetKey(0);
                    int prev = GetIndexOfPreviousValue(parent, removeValue);
                    K parentValue = parent.RemoveKey(prev);
                    parent.RemoveChild(rightNeighbor);
                    node.AddKeyValue(parentValue,default);
                    for (int i = 0; i < rightNeighbor.keySize; i++)
                    {
                        K v = rightNeighbor.GetKey(i);
                        node.AddKeyValue(v,default);
                    }
                    for (int i = 0; i < rightNeighbor.childrenSize; i++)
                    {
                        Node c = rightNeighbor.GetChild(i);
                        node.AddChild(c);
                    }

                    if (parent.parent != null && parent.NumberOfKeys() < minKeySize)
                    {
                       
                        this.Combined(parent);
                    }
                    else if (parent.NumberOfKeys() == 0)
                    {             
                        node.parent = null;
                        root = node;
                    }
                }
                else if (leftNeighbor != null && parent.NumberOfKeys() > 0)
                {                  
                    K removeValue = leftNeighbor.GetKey(leftNeighbor.NumberOfKeys() - 1);
                    int prev = GetIndexOfNextValue(parent, removeValue);
                    K parentValue = parent.RemoveKey(prev);
                    parent.RemoveChild(leftNeighbor);
                    node.AddKeyValue(parentValue,default);
                    for (int i = 0; i < leftNeighbor.keySize; i++)
                    {
                        K v = leftNeighbor.GetKey(i);
                        node.AddKeyValue(v,default);
                    }
                    for (int i = 0; i < leftNeighbor.childrenSize; i++)
                    {
                        Node c = leftNeighbor.GetChild(i);
                        node.AddChild(c);
                    }

                    if (parent.parent != null && parent.NumberOfKeys() < minKeySize)
                    {                     
                        this.Combined(parent);
                    }
                    else if (parent.NumberOfKeys() == 0)
                    {                      
                        node.parent = null;
                        root = node;
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            return TreePrinter.GetString(this);
        }

        public static class TreePrinter
        {

            public static String GetString(MyBPlusTree<K,V> tree)
            {
                if (tree.root == null) return "Tree has no nodes.";
                return GetString(tree.root, "",true);
            }

            private static String GetString(Node node, String prefix, bool isTail)
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(prefix).Append((isTail ? "└── " : "├── "));
                for (int i = 0; i < node.NumberOfKeys(); i++)
                {
                    K value = node.GetKey(i);
                    builder.Append(value);
                    if (i < node.NumberOfKeys() - 1)
                        builder.Append(", ");
                }
                builder.Append("\n");

                if (node.children != null)
                {
                    for (int i = 0; i < node.NumberOfChildren() - 1; i++)
                    {
                        Node obj = node.GetChild(i);
                        builder.Append(GetString(obj, prefix + (isTail ? "    " : "│   "), false));
                    }
                    if (node.NumberOfChildren() >= 1)
                    {
                        Node obj = node.GetChild(node.NumberOfChildren() - 1);
                        builder.Append(GetString(obj, prefix + (isTail ? "    " : "│   "), true));
                    }
                }

                return builder.ToString();
            }
        }


    }
}
