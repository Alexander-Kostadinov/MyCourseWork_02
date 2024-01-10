using System;
using System.IO;

namespace MyCourseWork_02
{
    public class Command
    {
        private string _file;
        private string _name;
        private string _path;
        private string _content;
        private Node<HtmlElement> _elementNode;

        public Command(string file, string command, Node<HtmlElement> element)
        {
            _file = file;
            _elementNode = element;
            GetCommandElements(command);
        }

        public void Execute()
        {
            switch (_name)
            {
                case "PRINT":
                    var path = new PathFinder(_path, _elementNode);
                    var printer = new Printer(path.Elements);
                    printer.Print();
                    break;
                case "SET":
                    path = new PathFinder(_path, _elementNode);
                    var setter = new Setter(path.Elements, _content);
                    setter.Set();
                    break;
                case "COPY":
                    if (_path != _content)
                    {
                        path = new PathFinder(_path, _elementNode);
                        var secondPath = new PathFinder(_content, _elementNode);
                        var copier = new Copier(path.Elements, secondPath.Elements);
                        copier.Copy();
                    }
                    break;
                case "SAVE":

                    var elements = new LinkedList<string>();
                    Save(_elementNode.Value, elements, 0);
                    var html = string.Empty;

                    var element = elements.First;

                    while (element != null)
                    {
                        html += element.Value;
                        element = element.Next;
                    }

                    string file = Path.ChangeExtension(_file, ".html");
                    File.WriteAllText(file, html);
                    break;
                default:
                    break;
            }
        }

        private void Save(HtmlElement element, LinkedList<string> html, int spaceCount)
        {
            if (element == null) return;

            if (element.Children.Count == 0)
            {
                if (element.IsVoid)
                {
                     html.Add(new string(' ', spaceCount) + '<' +
                        element.TagName + ' ' + PrintAttributes(element) + "/>" + '\n');
                }
                else
                {
                    html.Add(new string(' ', spaceCount) + '<' + element.TagName
                        + PrintAttributes(element) + '>' + element.Content
                        + "</" + element.TagName + '>' + '\n');
                }
            }
            else if (element.Children.Count > 0)
            {
                html.Add(new string(' ', spaceCount) + '<' + element.TagName +
                PrintAttributes(element) + '>' + element.Content + '\n');

                spaceCount++;
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    Save(child.Value, html, spaceCount);
                    child = child.Next;
                }

                spaceCount--;
                html.Add(new string(' ', spaceCount) + "</" + element.TagName + '>' + '\n');
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

        private void GetCommandElements(string command)
        {
            if (command == null) return;

            var length = 0;
            var counter = 0;

            for (int i = 0; i < command.Length; i++)
            {
                if (command[i] == '"')
                {
                    for (int j = i + 1; j < command.Length; j++)
                    {
                        if (command[j] == '"')
                        {
                            counter++;

                            if (counter == 1)
                            {
                                _path = Substring(command, i + 1, length);
                            }
                            else
                            {
                                _content = Substring(command, i + 1, length);
                            }

                            length = 0;
                            i = j;
                            break;
                        }
                        length++;
                    }
                }
                else if (counter == 0 && command[i] != ' ')
                {
                    _name += command[i];
                }
            }
        }

        private string Substring(string text, int index, int length)
        {
            if (text == null) return null;

            int counter = 0;
            var result = new char[length];

            for (int i = index; i < text.Length; i++)
            {
                if (counter >= length)
                {
                    break;
                }

                result[counter++] = text[i];
            }

            return new string(result);
        }
    }
}
