using System;

namespace MyCourseWork_02
{
    public class CommandExecuter
    {
        private LinkedList<HtmlElement> _elements;

        public CommandExecuter(LinkedList<HtmlElement> elements)
        {
            if (elements.Count == 0)
            {
                Console.WriteLine("No matches!");
            }

            _elements = elements;
        }

        public void Print()
        {
            var element = _elements.First;

            for (int i = 0; i < _elements.Count; i++)
            {
                PrintHtmlElement(element.Value, 0);
                element = element.Next;
            }
        }

        private string PrintAttributes(HtmlElement element)
        {
            if (element == null) return null;

            var result = string.Empty;
            var attribute = element.Attributes.First;

            for (int i = 0; i < element.Attributes.Count; i++)
            {
                result += ' ' + attribute.Value;
                attribute = attribute.Next;
            }

            return result;
        }

        private void PrintHtmlElement(HtmlElement element, int spaceCount)
        {
            if (element == null) return;

            if (element.Children.Count == 0)
            {
                if (element.IsVoid)
                {
                    Console.WriteLine(new string(' ', spaceCount) + '<' +
                        element.TagName + ' ' + PrintAttributes(element) + "/>");
                }
                else
                {
                    Console.WriteLine(new string(' ', spaceCount) + '<' + element.TagName +
                        PrintAttributes(element) + '>' + element.Content + "</" + element.TagName + '>');
                }
            }
            else if (element.Children.Count > 0)
            {
                Console.WriteLine(new string(' ', spaceCount) + '<' + element.TagName +
                    PrintAttributes(element) + '>' + element.Content);

                spaceCount++;
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    PrintHtmlElement(child.Value, spaceCount);
                    child = child.Next;
                }

                spaceCount--;
                Console.WriteLine(new string(' ', spaceCount) + "</" + element.TagName + '>');
            }
        }

        public void Set(string content)
        {
            if (Contains(content, '<'))
            {
                var treeBuilder = new HtmlTreeBuilder(content);

                if (treeBuilder.VoidElements.Count > 0)
                {
                    var element = _elements.First;
                    var voidElement = treeBuilder.VoidElements.First;

                    for (int i = 0; i < _elements.Count; i++)
                    {
                        for (int j = 0; j < treeBuilder.VoidElements.Count; j++)
                        {
                            element.Value.Children.Add(voidElement.Value);
                            voidElement = voidElement.Next;
                        }
                        element = element.Next;
                    }
                }
                if (treeBuilder.Root != null)
                {
                    var element = _elements.First;

                    for (int i = 0; i < _elements.Count; i++)
                    {
                        var child = treeBuilder.Root;
                        child.Parent = element.Value;
                        element.Value.Children.Add(child);
                        element.Value.Content = string.Empty;
                        element = element.Next;
                    }
                }
            }
            else
            {
                var element = _elements.First;

                for (int i = 0; i < _elements.Count; i++)
                {
                    element.Value.Content = content;
                    element = element.Next;
                }
            }
        }

        private bool Contains(string text, char ch)
        {
            if (text == null) return false;

            for (int i = 0; i < text.Length; i++)
                if (text[i] == ch) return true;

            return false;
        }
    }
}
