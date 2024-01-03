using System;

namespace MyCourseWork_02
{
    public class CommandExecuter
    {
        private LinkedList<HtmlElement> _elements;

        public CommandExecuter(LinkedList<HtmlElement> elements)
        {
            if (elements == null) 
                throw new Exception("No matched elements!");

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
    }
}
