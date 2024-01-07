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
                PrintElement(element.Value, 0);
                element = element.Next;
            }
        }

        public void Set(string content)
        {
            if (Contains(content, '<'))
            {
                SetElement(content);
            }
            else
            {
                var element = _elements.First;

                for (int i = 0; i < _elements.Count; i++)
                {
                    element.Value.Children.Clear();
                    element.Value.Content = content;
                    element = element.Next;
                }
            }
        }

        private void SetElement(string content)
        {
            var treeBuilder = new HtmlTreeBuilder(content);

            if (treeBuilder.VoidElements.Count > 0)
            {
                var element = _elements.First;
                var voidElement = treeBuilder.VoidElements.First;

                for (int i = 0; i < _elements.Count; i++)
                {
                    element.Value.Children.Clear();
                    element.Value.Content = string.Empty;

                    for (int j = 0; j < treeBuilder.VoidElements.Count; j++)
                    {
                        voidElement.Value.Parent = element.Value;
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
                    element.Value.Children.Clear();
                    element.Value.Content = string.Empty;
                    var child = treeBuilder.Root;
                    child.Parent = element.Value;
                    element.Value.Children.Add(child);
                    element = element.Next;
                }
            }
        }

        public void Copy(LinkedList<HtmlElement> elements)
        {
            var element = _elements.First;
            var element2 = elements.First;
            var copy = new HtmlElement(string.Empty);

            for (int i = 0; i < elements.Count; i++)
            {
                if (IsSamePath(element.Value, element2.Value))
                {
                    element2 = element2.Next;
                    continue;
                }

                if (element.Value.Children.Count > 0)
                {
                    var child = element.Value.Children.First;

                    for (int j = 0; j < element.Value.Children.Count; j++)
                    {
                        var copyElement = new HtmlElement(string.Empty);
                        CopyElement(child.Value, copyElement);
                        copy.Children.Add(copyElement);
                        child = child.Next;
                    }

                    element2.Value.Children.Clear();
                    element2.Value.Content = string.Empty;
                    var copyChild = copy.Children.First;

                    for (int j = 0; j < copy.Children.Count; j++)
                    {
                        copyChild.Value.Parent = element2.Value;
                        element2.Value.Children.Add(copyChild.Value);
                        copyChild = copyChild.Next;
                    }
                    copy = null;
                }
                else
                {
                    element2.Value.Content = string.Empty;
                    element2.Value.Content = element.Value.Content;
                    element2.Value.Children.Clear();
                }
                element2 = element2.Next;
            }
        }

        private bool Contains(string value, char symbol)
        {
            if (value == null) return false;

            for (int i = 0; i < value.Length; i++)
                if (value[i] == symbol) return true;

            return false;
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

        private bool IsSamePath(HtmlElement e1, HtmlElement e2)
        {
            if (e1 == null || e2 == null)
                return true;

            else if (e1.TagName != e2.TagName)
                return false;

            var parent1 = e1.Parent;
            var parent2 = e2.Parent;

            while (parent1 != null && parent2 != null)
            {
                if (parent1.TagName != parent2.TagName) return false;

                parent1 = parent1.Parent;
                parent2 = parent2.Parent;
            }

            if (parent1 == null && parent2 == null)
            {
                if (e1.Attributes.Count != e2.Attributes.Count)
                    return false;

                var att1 = e1.Attributes.First;
                var att2 = e2.Attributes.First;

                for (int i = 0; i < e1.Attributes.Count; i++)
                {
                    if (att1.Value != att2.Value) return false;

                    att1 = att1.Next;
                    att2 = att2.Next;
                }
            }

            return true;
        }

        private void CopyElement(HtmlElement element, HtmlElement copy)
        {
            if (element == null || copy == null) return;

            copy.IsVoid = element.IsVoid;
            copy.TagName = element.TagName;
            copy.Content = element.Content;

            if (element.Attributes.Count > 0)
            {
                var attribute = element.Attributes.First;

                for (int i = 0; i < element.Attributes.Count; i++)
                {
                    var copyAttribute = attribute.Value;
                    copy.Attributes.Add(copyAttribute);
                    attribute = attribute.Next;
                }
            }
            if (element.Children.Count > 0)
            {
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    var childCopy = new HtmlElement(string.Empty);
                    CopyElement(child.Value, childCopy);
                    copy.Children.Add(childCopy);
                    childCopy.Parent = copy;
                    child = child.Next;
                }
            }
        }

        private void PrintElement(HtmlElement element, int spaceCount)
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
                    PrintElement(child.Value, spaceCount);
                    child = child.Next;
                }

                spaceCount--;
                Console.WriteLine(new string(' ', spaceCount) + "</" + element.TagName + '>');
            }
        }
    }
}
