using System;

namespace MyCourseWork_02
{
    public class HtmlTreeBuilder
    {
        private string _html;
        private LinkedList<HtmlElement> _elements;

        public HtmlTreeBuilder(string html)
        {
            _html = html;
            _elements = new LinkedList<HtmlElement>();
        }

        private char ToLowerLetter(char ch)
        {
            if (ch >= 65 && ch <= 90) 
                return (char)(ch + 32);

            return ch;
        }

        private bool IsVoidHtmlTag(string tag)
        {
            var voidTags = new string[] { "area", "base", "br", "col", "command", "embed", "hr",
                "img", "input", "keygen", "link", "meta", "param", "source", "track", "wbr" };

            for (int i = 0; i < voidTags.Length; i++)
                if (tag == voidTags[i]) return true;

            return false;
        }

        private bool Equals(string tag1, string tag2)
        {
            if (tag1.Length != tag2.Length)
                return false;

            for (int i = 0; i < tag1.Length; i++)
            {
                if (ToLowerLetter(tag1[i]) != ToLowerLetter(tag2[i]))
                    return false;
            }

            return true;
        }

        public HtmlElement BuildHtmlTree()
        {
            var tag = "";
            var content = "";
            var quotsCount = 0;

            for (int i = 0; i < _html.Length; i++)
            {
                if (_html[i] == '<')
                {
                    if (tag != "")
                    {
                        if (tag[0] == '/' && _elements.Count >= 1)
                        {
                            var child = _elements.Last.Value;

                            if (Equals(_elements.Last.Value.TagName, tag.Substring(1)))
                            {
                                var parent = _elements.Last.Previous.Value;

                                if (parent != null)
                                {
                                    parent.Children.Add(child);
                                    _elements.RemoveAt(_elements.Last.Index);
                                }
                            }
                            else throw new Exception("Incorrect closing tag!");
                        }
                        else if (tag[tag.Length - 1] == '/' && _elements.Last != null)
                        {
                            content = null;
                            tag = tag.Substring(0, tag.Length - 1);
                            var element = new HtmlElement(tag, content);

                            if (!IsVoidHtmlTag(element.TagName))
                                throw new Exception("Incorrect closed tag!");
                            _elements.Last.Value.Children.Add(element);
                        }
                        else _elements.Add(new HtmlElement(tag, content));

                        tag = content = "";
                    }
                    for (int j = i + 1; j < _html.Length; j++)
                    {
                        if (_html[j] == '>' && quotsCount % 2 == 0)
                        {
                            i = j;
                            break;
                        }
                        else if (_html[j] == '"' || _html[j] == '\'')
                            quotsCount++;
                        tag += _html[j];
                    }
                }
                else if (tag != "") content += _html[i];
            }

            if (_elements.Count != 1)
                throw new Exception("Something's wrong! The operation failed!");
            return _elements.First.Value;
        }
    }
}
