using List;
using System;
using System.Collections.Generic;


/* Student Version */
namespace BinarySearchTree
{
    /// <summary>
    /// A Binary Search Tree where the data can be compared
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinSearchTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// A priave node class to hide the internal implementation details
        /// </summary>
        private class Node  // shown in class
        {
            /// <summary>
            /// The data for each node
            /// </summary>
            public T Data { get; set; }
            /// <summary>
            /// Pointer to the left child which must be < parent
            /// </summary>
            public Node Left;
            /// <summary>
            /// Pointer to the right child which must be < parent
            /// </summary>
            public Node Right;
            public Node(T d = default, Node leftnode = null, Node rightnode = null)
            {
                Data = d;
                Left = leftnode;
                Right = rightnode;
            }

            /// <summary>
            /// Needed to print the data
            /// </summary>
            /// <returns>a string representing the data</returns>
            public override string ToString()
            {
                return Data.ToString();
            }
        }
        
        /// <summary>
        /// Points to the root or null if tree is empty
        /// </summary>
        private Node root;

        public BinSearchTree()
        {
            root = null;
        }
        public void Clear()
        {
            root = null;
        }

        /// <summary>
        /// Returns the min data in the tree. The algorithm is non-recursive
        /// </summary>
        /// <returns></returns>
        public T FindMinNR()
        {
            if (root == null)
                throw new ApplicationException("Can find min on empty tree");
            else
            {
                Node pTmp = root;
                while (pTmp.Left != null)  // keep going left until there is no left child
                    pTmp = pTmp.Left;
                return pTmp.Data;
            }
        }
        /// <summary>
        /// Returns the min data in the tree but uses recursion.
        /// </summary>
        /// <returns></returns>
        public T FindMin()
        {
            if (root == null)
                throw new ApplicationException("FindMin called on empty BinSearchTree");
            else
                return FindMin(root);
        }
        private T FindMin(Node pTmp)
        {
            // if no left child, return the min data in the tree up the recusion chain
            if (pTmp.Left == null)
                return pTmp.Data;
            else
                return FindMin(pTmp.Left);  // this node has a left child, so "go left"
        }

        /// <summary>
        /// public find method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Find(T value)
        {
            return Find(value, root);
        }
        /// <summary>
        /// private, recursive find method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pTmp"></param>
        /// <returns>returns the data</returns>
        private T Find(T value, Node pTmp)
        {
            if (pTmp == null)
                throw new ApplicationException("BinSearchTree could not find " + value);   // Item not found
            else if (value.CompareTo(pTmp.Data) < 0) // value < pTmp.Data
                return Find(value, pTmp.Left);		// search left subtree
            else if (value.CompareTo(pTmp.Data) > 0) // value > pTmp.Data
                return Find(value, pTmp.Right); 	// search right subtree
            else
                return pTmp.Data;    			// Found it
        }

        /// <summary>
        /// Non-recursive find that does not throw
        /// </summary>
        /// <param name="value"></param>
        /// <returns>false is value not found, true and value stored in the tree</returns>
        public bool TryFind(ref T value)
        {
            Node pTmp = root;
            int result;
            while (pTmp != null)
            {
                result = value.CompareTo(pTmp.Data);
                if (result == 0)
                {           // found it
                    value = pTmp.Data;
                    return true;
                }
                else if (result < 0)        // value < pTmp.Data, search left subtree
                    pTmp = pTmp.Left;
                else if (result > 0)        // value > pTmp.Data, search right subtree
                    pTmp = pTmp.Right;
            }
            return false;       // didn't find it
        }
        /// <summary>
        /// Insert an item 
        /// </summary>
        /// <param name="newItem"></param>
        public void Insert(T newItem)
        {
            root = Insert(newItem, root);
        }
        /// <summary>
        /// Recursive version
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="pTmp"></param>
        /// <returns></returns>
        /// <remarks>Goes down tree until it falls off
        ///          Then, it creates a new node and passes back to its caller 
        ///          The caller then attaches it</remarks>
        private Node Insert(T newItem, Node pTmp)
        {
            if (pTmp == null)
                return new Node(newItem, null, null);
            else if (newItem.CompareTo(pTmp.Data) < 0) // newItem < pTmp.Data
                pTmp.Left = Insert(newItem, pTmp.Left);
            else if (newItem.CompareTo(pTmp.Data) > 0) // newItem > pTmp.Data 
                pTmp.Right = Insert(newItem, pTmp.Right);
            else   // Duplicate
                throw new ApplicationException("Tree did not insert " + newItem + " since an item with that value is already in the tree");

            return pTmp;
        }
        /// <summary>
        /// Removes a value from the tree
        /// </summary>
        /// <param name="value"></param>
        public void Remove(T value)
        {
            Remove(value, ref root);
        }
        /// <summary>
        /// Recursive remove method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pTmp"></param>
        private void Remove(T value, ref Node pTmp)
        {
            if (pTmp == null)
                throw new ApplicationException("BinSearchTree could not remove " + value);   // Item not found
            else if (value.CompareTo(pTmp.Data) < 0) // value < pTmp.Data
                Remove(value, ref pTmp.Left);
            else if (value.CompareTo(pTmp.Data) > 0) // value > pTmp.Data
                Remove(value, ref pTmp.Right);
            else if (pTmp.Left != null && pTmp.Right != null) // Two children, replace with min in right subtree
            {
                pTmp.Data = FindMin(pTmp.Right); // Replace this node with min in right subtree
                Remove(pTmp.Data, ref pTmp.Right); // Remove old min in this node's right subtree
            }
            else
                // pTmp is the left child unless there is no left child, then it becomes the right child
                pTmp = (pTmp.Left != null) ? pTmp.Left : pTmp.Right;
               // if (pTmp.Left != null) pTmp = pTmp.Left else pTmp = pTmp.Right;
        }

