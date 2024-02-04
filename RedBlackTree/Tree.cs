using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedBlackTree
{
    public class Tree<T> where T : IComparable<T>
    {
        private Node<T> root;
        private int count;

        public Tree()
        {
            count = 0;
        }

        public void Add(T value)
        {
            if (root == null)
            {
                root = new Node<T>(value, false);
                count++;
            }
            else
            {
                Node<T> node = root;

                root = Add(value, node);
                root.isRed = false;
                count++;
            }
        }

        private Node<T> Add(T value, Node<T> node)
        {
            if (node.left != null && node.left.isRed && node.right != null && node.right.isRed)
            {
                FlipColor(node);
            }

            if (value.CompareTo(node.value) < 0)
            {
                if (node.left == null)
                {
                    node.left = new Node<T>(value, true);
                }
                else
                {
                    node.left = Add(value, node.left);
                }
            }
            else if (value.CompareTo(node.value) > 0)
            {
                if (node.right == null)
                {
                    node.right = new Node<T>(value, true);
                }
                else
                {
                    node.right = Add(value, node.right);
                }
            }
            else
            {
                throw new ArgumentException("Duplicates can't be added.");
            }

            if (node.right != null && node.right.isRed)
            {
                node = RotateLeft(node);
            }

            if (node.left != null && node.left.isRed && node.left.left != null && node.left.left.isRed)
            {
                node = RotateRight(node);
            }

            return node;
        }

        public void FlipColor(Node<T> node)
        {
            node.isRed = !node.isRed;
            node.right.isRed = !node.right.isRed;
            node.left.isRed = !node.left.isRed;
        }

        public Node<T> RotateLeft(Node<T> node)
        {
            var temp = node.right;
            node.right = temp.left;
            temp.left = node;

            temp.isRed = node.isRed;
            node.isRed = true;

            return temp;
        }
        public Node<T> RotateRight(Node<T> node)
        {
            var temp = node.left;
            node.left = temp.right;
            temp.right = node;

            temp.isRed = node.isRed;
            node.isRed = true;

            return temp;
        }

        public bool Remove(T value)
        {
            int start = count;
            if (root != null)
            {
                root = Remove(value, root);

                if (root != null)
                {
                    root.isRed = false;
                }
            }
            return start != count;
        }

        private Node<T> Remove(T value, Node<T> runner)
        {
            if (value.CompareTo(runner.value) < 0)
            {
                if (runner.left != null)
                {
                    if (runner.left != null && !runner.left.isRed && runner.left.left != null && !runner.left.left.isRed)
                    {
                        runner = MoveRedLeft(runner);
                    }

                    runner.left = Remove(value, runner.left);
                }
            }
            else
            {
                if (runner.left != null && runner.left.isRed)
                {
                    runner = RotateRight(runner);
                }

                if (value.CompareTo(runner.value) == 0)
                {
                    if (runner.IsLeaf())
                    {
                        count--;
                        return null;
                    }
                }

                //if value is in 2-node
                if (runner.right != null && !runner.right.isRed && runner.right.left != null && !runner.right.left.isRed)
                {
                    runner = MoveRedRight(runner);
                }
                if (value.CompareTo(runner.value) > 0 && IsTwoNode(runner))
                {
                    runner = MoveRedRight(runner);
                }
                //if value is in 3-node or 4-node
                if (value.CompareTo(runner.value) == 0)
                {
                    var min = runner.right; //
                    while (min.left != null)
                    {
                        min = min.left;
                    }
                    runner.value = min.value;
                    runner.right = Remove(runner.value, runner.right);
                }
                else
                {
                    runner.right = Remove(value, runner.right);
                }
            }

            return Fixup(runner);
        }

        private bool IsTwoNode(Node<T> node)
        {
            if (!node.left.isRed && node.left.left != null && !node.left.left.isRed)
            {
                return true;
            }

            if (node.left == null && node.right == null)
            {
                return true;
            }

            if (node.right != null && !node.right.isRed && node.right.left != null && !node.right.left.isRed)
            {
                return true;
            }    

            return false;
        }

        private bool IsThreeNodeOrFourNode(Node<T> node)
        {
            if (node.left != null && node.left.isRed && node.left.left != null && node.left.left.isRed)
            {
                return true;
            }

            return false;
        }

        private Node<T> MoveRedLeft(Node<T> node)
        {
            FlipColor(node);
            if (node.right != null && node.right.isRed && node.right.left != null && node.right.left.isRed)
            {
                node.right = RotateRight(node.right);
                node = RotateLeft(node);
                FlipColor(node);

                if (node.right.right != null && node.right.right.isRed)
                {
                    node.right = RotateLeft(node.right);
                }
            }

            return node;
        }

        private Node<T> MoveRedRight(Node<T> node)
        {
            FlipColor(node);
            if (node.left != null && node.left.isRed && node.left.left != null && node.left.left.isRed)
            {
                node = RotateRight(node);
                FlipColor(node);
            }

            return node;
        }

        private Node<T> Fixup(Node<T> node)
        {
            if (node.right != null && node.right.isRed)
            {
                node = RotateLeft(node);
            }

            if (node.left != null && node.left.isRed && node.left.left != null && node.left.left.isRed)
            {
                node = RotateRight(node);
            }

            if (!node.isRed && !node.IsLeaf() && node.right.isRed)
            {
                FlipColor(node);
            }

            if (node.left != null && !node.left.isRed && node.left.left != null && !node.left.left.isRed && node.left.right != null && node.left.right.isRed && !node.left.right.IsLeaf() && !node.left.right.left.isRed && !node.left.right.right.isRed)
            {
                node = RotateLeft(node);
                node = RotateRight(node);
            }

            return node;
        }
    }
}
