
namespace MyCourseWork_02
{
    public class Node<T>
    {
        public T Value;
        public int Index;
        public Node<T> Next;
        public Node<T> Previous;

        public Node(T value)
        {
            this.Value = value;
        }
    }
}
