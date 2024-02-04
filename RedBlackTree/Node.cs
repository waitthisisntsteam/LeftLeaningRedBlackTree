using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public class Node<T>
    {
        public T value; 
        public bool isRed;
        public Node<T> left;
        public Node<T> right;
        public Node<T> parent;

        public Node(T v, bool c)
        {
            value = v;
            isRed = c;
        }
        public Node(T v, bool c, Node<T> l, Node<T> r, Node<T> p)
        { 
            value = v;
            isRed = c;
            left = l;
            right = r;
            parent = p;
        }

        public bool IsLeaf()
        {
            if (left == null && right == null)
            {
                return true;
            }
            return false;
        }
    }
}
