using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Student version */
// Reminder - turn in only code you added or modified
public class AVLTree<T> where T : IComparable<T>
{
    private class Node  // shown in class
    {
        public T Data { get; set; }
        public Node Left; // { get; set; }
        public Node Right; // { get; set; }
        public Node(T d = default, Node leftnode = null, Node rightnode = null, int aHeight = 0)
        {
            Data = d;
            Left = leftnode;
            Right = rightnode;
            Height = aHeight;
        }
        private int height;
        public int Height
        {
            get { return height; }
            set
            {
                if (value >= 0)
                    height = value;
                else
                    throw new ApplicationException("TreeNode height can't be < 0");
            }
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }

    private Node root;

    public AVLTree()
    {
        root = null;
    }
    public virtual void Clear()
    {
        root = null;
    }

    private int GetHeight(Node pTmp)
    {
        if (pTmp == null)
            return -1;
        else
            return pTmp.Height;
    }

    public void Insert(T newItem)
    {
        root = Insert(newItem, root);
    }
    private Node Insert(T newItem, Node pTmp)
    {
        if (pTmp == null)
            return new Node(newItem, null, null);
        else if (newItem.CompareTo(pTmp.Data) < 0) // newItem < pTmp.Data --> go Left
        {
            pTmp.Left = Insert(newItem, pTmp.Left);
            if ((GetHeight(pTmp.Left) - GetHeight(pTmp.Right)) == 2)
                if (newItem.CompareTo(pTmp.Left.Data) < 0)
                    pTmp = RotateLeftChild(pTmp);
                else
                    pTmp = DoubleLeftChild(pTmp);
        }
        else if (newItem.CompareTo(pTmp.Data) > 0) // newItem > pTmp.Data --> go right
        {
            pTmp.Right = Insert(newItem, pTmp.Right);
            if ((GetHeight(pTmp.Right) - GetHeight(pTmp.Left)) == 2)
                if (newItem.CompareTo(pTmp.Right.Data) > 0)
                    pTmp = RotateRightChild(pTmp);
                else
                    pTmp = DoubleRightChild(pTmp);
        }
        else   // Duplicate
            throw new ApplicationException("Tree did not insert " + newItem + " since an item with that value is already in the tree");

        pTmp.Height = Math.Max(GetHeight(pTmp.Left), GetHeight(pTmp.Right)) + 1;
        return pTmp;
    }

    /*
     *   Before                 After
     *   pTop -->   *             *  <-- pLeft
     *             /             / \
     *   PLeft--> *             *   * <-- pTop
     *           /
     *          *
     */
    private Node RotateLeftChild(Node pTop)
    {
        Node pLeft = pTop.Left;
        pTop.Left = pLeft.Right;
        pLeft.Right = pTop;

        // update heights
        pTop.Height = Math.Max(GetHeight(pTop.Left), GetHeight(pTop.Right)) + 1;
        pLeft.Height = Math.Max(GetHeight(pLeft.Left), GetHeight(pTop)) + 1;

        return pLeft;  // attached to caller as the new top of this subtree
    }

    /*
     *   Before             After
     *     pTop --> *                  *  <-- pRight
     *               \                / \
     *      pRight--> *     pTop --> *   * 
     *                 \
     *                  *
     */
    private Node RotateRightChild(Node pTop)
    {
        Node pRight = pTop.Right;
        pTop.Right = pRight.Left;
        pRight.Left = pTop;

        // update heights
        pTop.Height = Math.Max(GetHeight(pTop.Left), GetHeight(pTop.Right)) + 1;
        pRight.Height = Math.Max(GetHeight(pRight.Left), GetHeight(pTop)) + 1;

        return pRight; // attached to caller as the new top of this subtree
    }

    private Node DoubleLeftChild(Node pTmp)
    {
        pTmp.Left = RotateRightChild(pTmp.Left);
        return RotateLeftChild(pTmp);
    }
    private Node DoubleRightChild(Node pTmp)
    {
        pTmp.Right = RotateLeftChild(pTmp.Right);
        return RotateRightChild(pTmp);
    }

    public T FindNR(T value)
    {
        Node pTmp = root;

        while (pTmp != null)
        {
            int result = value.CompareTo(pTmp.Data);

            if (result == 0)
                return pTmp.Data;
            else if (result < 0) // value < pTmp.Data
                pTmp = pTmp.Left;
            else
                pTmp = pTmp.Right;
        }

        throw new ApplicationException("AVL Tree could not find " + value);   // Item not found
    }

    public T Find(T value)
    {
        return Find(value, root);
    }
    private T Find(T value, Node pTmp)
    {
        if (pTmp == null)
            throw new ApplicationException("BinSearchTree could not find " + value);   // Item not found
        else if (value.CompareTo(pTmp.Data) < 0) // value < pTmp.Data
            return Find(value, pTmp.Left);      // search left subtree
        else if (value.CompareTo(pTmp.Data) > 0) // value > pTmp.Data
            return Find(value, pTmp.Right);     // search right subtree
        else
            return pTmp.Data;               // Found it
    }

