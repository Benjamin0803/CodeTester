using System;
using System.Collections.Generic;

// Student Version
namespace Queue
{
    /// <summary>
    /// A Queue class using a "circular" array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OurQueue<T> 
    {
        
        /// <summary>
        /// The front points to the first empty cell
        /// The end points to the last cell with data
        /// The size is the number of items in the queue
        /// </summary>
        public int front, end, size;
        /// <summary>
        /// The array holds the data but the data is from front to end and NOT always starting at array position 0
        /// </summary>
        public T[] myArray;

        /// <summary>
        /// Construct an emtpy queue with a specified size
        /// </summary>
        /// <param name="capacity"></param>
        public OurQueue(int capacity = 10)
        {
            myArray = new T[capacity];
        }

        /// <summary>
        /// Clears or "removes" the contents of the queue
        /// </summary>
        /// <remarks>Doesn't actually erase anything just makes it inaccessible and write over it later</remarks>
        public void Clear()
        {
            front = 0;
            end = 0;
            size = 0;
        }

        /// <summary>
        /// Since the user can't look at the private variable, we need to provide a way to tell if the queue is empty
        /// </summary>
        /// <returns>Returns true if empty; otherwise returns false</returns>
        public bool IsEmpty()
        {
            return size == 0;
        }

        /// <summary>
        /// Removes and returns the item in the front of the queue
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            size--;
            T data = myArray[front];
            Increment(ref front);
            return data;
        }
        /// <summary>
        /// Inserts the new item at the end of the queue
        /// </summary>
        /// <param name="newItem"></param>
        public void Enqueue(T newItem)
        {
            if (this.size < myArray.Length)
            {
                myArray[end] = newItem;
                Increment(ref end);
                size++;
            }
            else
                Resize();
        }
        /// <summary>
        /// If the array is full (e.g. size = array.Length) then create a new, larger array and copy the data into it.
        /// </summary>
        public void Resize()
        {
            OurQueue<T> newQueue = new OurQueue<T>(myArray.Length*2);

            int tmp = front;
            for (int i = 0; i < this.size; i++)
            { 
                newQueue.Enqueue(myArray[tmp]);
                Increment(ref tmp);
            }

            
            this.front = newQueue.front;
            this.size = newQueue.size;
            this.end = newQueue.end;
            myArray = newQueue.myArray;
        }

        /// <summary>
        /// Increments the value and handles the "gap of death"
        /// </summary>
        /// <param name="value"></param>
        private void Increment(ref int value)
        {
            if (++value == myArray.Length)
                value = 0;
        }

        public bool Contains(T item)
        {
            int tmp = front;

            for(int i = 0; i < this.size; ++i)
            {
                if (myArray[tmp].Equals(item))
                {
                    return true;
                }
                Increment(ref tmp);
            }
            return false;
        }


        public T DeQueueN(int nth)
        {
            if (nth < 0 || nth > size)
            {
                throw new ArgumentOutOfRangeException(nameof(nth));
            }
            else
            {
                int ptmp = front;
                for (int i = 0; i < nth; i++)
                {
                    Increment(ref ptmp);
                }

                T result = myArray[ptmp];

                int nextIndex = ptmp;
                Increment(ref nextIndex);
                this.size--;

                for (int i = 0; i < size; ++i)
                {
                    myArray[ptmp] = myArray[nextIndex];
                    Increment(ref nextIndex);
                    Increment(ref ptmp);
                }

                Decrement(ref end);
                return result;
            }
        }
        public void Decrement(ref int val)
        {
            if (--val == -1)
                val = myArray.Length + val;
        }


        //EXAM 1 Q1 ANSWER
        public void AddQueue(OurQueue<T> other)
        {
            int position = other.front;
            for(int i = 0; i < other.size; i++)
            {
                Enqueue(other.myArray[position]);
                other.Increment(ref position);
            }
        }
    }
}
