using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tree;

namespace Tree
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int> MyTree = new Tree<int>();
            MyTree.insertNode(null, 5);
            MyTree.insertNode(5, 13);
            MyTree.insertNode(5, 2);
            MyTree.insertNode(5, 4);

            MyTree.insertNode(13, 1);
            MyTree.insertNode(13, 8);
            MyTree.insertNode(2, 12);
            MyTree.insertNode(2, 15);
            MyTree.insertNode(2, 9);
            MyTree.insertNode(4, 21);
            MyTree.insertNode(12, 6);

            foreach (Node<int> leaf in MyTree.DFS())
            {
                Console.Out.Write(leaf.AccData + " ");
            }
            int a = 5;
        }
    }
}