    public bool TryFind(ref T value)
    {
        Node pTmp = root;
        int result;
        while (pTmp != null)
        {
            result = value.CompareTo(pTmp.Data);
            if (result == 0)
            {           // found it
                value = pTmp.Data;            //WHY THIS LINEEEE
                return true;
            }
            else if (result < 0)        // value < pTmp.Data, search left subtree
                pTmp = pTmp.Left;
            else if (result > 0)        // value > pTmp.Data, search right subtree
                pTmp = pTmp.Right;
        }
        return false;       // didn't find it
    }



    public bool Contains(T value)
    {
        throw new NotImplementedException("AVL Contains not implemented");
    }

    public T FindMinNR()    // NR - non-recursive version
    {
        if (root == null)
            throw new ApplicationException("Can find min on empty tree");
        else
        {
            Node pTmp = root;
            while (pTmp.Left != null)
                pTmp = pTmp.Left;
            return pTmp.Data;
        }
    }
    public T FindMin()
    {
        if (root == null)
            throw new ApplicationException("FindMin called on empty BinSearchTree");
        else
            return FindMin(root);
    }
    private T FindMin(Node pTmp)
    {
        if (pTmp.Left == null)
            return pTmp.Data;
        else
            return FindMin(pTmp.Left);
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

    public void Remove(T value)
    {
        Remove(value, ref root);
    }
    private void Remove(T value, ref Node pTmp)
    {
        if (pTmp == null)
            throw new ApplicationException("BinSearchTree could not remove " + value);   // Item not found
        else if (value.CompareTo(pTmp.Data) < 0) // value < pTmp.Data, check left subtree
        {
            Remove(value, ref pTmp.Left);  // similar to BST's find and remove method
            if (SubtreeBalance(pTmp) <= -2) // negative balance means heavy on right side
            {
                if (SubtreeBalance(pTmp.Right) <= 0) // children in straight line
                    pTmp = RotateRightChild(pTmp);   //  rotate middle up to balance   
                else
                    pTmp = DoubleRightChild(pTmp);  // children in zig patter - needs double rotate to balance
            }
        }
        else if (value.CompareTo(pTmp.Data) > 0) // value > pTmp.Data, check right subtree
        {
            Remove(value, ref pTmp.Right);
            if (SubtreeBalance(pTmp) >= 2)
            {
                if (SubtreeBalance(pTmp.Left) >= 0)
                    pTmp = RotateLeftChild(pTmp);
                else
                    pTmp = DoubleLeftChild(pTmp);
            }
        }
        else if (pTmp.Left != null && pTmp.Right != null) // Two children
        {
            pTmp.Data = FindMin(pTmp.Right);
            Remove(pTmp.Data, ref pTmp.Right);
            if (SubtreeBalance(pTmp) == 2)//rebalancing
            {
                if (SubtreeBalance(pTmp.Left) >= 0)
                    pTmp = RotateLeftChild(pTmp);
                else
                    pTmp = DoubleLeftChild(pTmp);
            }

        }
        else
            pTmp = (pTmp.Left != null) ? pTmp.Left : pTmp.Right;  // replace with one or no child
    }
    private int SubtreeBalance(Node pTmp)
    {
        // If nodes were deleted, the height must be recomputed
        UpdateHeight(pTmp.Left);
        UpdateHeight(pTmp.Right);
        if (pTmp == null)
            return 0;
        else if (pTmp.Left == null && pTmp.Right == null)
            return 0;
        else if (pTmp.Left == null)
            return -(pTmp.Right.Height + 1);    // right side heavy represented by negative number
        else if (pTmp.Right == null)
            return pTmp.Left.Height + 1;
        else
            return pTmp.Left.Height - pTmp.Right.Height;
    }
    private int UpdateHeight(Node pTmp)   // Maybe a HW assignment?? but this means I can't give them the remove method
    {
        int height = -1;
        if (pTmp != null)
        {
            int l = UpdateHeight(pTmp.Left);
            int r = UpdateHeight(pTmp.Right);
            height = Math.Max(l, r) + 1;
            pTmp.Height = height;
        }
        return height;
    }

    public List<T> RangeInTree(int low, int high)
    {
        List<T> list = new List<T>();
        RangeInTree(low, high, this.root, ref list);
        return list;
    }
    private void RangeInTree(int low, int high, Node ptmp, ref List<T> alist)
    {
        if (ptmp == null) { return; }

        if(low.CompareTo(ptmp.Data) < 0)
            RangeInTree(low, high, ptmp.Left, ref alist);

        if (low.CompareTo(ptmp.Data) <= 0 && high.CompareTo(ptmp.Data) >=0)
            alist.Add(ptmp.Data);

        if (high.CompareTo(ptmp.Data) > 0)
            RangeInTree(low, high, ptmp.Right, ref alist);                
    }


    public List<T> Range(int low, int high)
    {
        List<T> aList = new List<T>();
        Range(low, high, this.root, ref aList);
        return aList;
    }
    private void Range(int low, int high, Node pTmp, ref List<T> aList1)
    {
        if (pTmp == null)
            return;
        if (low.CompareTo(pTmp.Data) < 0)
            Range(low, high, pTmp.Left, ref aList1);

        if (high.CompareTo(pTmp.Data) > 0)
            Range(low, high, pTmp.Right, ref aList1);

        if (low.CompareTo(pTmp.Data) <= 0 && high.CompareTo(pTmp.Data) >= 0)
            aList1.Add(pTmp.Data);

    }


}
