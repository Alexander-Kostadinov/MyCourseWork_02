using System;

namespace MyCourseWork_02
{
    public class Setter
    {
        string _content;
        LinkedList<HtmlElement> _elements;

        public Setter (LinkedList<HtmlElement> elements, string content)
        {
            if (elements == null || content == null)
            {
                throw new NullReferenceException();
            }
            else if (elements.Count == 0)
            {
                Console.WriteLine("No matches!");
            }

            _content = content;
            _elements = elements;
        }

        public void Set()
        {
            if (Contains(_content, '<'))
            {
                SetElement();
            }
            else
            {
                var element = _elements.First;

                for (int i = 0; i < _elements.Count; i++)
                {
                    element.Value.Children.Clear();
                    element.Value.Content = _content;
                    element = element.Next;
                }
            }
        }

        private void SetElement()
        {
            var treeBuilder = new HtmlTreeBuilder(_content);
            var element = _elements.First;

            for (int i = 0; i < _elements.Count; i++)
            {
                element.Value.Children.Clear();
                element.Value.Content = string.Empty;

                if (treeBuilder.Roots.Count > 0)
                {
                    var child = treeBuilder.Roots.First;

                    for (int j = 0; j < treeBuilder.Roots.Count; j++)
                    {
                        child.Value.Parent = element.Value;
                        element.Value.Children.Add(child.Value);
                        child = child.Next;
                    }
                    treeBuilder.Roots.Clear();
                }
                if (treeBuilder.VoidElements.Count > 0)
                {
                    var voidElement = treeBuilder.VoidElements.First;

                    for (int j = 0; j < treeBuilder.VoidElements.Count; j++)
                    {
                        voidElement.Value.Parent = element.Value;
                        element.Value.Children.Add(voidElement.Value);
                        voidElement = voidElement.Next;
                    }
                    treeBuilder.VoidElements.Clear();
                }
                element = element.Next;
            }
        }

        private bool Contains(string value, char symbol)
        {
            if (value == null) return false;

            for (int i = 0; i < value.Length; i++)
                if (value[i] == symbol) return true;

            return false;
        }
    }
}
