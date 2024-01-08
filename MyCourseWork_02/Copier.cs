using System;

namespace MyCourseWork_02
{
    public class Copier
    {
        LinkedList<HtmlElement> _elements;
        LinkedList<HtmlElement> _secondElements;

        public Copier (LinkedList<HtmlElement> elements, LinkedList<HtmlElement> secondElements)
        {
            if (elements == null || secondElements == null)
            {
                throw new NullReferenceException();
            }
            else if (elements.Count == 0)
            {
                Console.WriteLine("No matches!");
            }

            _elements = elements;
            _secondElements = secondElements;
        }

        public void Copy()
        {
            var element = _elements.First;
            var secondElement = _secondElements.First;
            var copy = new HtmlElement(string.Empty);

            for (int i = 0; i < _secondElements.Count; i++)
            {
                if (IsSamePath(element.Value, secondElement.Value))
                {
                    secondElement = secondElement.Next;
                    continue;
                }

                if (element.Value.Children.Count > 0)
                {
                    var child = element.Value.Children.First;

                    for (int j = 0; j < element.Value.Children.Count; j++)
                    {
                        var elementChild = new HtmlElement(string.Empty);
                        CopyElement(child.Value, elementChild);
                        copy.Children.Add(elementChild);
                        child = child.Next;
                    }

                    secondElement.Value.Children.Clear();
                    secondElement.Value.Content = string.Empty;
                    var copyChild = copy.Children.First;

                    for (int j = 0; j < copy.Children.Count; j++)
                    {
                        copyChild.Value.Parent = secondElement.Value;
                        secondElement.Value.Children.Add(copyChild.Value);
                        copyChild = copyChild.Next;
                    }
                }
                else
                {
                    secondElement.Value.Children.Clear();
                    secondElement.Value.Content = element.Value.Content;
                }
                secondElement = secondElement.Next; copy.Children.Clear();
            }
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
    }
}
