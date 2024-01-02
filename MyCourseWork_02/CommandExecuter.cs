using System;

namespace MyCourseWork_02
{
    public class CommandExecuter
    {
        private LinkedList<HtmlElement> _elements;

        public CommandExecuter(LinkedList<HtmlElement> elements)
        {
            if (elements == null) 
                throw new Exception("No elements were found!");

            _elements = elements;
        }

        public void Print()
        {
            var pathElement = _elements.First;

            for (int i = 0; i < _elements.Count; i++)
            {
                PrintHtmlElement(pathElement.Value, 0);
                pathElement = pathElement.Next;
            }
        }

        private void PrintHtmlElement(HtmlElement element, int spaceCount)
        {
            if (element == null) return;

            var attributes = "";
            var attribute = element.Attributes.First;

            if (element.Attributes.Count > 0)
            {
                for (int i = 0; i < element.Attributes.Count; i++)
                {
                    attributes += attribute.Value;

                    attribute = attribute.Next;
                }
            }

            Console.WriteLine(new string(' ', spaceCount) +
                element.TagName + ' ' + attributes + ' ' + element.Content);

            if (element.Children.Count > 0)
            {
                spaceCount++;
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    PrintHtmlElement(child.Value, spaceCount);
                    child = child.Next;
                }
            }
        }
    }
}