        /// <summary>
        /// An insert that doesn't throw
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryInsert(T value)
        {
            if (root == null)  // empty tree, put new value at root
                root = new Node(value, null, null);
            else
            {
                Node pTmp = root, parent;
                while (pTmp != null) // keep going down until you fall off the tree
                {
                    parent = pTmp; // this is a trailing pointer (e.g. drop an anchor)
                    if (value.CompareTo(pTmp.Data) == 0)        // trying to insert duplicate key
                        return false;
                    else if (value.CompareTo(pTmp.Data) < 0)    // value < pTmp.Data, search left subtree
                    {
                        pTmp = pTmp.Left;
                        if (pTmp == null)  // fell off the tree
                            parent.Left = new Node(value, null, null); // use trailing pointer to attach new node
                    }
                    else                                        // value > pTmp.Data, search right subtree
                    {
                        pTmp = pTmp.Right;
                        if (pTmp == null) // fell off the tree
                            parent.Right = new Node(value, null, null); // use trailing pointer to attach new node
                    }
                }
            }
            return true;
        }

        public bool Contains(T value)
        {
            throw new NotImplementedException("BST Contains not implemented");
        }

        public T FindMax()
        {
            return FindMax(root).Data;
        }
        private Node FindMax(Node pTmp)
        {
            if (pTmp == null)
                throw new ApplicationException("FindMax called on empty BinSearchTree");
            if (pTmp.Right == null)
                return pTmp;
            return FindMax(pTmp.Right);
        }

        public int Count()
        {
            if(root == null)
            {
                return 0;
            }
            else
            {
                return Count(root);
            }
        }

        private int Count(Node pTmp) 
        { 
            
            if (pTmp.Left != null && pTmp.Right != null)
            {
                return 1 + Count(pTmp.Left) + Count(pTmp.Right);
            }
            else if (pTmp.Right != null && pTmp.Left == null)
            {
                return 1 + Count(pTmp.Right);
            }
            else if (pTmp.Left != null && pTmp.Right == null)
            {
                return 1 + Count(pTmp.Left);
            }
            else
            {
                return 1;
            }

        }

        public List<T> ToList()
        {
            List<T> aList = new List<T>();
            ToList(root, ref aList);
            return aList;
        }

        private void ToList(Node ptmp, ref List<T> aList)
        {
            if (ptmp == null)
            {
                return;
            }

            aList.Add(ptmp.Data);
            ToList(ptmp.Left, ref aList);
            ToList(ptmp.Right, ref aList);    
        }
        
        public int Height()
        {
            if (root == null)
            {
                return - 1;
            }

            return Height(root);
        }

        private int Height(Node ptmp)
        {
            if (ptmp.Left == null && ptmp.Right == null)
            {
                return 0;
            }

            return Math.Max(Height(ptmp.Left), Height(ptmp.Right)) + 1;
            
        }

        private void MyRemove(T value, ref Node ptmp)
        {
            if (ptmp == null)
            {
                //Throw
            }
            else if(value.CompareTo(ptmp.Data) < 0)
            {
                MyRemove(value, ref ptmp.Left);
            }
            else if (value.CompareTo(ptmp.Data) > 0)
            {
                MyRemove(value, ref ptmp.Right);
            }
            else if(ptmp.Left != null & ptmp.Right != null)
            {
                ptmp.Data = FindMin(ptmp.Right);
                MyRemove(ptmp.Data, ref ptmp.Right);
            }
            else
            {
                ptmp = (ptmp.Left != null) ? ptmp.Left : ptmp.Right;
            }
        }

        public int CountNodes()
        {
            if(root == null)
            {
                return 0;
            }

            return Count(root);
        }

        private int CountNodes(ref Node ptmp)
        {
            if(ptmp != null)
            {
                return 1 + Count(ptmp.Left) + Count(ptmp.Right);
            }
            return 0;
        }

        public void PreOrder()
        {
            PreOrder(root);
        }

        private void PreOrder(Node ptmp)
        {
            if (ptmp == null)
                return;

            Console.WriteLine(ptmp.Data + " ");
            PreOrder(ptmp.Left);
            PreOrder(ptmp.Right);
        }

        public void PostOrder()
        {
            PostOrder(root);
        }

        private void PostOrder(Node ptmp)
        {
            if (root == null)
                return;

            PostOrder(ptmp.Left);
            PostOrder(ptmp.Right);
            Console.WriteLine(ptmp.Data + " ");
        }

        public void InOrder()
        {
            InOrder(root);
        }

        private void InOrder(Node ptmp)
        {
            if (ptmp == null)
                return;

            InOrder(ptmp.Left);
            Console.WriteLine(ptmp.Data + " ");
            InOrder(ptmp.Right);
        }


    }
}
