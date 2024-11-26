using ConsoleApplication1;
using List;
using Stack;
using System;
using System.Globalization;


class Program
{

    static void Main(string[] args) 
    {
        OurDictionary<int, int> ourDictionary = new OurDictionary<int, int>();

        //Console.WriteLine(Math.Abs(5.GetHashCode())%31);

        ourDictionary.Add(5, 8);
        ourDictionary.Add(7, 8);
        ourDictionary.Add(36, 10);
        ourDictionary.Add(67, 12);
        ourDictionary.Add(38, 50);

        Console.WriteLine(ourDictionary.FalseOriginalPosition());

    }

    public static void InsertionSort(List<T> list)
    {
        T tmp;
        int hole;

        for (int bar = 1; bar < list.Count; bar++)
        {
            tmp = list[bar];
            for (hole = bar; hole > 0 && tmp.CompareTo(list[hole-1]) < 0; hole--)
                list[hole] = list[hole-1];
            list[hole] = tmp;
        }
    }


    public static void QuickSort(List<T> alist)
    {
        QuickSort(alist, 0, alist.Count - 1);
        InsertionSort(alist);
    }

    public static void QuickSort(List<T> alist, int left, int right)
    {
        if ((left - right) < 3) //3 is the cutoff for quiecksort. Real life version is about 25
            return;
        else
        {
            T pivot = medianThree(alist, left, right);
            int i = left;
            for(int j = right; i < j;)
            {
                while (alist[++i].CompareTo(pivot) < 0) ;//Since the inccrementation is done on the boolean expression this can be done
                while (alist[pivot].CompareTo(--j) < 0) ;
                if (i < j)
                {
                    Swap(alist, i, j);
                }
                else
                {
                    break;
                }
            }
            Swap(alist, i,  right);
            QuickSort(alist, left, i-1); // Quicksort L
            QuickSort(alist, i, right);
        }
    }


    private static T medianThree(List<T> alist, int left, int right)
    {
        int center = (left + right) / 2;
        if (alist[center].CompareTo(alist[left]) < 0)
        {
            Swap(alist, left, center);
        }

        if (alist[right].CompareTo(alist[left] < 0)
        {
            Swap(alist, left, right);
        }

        if(alist[right].CompareTo(alist[center]) <0)
        {
            Swap(alist, center, right);
        }

        Swap(alist, center, right);
        return alist[right];
    }


    

}


class OurPriorityQueue<TPriority, TValue> where TPriority : IComparable<TPriority> 
{
    private class Cell
    {
        public TValue Value;
        public TPriority Priority;
        //...More Stuff
    }

    private Cell[] table;
    private int count = 0;//Increment before inserting

    public void Add(TPriority priority, TValue value)
    {
        int hole = ++count;
        for (; hole > 1 && priority.CompareTo(table[hole / 2]) < 0; hole /=2) 
        {
            table[hole] = table[hole / 2];
        }
        table[hole] = new Cell(priority, value);
    }

    public TValue Remove()
    {
        TValue value = table[1].Value;
        table[1] = table[count--];
        PercolateDown(1);
        return value;
    }

    private void PercolateDown(int hole)
    {
        Cell ptmp = table[hole];
        //pull out and aside to make loop easier

        int child;
        for (; hole * 2 <= count; hole = child)
        {
            child = hole * 2; //Assumes the left child is the highest priority

            //Compares to the right child to see if r child has highest priority
            if (child != count && table[child + 1].Priority.CompareTo(table[child].Priority) < 0)
                child++;

            //Check for second conditon: Value less than both children
            if (table[child].Priority.CompareTo(ptmp.Priority) < 0)
                table[hole] = table[child];
            else
                break;
        }
        table[hole] = ptmp;
    }
}


//class DictionaryBucket<Tkey, Tvalue>
//{
//    private class Cell
//    {
//        private Tkey key;
//        public Tkey Key
//        {
//            get
//            {
//                return key;

//            }
//        }

//        public Tvalue Value; 
//    }

//    private List<Cell>[] table;

//    public DictionaryBucket(int size = 31)
//    {
//        table = new List<Cell>[NextPrime(size)];
//        for (int i = 0; i < table.Length; i++)
//            table[i] = new List<Cell>();
//    }

//    public void Add(Tkey akey, Tvalue avalue)
//    {
//        int index = Math.Abs(akey.GetHashCode() % table.Length);

//        List<Cell> alist = table[index];
//        Cell item = alist.Find(p => p.Key.Equals(akey));

//        if (item != null)
//            //itme.Value = Value;
//            throw new Exception();
//        else
//            alist.Add(new Cell(akey, avalue));
//    }

//    public Tvalue this[Tkey akey]
//    {
//        get
//        {
//            int index = Math.Abs(akey.GetHashCode() % table.Length);
//            List<Cell> alist = table[index];
//            Cell item = alist.Find(p => p.Key.Equals(akey));

//            if (item != null)
//                return item.Value;
//            else
//                throw new Exception();
//        }

//        set
//        {
//            Add(akey, value);
//        }
//    }


//}


