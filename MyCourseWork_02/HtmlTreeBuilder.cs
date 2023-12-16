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

        private bool Equals(string tag1, string tag2)
        {
            if (tag1.Length != tag2.Length)
                return false;

            for (int i = 0; i < tag1.Length; i++)
            {
                if (ToLowerLetter(tag1[i]) != ToLowerLetter(tag2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsVoidHtmlTag(string tag)
        {
            var voidTags = new string[] { "area", "base", "br", "col", "command", "embed", "hr",
                "img", "input", "keygen", "link", "meta", "param", "source", "track", "wbr" };

            for (int i = 0; i < voidTags.Length; i++)
            {
                if (tag == voidTags[i])
                {
                    return true;
                }
            }

            return false;
        }

        private void GetHtmlElements()
        {
            for (int i = 0; i < _html.Length - 1; i++)
            {
                if (_html[i] == '<' && _html[i + 1] != '/')
                {
                    var tag = "";
                    var content = "";
                    var quotsCount = 0;
                    bool isContent = false;

                    for (int j = i; j < _html.Length - 1; j++)
                    {
                        if (_html[j] == '<')
                        {
                            if (!isContent) throw new Exception("Error detected! Missing closing bracket!");
                            i = j - 1;
                            break;
                        }
                        else if ((_html[j] == '"' || _html[j] == '\'') && !isContent)
                        {
                            tag += _html[j];
                            quotsCount++;
                            continue;
                        }
                        else if ((_html[j] == '>' || (_html[j] == '/' && _html[j + 1] == '>')) && (quotsCount % 2 == 0))
                        {
                            if (_html[j] == '/')
                            {
                                i = j; break;
                            }
                            isContent = true;
                            continue;
                        }
                        else if (!isContent)
                        {
                            tag += _html[j];
                            continue;
                        }
                        content += _html[j];
                    }

                    var element = new HtmlElement(tag, content);
                    _elements.Add(element);
                }
            }
        }

        //public TreeNode<HtmlElement> BuildHtmlTree()
        //{
        //    for (int i = 0; i < _html.Length - 1; i++)
        //    {
        //        if (_html[i] == '<' && _html[i + 1] != '/')
        //        {
        //            var tag = "";
        //            var content = "";
        //            var quotsCount = 0;
        //            bool isContent = false;

        //            for (int j = i; j < _html.Length - 1; j++)
        //            {
        //                if (_html[j] == '<')
        //                {
        //                    if (!isContent) throw new Exception("Error detected! Missing closing bracket!");
        //                    i = j - 1;
        //                    break;
        //                }
        //                else if ((_html[j] == '"' || _html[j] == '\'') && !isContent)
        //                {
        //                    tag += _html[j];
        //                    quotsCount++;
        //                    continue;
        //                }
        //                else if ((_html[j] == '>' || (_html[j] == '/' && _html[j + 1] == '>')) && (quotsCount % 2 == 0))
        //                {
        //                    if (_html[j] == '/')
        //                    {
        //                        i = j; break;
        //                    }
        //                    isContent = true;    
        //                    continue;
        //                }
        //                else if (!isContent)
        //                {
        //                    tag += _html[j];
        //                    continue;
        //                }
        //                content += _html[j];
        //            }

        //            var element = new HtmlElement(tag);

        //            if (_html[i] == '/' && !IsVoidHtmlTag(element.Tag))
        //                throw new Exception("Error detected! Improperly closed tag!");

        //            else if (IsVoidHtmlTag(element.Tag))
        //            {
        //                var parent = _elements.Last.Value;

        //                if (parent != null)
        //                    parent.Children.Add(new TreeNode<HtmlElement>(element));
        //                else throw new Exception("Error detected! Iproperly nested elements!");
        //            }
        //            else
        //            {
        //                if (!IsEmptyContent(content))
        //                    element.Content = content;
        //                _elements.Add(new TreeNode<HtmlElement>(element));
        //            }
        //        }
        //        else if (_html[i] == '<' && _html[i + 1] == '/')
        //        {
        //            var endTag = "";

        //            for (int j = i + 2; j < _html.Length; j++)
        //            {
        //                if (_html[j] == '>')  break;
        //                endTag += _html[j];
        //            }

        //            var child = _elements.Last;
        //            var parent = child.Previous;

        //            if (child != null && parent != null)
        //            {
        //                if (Equals(child.Value.Element.Tag, endTag))
        //                {
        //                    parent.Value.Children.Add(child.Value);
        //                    _elements.RemoveAt(child.Index);
        //                }
        //                else throw new Exception("Error was found! Incorrect or missing closing tag!");
        //            }
        //        }
        //    }

        //    if (_elements.Count < 1 || _elements.Count > 1)
        //        throw new Exception("Something's wrong! The operation failed!");
        //    return _elements.First.Value;
        //}
    }
}
