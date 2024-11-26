using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// A Dictionary or Associative Array class
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class OurDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Initially, a cell is empty. If it has been used but is now "empty", the it is Deleted so as to act as a placeholder
        /// </summary>
        public enum StatusType { Empty, Active, Deleted };

        /// <summary>
        /// Private array cell that holds the status, data, and key value of the data
        /// </summary>
        /// <remarks>The key value is seperate from the data</remarks>
        private class Cell
        {
            public StatusType Status { get; set; }
            public TValue Value { get; set; }
            public TKey Key { get; set; }

            public Cell(TKey aKey = default(TKey), TValue aValue = default(TValue), StatusType aStatus = StatusType.Empty)
            {
                Key = aKey;
                Value = aValue;
                Status = aStatus;
            }
            public override string ToString()
            {
                return Key + ":" + Value;
            }
        }

        /// <summary>
        /// The hash table is an array of Cells (see above).
        /// </summary>
        private Cell[] table;    // initially each slot is null

        /// <summary>
        /// Returns a string representing the contents of the dictionary
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder hashString = new StringBuilder();
            int i = 0;
            foreach (Cell pos in table)
            {
                hashString.AppendLine($"Position {i}: {pos}");
                i++;
            }
            return hashString.ToString();
        }

        /// <summary>
        /// The constructor to create an empty dictionary
        /// </summary>
        /// <param name="size"></param>
        /// <param name="aCollisionStrategy">The user of the class can pick the collision resolution strategy. The choices are Linear, Quad, Double (e.g. Double Hashing)</param>
        public OurDictionary(int size = 31, CollisionRes aCollisionStrategy = CollisionRes.Double)
        {
            table = new Cell[NextPrime(size)];
            Strategy = aCollisionStrategy;
        }

        /// <summary>
        /// Empties the table
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < table.Length; i++)
                if (table[i] != null)
                    table[i].Status = StatusType.Deleted; // Reuse cells by setting them to Empty
        }

        /// <summary>
        /// Allows the use of [] like an array
        /// </summary>
        /// <param name="aKey"></param>
        /// <returns></returns>
        public TValue this[TKey aKey]
        {
            get
            {
                return Find(aKey);
            }
            set
            {
                // find empty cell (e.g. cell is null, status empty or deleted)
                int count = 0;
                int index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
                while (table[index] != null && table[index].Status.Equals(StatusType.Deleted) == false)
                {
                    count++;
                    if (count == table.Length) // in case table is full, kicks out of inf loop
                        throw new ApplicationException("Table is full");
                    index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
                }
                // table slot is empty
                if (table[index] == null)
                    table[index] = new Cell(aKey, value, StatusType.Active);
                // duplicate key found
                else if (table[index].Key.Equals(aKey) == true && table[index].Status == StatusType.Active)
                    table[index].Value = value;
                // previously used item, reuse it
                else if (table[index].Status == StatusType.Deleted)
                {
                    table[index].Value = value;
                    table[index].Key = aKey;
                    table[index].Status = StatusType.Active;
                }
                else
                    throw new ApplicationException("Something went wrong in Dictionary's [] set");
            }
        }
        /// <summary>
        /// Returns the data associated with the key
        /// </summary>
        /// <param name="aKey"></param>
        /// <returns>data item</returns>
        public TValue Find(TKey aKey)
        {
            // search until found or empty 
            int count = 0;
            int index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
            while (table[index] != null && table[index].Status != StatusType.Deleted && table[index].Key.Equals(aKey) == false)
            {
                count++;
                if (count == table.Length) // in case table is full, kicks out of inf loop
                    throw new ApplicationException("Table is full");
                index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
            }

            if (table[index] == null)
                throw new KeyNotFoundException("The key " + aKey.ToString() + " was not found");
            else if (table[index].Status == StatusType.Active && table[index].Key.Equals(aKey) == true)
                return table[index].Value;
            else
                throw new ApplicationException("Something went horribly wrong in Find method ???");
        }

        /// <summary>
        /// Adds a new key value and data pair using the key value 
        /// </summary>
        /// <param name="aKey"></param>
        /// <param name="aValue"></param>
        public void Add(TKey aKey, TValue aValue)
        {
            // find empty cell (e.g. cell is null, status empty or deleted)
            int count = 0;
            int index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
            while (table[index] != null && table[index].Status.Equals(StatusType.Deleted) == false)
            {
                count++;
                if (count == table.Length) // in case table is full, kicks out of inf loop
                    throw new ApplicationException("Table is full");
                index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
            }

            if (table[index] == null) // table slot is empty (e.g. never been used)
                table[index] = new Cell(aKey, aValue, StatusType.Active);
            else if (table[index].Key.Equals(aKey) == true && table[index].Status == StatusType.Active) // duplicate key found
                throw new ArgumentException("Dictionary Error: Don't add duplicate keys: " + aKey.ToString());
            else if (table[index].Status == StatusType.Deleted) // previously used item, reuse the cell
            {
                table[index].Value = aValue;
                table[index].Key = aKey;
                table[index].Status = StatusType.Active;
            }
            else
                throw new ApplicationException("Something went wrong in HashTableO Add method ???");
        }

        public enum CollisionRes { Linear, Quad, Double };
        private readonly CollisionRes Strategy;
        private int f(int i, TKey aKey)
        {
            if (i == 0)
                return 0;
            else
            {
                if (Strategy == CollisionRes.Linear)
                    return i++;
                else if (Strategy == CollisionRes.Quad)
                    return i * i;
                else if (Strategy == CollisionRes.Double)
                    return (Math.Abs(aKey.GetHashCode()) + i) % (table.Length - 1) + 1; // hash2
                else
                    throw new ApplicationException("Unknown Collision Startegy in OurHashTable)");
            }
        }

        public void Remove(TKey aKey)
        {
            //int index = Search(aKey, IsDeletedOrFound);
            int count = 0;
            int index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
            while (table[index] != null && table[index].Status != StatusType.Deleted && table[index].Key.Equals(aKey) == false)
            {
                count++;
                if (count == table.Length) // in case table is full, kicks out of inf loop
                    throw new ApplicationException("Table is full");
                index = (Math.Abs(aKey.GetHashCode()) + f(count, aKey)) % table.Length;
            }
            // Search will keep looking until found or empty cell
            if (table[index] == null)
                throw new KeyNotFoundException("Can't delete missing key:" + aKey.ToString());
            else if (table[index].Status == StatusType.Active && table[index].Key.Equals(aKey) == true)
                table[index].Status = StatusType.Deleted; // found it, make the cell deleted
            else
                throw new ApplicationException("Something went horribly wrong in HashTableO Find method ???");
        }
        private static bool IsPrime(int n)
        {
            if (n == 2 || n == 3)
                return true;
            if (n == 1 || n % 2 == 0)
                return false;
            for (int i = 3; i * i <= n; i += 2)
                if (n % i == 0)
                    return false;
            return true;
        }

        private static int NextPrime(int n)
        {
            if (n % 2 == 0)
                n++;

            for (; !IsPrime(n); n += 2)
                ;

            return n;
        }

        public int FalseOriginalPosition()
        {
            int n = 0;

            for(int i  = 0; i < table.Length; i++)
            {
                if (table[i] != null)
                {
                    int hashed_key = Math.Abs(table[i].Key.GetHashCode())%table.Length;
                    if (hashed_key != i)
                    {
                        n++;
                    }
                }
            }
            return n;
        }

        
    }
}

