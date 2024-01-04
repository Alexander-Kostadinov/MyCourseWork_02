using System;

namespace MyCourseWork_02
{
    public class HtmlTreeBuilder
    {
        private string _html;
        public HtmlElement Root;
        public LinkedList<HtmlElement> VoidElements;
        private LinkedList<HtmlElement> _treeElements;

        public HtmlTreeBuilder(string html)
        {
            _html = html;
            VoidElements = new LinkedList<HtmlElement>();
            _treeElements = new LinkedList<HtmlElement>();

            BuildHtmlTreeStructure();
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

        private void AppendVoidElement(string tag, int length)
        {
            if (tag == string.Empty) return;

            if (tag[tag.Length - 1] == '/')
            {
                var element = new HtmlElement(tag.Substring(0, length - 1));

                if (IsVoidHtmlTag(element.TagName) && _treeElements.Count > 0)
                {
                    element.IsVoid = true;
                    _treeElements.Last.Value.Children.Add(element);
                }
                else if (IsVoidHtmlTag(element.TagName))
                {
                    VoidElements.Add(element);
                }
                else throw new Exception("Incorrect closed tag!");
            }
        }

        private void AppendChild(Node<HtmlElement> child, string tag)
        {
            if (child == null || tag == string.Empty)
                throw new Exception("Incorrect end tag of element!");

            var parent = child.Previous;

            if (Equals(child.Value.TagName, tag) && parent != null)
            {
                child.Value.Parent = parent.Value;
                parent.Value.Children.Add(child.Value);
                _treeElements.RemoveAt(_treeElements.Count - 1);

            }

            else if (Equals(child.Value.TagName, tag) && _treeElements.Count == 1)
                return;

            else throw new Exception("Incorrect end tag of element!");
        }

        public HtmlElement BuildHtmlTreeStructure()
        {
            var quotsCount = 0;
            var content = string.Empty;
            HtmlElement element = null;

            for (int i = 0; i < _html.Length; i++)
            {
                if (_html[i] == '<')
                {
                    if (element != null)
                    {
                        element.Content = content;
                        content = string.Empty;
                    }
                    var length = 0;

                    for (int j = i + 1; j < _html.Length; j++)
                    {
                        if (_html[j] == '>' && quotsCount % 2 == 0)
                        {
                            var tag = _html.Substring(i + 1, length); i = j;

                            if (tag[0] == '/')
                            {
                                AppendChild(_treeElements.Last, tag.Substring(1));
                                element = null;
                            }
                            else if (tag[tag.Length - 1] == '/')
                            {
                                AppendVoidElement(tag, length);
                                element = null;
                            }
                            else
                            {
                                element = new HtmlElement(tag);
                                _treeElements.Add(element);
                            }
                            break;
                        }
                        else if (_html[j] == '"' || _html[j] == '\'') quotsCount++;
                        length++;
                    }
                }
                else if (element != null) content += _html[i];
            }
            return Root = _treeElements.First.Value;
        }
    }
}
