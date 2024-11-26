using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// Student Version
namespace Stack
{
    /// <summary>
    /// A Stack class that uses a linked list 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OurStack<T>
    {
        /// <summary>
        /// Represents a node in the stack
        /// </summary>
        /// <remarks>This node cannot be used outside of the Stack class. The user of the class passes data into the stack but not nodes.</remarks>
        private class Node
        {
            /// <summary>  Data held by the node  </summary>
            public T Data { get; set; }
            /// <summary>
            /// Points to next node in the stack or 
            /// null if it is last node in the stack
            /// </summary>
            public Node Next { get; set; }
            public Node(T d = default(T), Node node = null)
            {
                Data = d;
                Next = node;
            }
        }
        /// <summary>
        /// Points to the top node in the stack or null if the stack is empty
        /// </summary>
        private Node top;

        /// <summary>
        /// Constructor that creates an empty stack
        /// </summary>
        public OurStack()
        {
            top = null;
        }

        /// <summary>
        /// Make the stack empty (e.g. clear the contents)
        /// </summary>
        public void Clear()
        {
            top = null;
        }
        /// <summary>
        /// Since top is private, the user of class needs a way to tell if the stack is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return top == null;
        }

        

        /// <summary>
        /// Inserts an item on top of the stack
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            top = new Node(value, top);
        }

        /// <summary>
        /// Removes and returns the item on the top of the stack
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (IsEmpty() == true)
                throw new ApplicationException("Error: can't pop an empty stack");
            T removedData = top.Data;
            top = top.Next;
            return removedData;
        }

        /// <summary>
        /// Returns the item on the top of the stack without reomving it
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (IsEmpty() == true)
                throw new ApplicationException("Error: can't peek at an empty stack");
            return top.Data;
        }
        /// <summary>
        /// Returns the number of items in a stack
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                Node pTmp = top;
                while (pTmp != null)
                {
                    count++;
                    pTmp = pTmp.Next;
                }
                return count;
            }
        }

        /// <summary>
        /// Returns a string with every item. Each item's ToString() method is used.
        /// </summary>
        /// <returns>a string</returns>
        public override string ToString()
        {
            if (IsEmpty() == true)
                return string.Empty;

            StringBuilder returnString = new StringBuilder();
            Node pTmp = top;
            while (pTmp != null)
            {
                if (returnString.Length > 0)
                    returnString.Append(":");
                returnString.Append(pTmp.Data);
                pTmp = pTmp.Next;
            }
            return returnString.ToString();
        }

        public int DistanceFromTop(T value)
        {
            int ctr = 0;
            Node ptmp = top;

            while (ptmp != null)
            {
                if (ptmp.Data.Equals(value))
                    return ctr;
                ctr++;
                ptmp = ptmp.Next;
            }
            return -1;
        }

        /// <summary>
        /// Funciton that adds another stack at the end of this one.
        /// </summary>
        /// <param name="secondStack"></param>

        public void AddStack(OurStack<T> secondStack)
        {
            if (top == null || secondStack.top == null)
            {
                Console.WriteLine("Empty Stack");
                return;
            }

            Node ptmp = top;
            while (ptmp.Next != null)
            {
                ptmp = ptmp.Next;
            }

            Node otherPtmp = secondStack.top;
            while (otherPtmp != null)
            {
                ptmp.Next = new Node(otherPtmp.Data, ptmp.Next);
                otherPtmp = otherPtmp.Next;
                ptmp = ptmp.Next;
            }
        }

        public OurStack<T> ReverseStack()
        {
            OurStack<T> reversed = new OurStack<T>();
            while (!IsEmpty())
            {
                reversed.Push(this.Pop());
            }
            return reversed;
        }

        public T Remove(T value)
        {
            Node ptmp = top;
            T storage;

            if (ptmp.Data.Equals(value))
                return Pop();

            while (ptmp != null)
            {
                if(ptmp.Next != null && ptmp.Next.Data.Equals(value))
                {
                    storage = ptmp.Next.Data;
                    ptmp.Next = ptmp.Next.Next;
                    return storage;
                }

                ptmp = ptmp.Next;
            }
            throw new Exception("The value is not part of the Queue");
        }


    }

    /// <summary>
    /// A Stack class that uses an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OurArrayStack<T> //: IEnumerable<T>, ICollection<T>
    {
        /// <summary>
        /// The array that holds the data in the stack
        /// </summary>
        private T[] mArray;
        /// <summary>
        /// Points to the next empty cell in the array
        /// </summary>
        private int top = 0;

        /// <summary>
        /// Creates an empty stack of the specified size
        /// </summary>
        /// <param name="size"></param>
        public OurArrayStack(int size = 10)
        {
            if (size <= 0)
                throw new ApplicationException("Stack size must be > 0");
            else
                mArray = new T[size];
        }

        /// <summary>
        /// Makes the stack empty (e.g. clears the contents)
        /// </summary>
        /// <remarks> Doesn't erase the data, just makes it inaccessible</remarks>
        public void Clear()
        {
            top = 0;
        }

        /// <summary>
        /// Since top is private, the user of class needs a way to tell if the stack is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return top == 0;
        }

        /// <summary>
        /// Inserts an item on top of the stack
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            if (top < mArray.Length)
                mArray[top++] = value;
        }


        /// <summary>
        /// Removes and returns the item on the top of the stack
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (IsEmpty() == true)
                throw new ApplicationException("Error: can't pop an empty stack");
            return mArray[--top];
        }

        /// <summary>
        /// Returns the item on the top of the stack without reomving it
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (IsEmpty() == true)
                throw new ApplicationException("Error: can't peek at an empty stack");
            return mArray[top - 1];
        }

        /// <summary>
        /// Returns the number of items in a stack
        /// </summary>
        public int Count
        {
            get
            {
                return top;
            }
        }

        /// <summary>
        /// Returns a string with every item. Each item's ToString() method is used.
        /// </summary>
        /// <returns>a string</returns>
        public override string ToString()
        {
            if (IsEmpty() == true)
                return string.Empty;

            StringBuilder returnString = new StringBuilder();
            int pTmp = top;
            while (pTmp > 0)
            {
                if (pTmp < top)
                    returnString.Append(":");
                returnString.Append(mArray[--pTmp]);
            }
            return returnString.ToString();
        }

        
    }
}

