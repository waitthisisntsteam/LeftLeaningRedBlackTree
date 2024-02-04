using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RedBlackTree
{
    class Program
    {
        public static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>();

            tree.Add(1);
            tree.Add(2);
            tree.Add(3);
            tree.Add(4);
            tree.Add(5);
            tree.Add(6);
            tree.Add(7);
            tree.Add(8);
            tree.Add(9);

            tree.Remove(4);

            tree.Remove(1);

            ;
        }
    }
}