using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;

namespace List
{
    /// <summary>
    /// A linked list version of a list class
    /// </summary>
    /// <remarks>
    /// Basic class that students use to add to.</remarks>
    /// <example>
    /// OurList<string> aList = new OurList<string>();
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class OurList<T> : ICollection<T>, IEnumerable<T>
    {
        /// <summary>
        /// Represents a node in a linked list
        /// </summary>
        /// <remarks>This node cannot be used outside of the List class. The user of the class passes data into the list but not nodes.</remarks>
        private class Node
        {
            /// <summary>  Data held by the node  </summary>
            public T Data { get; set; }
            /// <summary>
            /// Points to next node in the list or 
            /// null if it is last node in the list
            /// </summary>
            public Node Next { get; set; } 
            public Node(T d = default(T), Node node = null)
            {
                Data = d;
                Next = node;
            }
        }
        /// <summary>
        /// Points to the first node or null if the list is empty
        /// </summary>
        private Node first;

        /// <summary>
        /// Constructor that creates an empty list
        /// </summary>
        public OurList()     // shown in class
        {
            first = null;
        }

        /// <summary>
        /// Make a list empty (e.g. clear the contents)
        /// </summary>
        public void Clear()     // shown in class
        {
            first = null;
        }

        /// <summary>
        /// Since first is private, the user of class needs a way to tell if the list is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()     // shown in class
        {
            return first == null;
        }

        /// <summary>
        /// Adds an item to the front of the list
        /// </summary>
        /// <param name="value"></param>
        public void AddFront(T value)     // shown in class
        {
            first = new Node(value, first);
        }

        /// <summary>
        /// Removes the first item from the list if it exists
        /// </summary>
        public void RemoveFirst()     // shown in class
        {
            if (first != null)
                first = first.Next;
        }

        /// <summary>
        /// Returns the number of items in a list
        /// </summary>
        /// <remarks> Show how to hop down a list </remarks>
        public int Count
        {
            get 
            {
                int count = 0;
                Node pTmp = first;  // point to first node
                while (pTmp != null)    // keep hopping down the list until pTmp has fallen off the end
                {
                    count++;
                    pTmp = pTmp.Next;   // a single hop
                }
                return count;
            }
        }
        /// <summary>
        /// Adds an item to the end of the list
        /// </summary>
        /// <param name="value"></param>
        public void AddLast(T value)     // shown in class
        {
            if (first == null)
                AddFront(value);
            else
            {
                Node mTmp = first;  // point to first node
                while (mTmp.Next != null)   // hop to last node
                    mTmp = mTmp.Next;

                mTmp.Next = new Node(value, null);
            }
        }

        /// <summary>
        /// Remove the last item from a list if it exists
        /// </summary>
        public void RemoveLast()     // shown in class
        {
            if (first == null)
                return;
            else if (first.Next == null)  // can't stop at previous node
                RemoveFirst();
            else
            {
                Node pTmp = first;
                while (pTmp.Next.Next != null)  // stop at 2nd to last node
                        pTmp = pTmp.Next;

                pTmp.Next = null;
            }
        }

        /// <summary>
        /// If index equals the number of items in the IList, then value is appended to the end. 
        /// </summary>
        /// <param name="index">Zero-based</param>
        /// <param name="value"></param>     
        public void InsertAt(int index, T value)
        {
            if (index >= 0 && index <= Count)
            {
                if (index == 0)
                    AddFront(value);
                else if (index == Count)
                    AddLast(value);
                else
                {
                    Node pTmp = first;
                    for (int i = 0; i < index - 1; i++) // Hop to node in front of the index position
                        pTmp = pTmp.Next;

                    // Make node in front point to new node and new node point to next node
                    // In other words, slip a new node between node in front and node previously at 
                    // the index position
                    pTmp.Next = new Node(value, pTmp.Next); 
                }
            }
            else
                throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Remove the item at the index position. 
        /// </summary>
        /// <param name="index"> Zero based </param>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Count)
            {
                if (index == 0)
                    RemoveFirst();
                else if (index == (Count - 1))
                    RemoveLast();
                else
                {
                    Node pTmp = first;
                    for (int i = 0; i < index - 1; i++)  // Stop at node in front of node to be remove
                        pTmp = pTmp.Next;
                    pTmp.Next = pTmp.Next.Next; // Make node in front skip over node to remove
                }
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Adds an item to end
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            AddLast(value);
        }

        /// <summary>
        /// Determines if item is in the list
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if item is found on list; otherwise returns false</returns>
        public bool Contains(T value)
        {
            Node pTmp = first;
            while (pTmp != null)
            {
                if (pTmp.Data.Equals(value))
                    return true;
                pTmp = pTmp.Next;
            }
            return false;
        }
        
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes first occurence of item on the list
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if item successfully remove; otherwise returns false</returns>
        public bool Remove(T value)
        {
            if (IsEmpty() == true)
                return false;

            if (first.Data.Equals(value))
            {
                RemoveFirst();
                return true;
            }
            else
            {
                Node pTmp = first;

                while (pTmp.Next != null)
                {
                    if (pTmp.Next.Data.Equals(value)) // finds node in front of value
                    {
                        pTmp.Next = pTmp.Next.Next; // makes node in front skip over the node with value
                        return true;
                    }
                    pTmp = pTmp.Next;
                }
            }
            return false; // only gets here if value was not in the list
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
             Node pTmp = first;
            while (pTmp != null)
            {
                yield return pTmp.Data;
                pTmp = pTmp.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a string with every item. Each item's ToString() method is used.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsEmpty() == true)
                return string.Empty;
            StringBuilder returnString = new StringBuilder();
            foreach (T item in this)
            {
                if (returnString.Length > 0)
                    returnString.Append(":");
                returnString.Append(item);
            }
            return returnString.ToString();
        }

        public OurList<T> NthList(int nth)
        {
            int ctr = 0;
            OurList<T> theList = new OurList<T>();
            Node ptmp = this.first;


            if (first == null)
                return theList;
            else if(nth == 0){
                theList.Add(first.Data);
                return theList;
            
            }
            else{

                if (nth >= Count)
                {
                    while (nth>= Count){nth = nth - Count;}
                    while(ptmp != null)
                    {
                        if (ctr == nth){                       
                            theList.AddLast(ptmp.Data);
                            return theList;
                        }
                        ptmp = ptmp.Next;
                        ctr++;
                    }
                }
                
                while(ptmp != null)
                {
                    if (ctr % nth  == 0)                        
                        theList.AddLast(ptmp.Data);
                    ptmp = ptmp.Next;
                    ctr++;
                }
                return theList;
            }


        }

        public void InsertMid(OurList<T> theList)
        {
            int ctr = this.Count/2;
            Node otherptmp = theList.first;
            while(otherptmp != null)
            {
                if (ctr == 0)
                {
                    this.AddLast(otherptmp.Data);
                    otherptmp = otherptmp.Next;
                }
                else
                {
                    this.InsertAt(ctr, otherptmp.Data);
                    ctr++;
                    otherptmp = otherptmp.Next;
                }
            }
        }

        public void InsertRange(int index, T[] array)
        {
            if (index < 0)
            {
                return;
            }
            if (index == 0 || first == null)
            {
                for (int i = array.Length - 1; i >= 0; i--)
                    first = new Node(array[i], first);
            }
            else
            {
                Node ptmp = first;
                int count = 1;
                while (ptmp.Next != null)
                {
                    if (count == index)
                    {
                        for (int i = array.Length - 1; i >= 0; i--)
                        {
                            ptmp.Next = new Node(array[i], ptmp.Next);
                        }
                    }

                    count++;
                    ptmp = ptmp.Next;
                }
            }
        }
       
        public int IndexOf(T value)
        {
            if (first == null)
            {
                return -1;
            }
            int index = 0;
            Node ptmp = first;
            while(ptmp != null)
            {
                if (ptmp.Data.Equals(value))
                {
                    return index;
                }
                index++;
                ptmp = ptmp.Next;
            }
            return -1;

        }

        public void ReplaceData(T replaced, T replacer)
        {
            if (first == null)
            {
                return;   
            }
            ReplaceData(first, replaced, replacer);
        }

        private void ReplaceData(Node node, T replaced, T replacer) 
        {
            if (node == null)
                return;

            if(node.Data.Equals(replaced))
                node.Data = replacer;

            node = node.Next;
        }


        //EXAM 1 Q3 ANSWER
        public bool RemoveRange(int start, int count)
        {
            if (first == null || start < 0 || count < 0) 
            {
                return false;
            }

            if (start == 0)
            {
                for (int i = 0; i < count && first.Next != null; i++)
                {
                    first = first.Next;
                }
                return true;
            }

            Node ptmp = first;
            Node pprev = null;

            for (int i = 0; i < start; i++)
            {
                pprev = ptmp;
                ptmp = ptmp.Next;
            }

            if (ptmp == null)
                return false;

            for (int i = 0; i < count && ptmp != null; i++)
                ptmp = ptmp.Next;
            pprev.Next = ptmp;
            return true;

        }
    }
}

