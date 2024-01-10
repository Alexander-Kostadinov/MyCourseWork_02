using System;

namespace MyCourseWork_02
{
    public class LinkedList<T>
    {
        public int Count;
        public Node<T> Last;
        public Node<T> First;

        public void Clear()
        {
            var node = First;

            while (Count > 0)
            {
                RemoveAt(node.Index);
            }

            Last = null;
        }

        public void Add(T value)
        {
            var newNode = new Node<T>(value);

            if (First == null)
            {
                First = Last = newNode;
            }
            else
            {
                var node = First;

                while (node.Next != null)
                {
                    node = node.Next;
                }

                node.Next = newNode;
                Last = newNode;
                newNode.Previous = node;
            }

            Count++;
            newNode.Index = Count - 1;
        }

        public void RemoveAt(int index)
        {
            if (index > Count - 1 || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Node<T> node = null;

            if (index == 0)
            {
                if (First != null)
                {
                    First = First.Next;
                    node = First;
                }
            }
            else if (index == Count - 1)
            {
                Last = Last.Previous;
                Last.Next = null;
            }
            else
            {
                node = First;

                while (node != null && node.Index != index)
                {
                    node = node.Next;
                }

                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
                node = node.Next;
            }

            while (node != null)
            {
                node.Index--;
                node = node.Next;
            }

            Count--;
        }
    }
}
