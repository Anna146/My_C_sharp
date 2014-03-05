using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tree
{
    public class Node<T>: IComparable<Node<T>> where T: IComparable
    {
        //глубина элемента
        private int level;
        
        //отец элемента
        private Node<T> father;
        //список детей
        private List<Node<T>> children;
        private T data;

        public int AccLevel
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
        

        public T AccData
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }


        public Node<T> AccFather
        {
            get
            {
                return father;
            }
            set
            {
                father = value;
            }
        }
        

        public List<Node<T>> AccChildren
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }

        public Node()
        {
            this.level = 0;
            data = default(T);
            this.father = null;
            this.children = new List<Node<T>>();
        }


        public Node(T data)
        {
            this.level = 0;
            this.data = data;
            this.father = null;
            this.children = new List<Node<T>>();
        }


        public Node(T data, Node<T> dad)
        {
            this.level = dad.AccLevel+1;
            this.data = data;
            this.father = dad;
            this.children = new List<Node<T>>();
        }

        public void createChild(T data)
        {
            if (this.children == null)
                throw new Exception("Вы пытаетесь добавить детей объекту, который не был проинициализирован");
            else
            {
                Node<T> ch = new Node<T>();
                ch.children = new List<Node<T>>();
                ch.level = level + 1;
                ch.data = data;
                ch.father = this;
                this.children.Add(ch);
            }
        }

        public int CompareTo(Node<T> other)
        {
            return data.CompareTo(other.data);
        }

        internal int CompareTo(object other)
        {
            return CompareTo((Node<T>)other);
        }
    }


    class Tree<T> : IEnumerable<Node<T>> where T : IComparable
    {
        private Node<T> root;
        private int size = 0;

        public Node<T> AccRoot 
        {
            get 
            {
                return root;
            }
            set
            {
                root = value;
            }
        }

        public Tree() {
            root = new Node<T>();
            root.AccLevel = 1;
        }

        public int AccSize
        {
            get
            {
                return size;
            }
            set 
            {
                size = value;
            }
        }

        public void insertNode(Node<T> dad, T data)
        {
            if (dad == null && size != 0)
                throw new Exception("Invalid insertion");
            if (size != 0) 
            {
                dad.createChild(data);
                size++;
            }
            else
            {
                root.AccData = data;
                size++;
            }
        }

        public void insertNode(T dad, T data)
        {
            if (dad == null && size != 0)
                throw new Exception("Invalid insertion");
            bool find = false;
            Node<T> tmp = null;
            foreach (Node<T> leaf in DFS()) 
            {
                if (leaf.AccData.CompareTo(dad) == 0)
                {
                    find = true;
                    tmp = leaf;
                    break;
                }
            }
            if (find == false)
                throw new Exception("no such dad");
            if (size != 0)
            {
                tmp.createChild(data);
                size++;
            }
            else
            {
                root.AccData = data;
                size++;
            }
        }

        //удаление элемента со всем поддеревом
        public void delNodeWithCh(Node<T> dn)
        {
            dn.AccFather.AccChildren.Remove(dn);
        }

        public IEnumerable<Node<T>> DFS()
        {
            Stack<Node<T>> st = new Stack<Node<T>>();
            Node<T> tmp = new Node<T>();
            st.Push(root);
            yield return root;
            while (st.Count != 0)
            {
                tmp = st.Pop();
                foreach (Node<T> ch in tmp.AccChildren)
                {
                    st.Push(ch);
                    yield return ch;
                }
            }
        }

        public IEnumerable<Node<T>> BFS()
        {
            Queue<Node<T>> que = new Queue<Node<T>>();
            Node<T> tmp = new Node<T>();
            que.Enqueue(root);
            yield return root;
            while (que.Count != 0)
            {
                tmp = que.Dequeue();
                foreach (Node<T> ch in tmp.AccChildren)
                {
                    que.Enqueue(ch);
                    yield return ch;
                }
            }
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            foreach (Node<T> leaf in DFS())
                yield return leaf;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Node<T> leaf in DFS())
                yield return leaf;
        }
    }
}

