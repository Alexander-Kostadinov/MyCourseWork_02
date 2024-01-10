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
                    if (!element.Value.IsVoid)
                    {
                        element.Value.Children.Clear();
                        element.Value.Content = _content;
                    }

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
                if (element.Value.IsVoid)
                {
                    element = element.Next;
                    continue;
                }

                element.Value.Children.Clear();
                element.Value.Content = string.Empty;

                if (treeBuilder.Elements.Count > 0)
                {
                    var child = treeBuilder.Elements.First;

                    for (int j = 0; j < treeBuilder.Elements.Count; j++)
                    {
                        child.Value.Parent = element.Value;
                        element.Value.Children.Add(child.Value);
                        child = child.Next;
                    }
                    treeBuilder.Elements.Clear();
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
