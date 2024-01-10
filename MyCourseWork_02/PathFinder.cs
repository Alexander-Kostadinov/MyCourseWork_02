using System;

namespace MyCourseWork_02
{
    public class PathFinder
    {
        private LinkedList<PathTag> _pathTags;
        private Node<HtmlElement> _elementNode;
        public LinkedList<HtmlElement> Elements;

        public PathFinder(string path, Node<HtmlElement> element) 
        {
            if (path == null || element == null)
                throw new ArgumentNullException();

            _elementNode = element;
            _pathTags = new LinkedList<PathTag>();
            Elements = new LinkedList<HtmlElement>();

            GetTagsFromPath(path);

            if (_pathTags.Count > 0)
                Find(_elementNode, _pathTags.First, 0);
        }

        private PathTag GetTag(string tag)
        {
            if (tag == null) return null;

            var hasAttribute = false;
            var value = string.Empty;
            var pathTag = new PathTag();

            for (int i = 0; i < tag.Length; i++)
            {
                if (tag[i] == '[')
                {
                    for (int j = i + 1; j < tag.Length; j++)
                    {
                        if (tag[j] == ']')
                        {
                            i = j;
                            break;
                        }
                        else if (tag[j] == '@')
                        {
                            hasAttribute = true;
                        }
                        value += tag[j];
                    }
                }
                if (hasAttribute)
                {
                    pathTag.Attribute = value.Substring(1);
                    value = string.Empty;
                }
                else if (value != string.Empty)
                {
                    int number;
                    bool success = int.TryParse(value, out number);

                    if (success)
                    {
                        pathTag.TagIndex = number;
                        value = string.Empty;
                    }
                    else
                    {
                        throw new Exception("Invalid index or attribute of tag in the path!");
                    }
                }
                else pathTag.Name += tag[i];
            }
            return pathTag;
        }

        private void GetTagsFromPath(string path)
        {
            if (path.Length < 2)
                return;

            else if (path[0] != '/' || path[1] != '/')
                return;

            else if (path == "//")
            {
                var root = new PathTag();
                root.Name = _elementNode.Value.TagName;
                _pathTags.Add(root); return;
            }

            var tag = "";
            var quotsCount = 0;

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/' && i < path.Length - 1 && quotsCount % 2 == 0)
                {
                    if (path[i + 1] == '/' && i > 1)
                        throw new Exception("Incorrect path format!");

                    if (tag != "" )
                    {
                        _pathTags.Add(GetTag(tag));
                        tag = "";
                    }
                    continue;
                }
                else if (path[i] == '\'') quotsCount++;

                tag += path[i];
            }

            if (tag != "")
            {
                _pathTags.Add(GetTag(tag));
            }
        }

        private void SearchTagElements(HtmlElement element, string tag)
        {
            if (element == null || tag == null) return;

            if (element.Children.Count > 0)
            {
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    if (child.Value.TagName == tag)
                    {
                        SearchTagElements(child.Value, tag);
                    }
                    else
                    {
                        Elements.Add(child.Value);
                    }

                    child = child.Next;
                }
            }
            else
            {
                Elements.Add(element);
            }
        }

        private bool ContainsAttribute(LinkedList<string> list, string value)
        {
            var current = list.First;

            for (int i = 0; i < list.Count; i++)
            {
                if (current.Value == value)
                {
                    return true;
                }
            }

            return false;
        }

        private void Find(Node<HtmlElement> element ,Node<PathTag> tag, int level)
        {
            if (tag == null || element == null)
                return;
            else if (level != tag.Index) 
                return;

            if (tag.Value.Name == "*" && tag.Index >= 1)
            {
                SearchTagElements(element.Value, tag.Previous.Value.Name);
            }
            else if (element.Value.TagName == tag.Value.Name)
            {
                if (tag.Value.Attribute != null && 
                    !ContainsAttribute(element.Value.Attributes, tag.Value.Attribute))
                {
                    return;
                }
                if (tag.Value.TagIndex != 0 && element.Index + 1 != tag.Value.TagIndex)
                {
                    return;
                }

                if (tag == _pathTags.Last)
                {
                    Elements.Add(element.Value);
                }
                else if (tag.Next != null)
                {
                    level++;
                    var child = element.Value.Children.First;

                    for (int i = 0; i < element.Value.Children.Count; i++)
                    {
                        Find(child, tag.Next, level);
                        child = child.Next;
                    }
                }
            }
        }
    }
}
