using System;

namespace MyCourseWork_02
{
    public class LinkedList<T>
    {
        public Node<T> First;
        public Node<T> Last;
        public int Count;

        public void Add(T value)
        {
            var newNode = new Node<T>(value);

            if (First == null)
            {
                First = Last = newNode;
            }
            else
            {
                var last = First;

                while (last.Next != null)
                {
                    last = last.Next;
                }

                last.Next = newNode;
                Last = newNode;
                newNode.Previous = last;
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

            Node<T> node = First;

            if (index == 0)
            {
                First = node.Next;
                First.Previous = null;
                node = First;

                while (node != null)
                {
                    node.Index--;
                    node = node.Next;
                }
            }
            else if (index == Last.Index)
            {
                node = Last;
                Last = node.Previous;
                Last.Next = null;
            }
            else
            {
                while (node != null && node.Index != index)
                {
                    node = node.Next;
                }

                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
                node = node.Next;

                while (node != null)
                {
                    node.Index--;
                    node = node.Next;
                }
            }

            Count--;
        }
    }
}
