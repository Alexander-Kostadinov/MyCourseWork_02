
namespace MyCourseWork_02
{
    public class PathFinder
    {
        private LinkedList<PathTag> _pathTags;
        public LinkedList<HtmlElement> ElementsFound;

        public PathFinder(string path, HtmlElement element) 
        {
            _pathTags = new LinkedList<PathTag>();
            ElementsFound = new LinkedList<HtmlElement>();

            GetTagsFromPath(path);
            FindPathElements(element, 0);
        }

        private void GetTagsFromPath(string path)
        {
            if (path == null || path.Length < 2)
                return;

            else if (path[0] != '/' || path[1] != '/')
                return;

            else if (path == "//")
            {
                var root = new PathTag();
                root.Name = "html";
                _pathTags.Add(root);
                return;
            }

            var tag = "";

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    if (tag != "")
                    {
                        _pathTags.Add(GetTagElements(tag));
                        tag = "";
                    }
                    continue;
                }
                tag += path[i];
            }

            if (tag != "")
            {
                _pathTags.Add(GetTagElements(tag));
            }
        }

        private PathTag GetTagElements(string tag)
        {
            var index = "";
            var isIndex = false;
            var isAttribute = false;
            PathTag htmlTag = new PathTag();

            for (int i = 0; i < tag.Length - 1; i++)
            {
                if (tag[i] == ']')
                    break;

                else if (tag[i] == '[' && tag[i + 1] != '@')
                    isIndex = true;

                else if (tag[i] == '[' && tag[i + 1] == '@')
                {
                    i++;
                    isAttribute = true;
                }

                else if (isAttribute)
                    htmlTag.Attribute += tag[i];

                else if (isIndex && tag[i] >= 48 && tag[i] <= 57)
                    index += tag[i];

                else
                    htmlTag.Name += tag[i];
            }

            if (index != "")
            {
                htmlTag.TagIndex = int.Parse(index);
            }
            else if (isAttribute == isIndex)
            {
                htmlTag.Name += tag[tag.Length - 1];
            }

            return htmlTag;
        }

        private bool IsCorrectPath(HtmlElement element)
        {
            if (element == null) return false;

            var tag = _pathTags.Last;

            for (int i = 0; i < _pathTags.Count; i++)
            {
                if (element == null || tag.Value == null)
                    return false;
                else if (element.TagName != tag.Value.Name)
                    return false;

                tag = tag.Previous;
                element = element.Parent;
            }

            return true;
        }

        private void CheckForSearchedElement(HtmlElement element)
        {
            if (_pathTags.Last.Value.Name == "*" && _pathTags.Last.Previous != null)
            {
                if (element.TagName == _pathTags.Last.Previous.Value.Name)
                {
                    _pathTags.RemoveAt(_pathTags.Count - 1);

                    if (IsCorrectPath(element))
                        FindTagElements(element, _pathTags.Last.Value.Name);

                    var tag = new PathTag();
                    tag.Name = "*";
                    _pathTags.Add(tag);
                }
            }
            else if (element.TagName == _pathTags.Last.Value.Name)
            {
                if (IsCorrectPath(element))
                    ElementsFound.Add(element);
            }
        }

        private void FindPathElements(HtmlElement element, int level)
        {
            if (element == null || _pathTags.Count == 0) return;

            CheckForSearchedElement(element);

            if (element.Children.Count > 0)
            {
                level++;
                var tag = _pathTags.First;
                var child = element.Children.First;

                for (int j = 0; j < _pathTags.Count; j++)
                {
                    if (tag.Index == level) break;
                    tag = tag.Next;
                }

                if (level > _pathTags.Count || tag == null) return;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    if (tag.Value.TagIndex != 0 && child.Index + 1 != tag.Value.TagIndex)
                    {
                        child = child.Next;
                        continue;
                    }
                    else if (tag.Value.Attribute != null &&
                        !ContainsAttribute(child.Value.Attributes, tag.Value.Attribute))
                    {
                        child = child.Next;
                        continue;
                    }

                    FindPathElements(child.Value, level);
                    child = child.Next;
                }
            }
        }

        private void FindTagElements(HtmlElement element, string tag)
        {
            if (element == null || tag == null) return;

            if (element.Children.Count > 0)
            {
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    if (child.Value.TagName == tag)
                    {
                        FindTagElements(child.Value, tag);
                    }
                    else
                    {
                        ElementsFound.Add(child.Value);
                    }

                    child = child.Next;
                }
            }
            else
            {
                ElementsFound.Add(element);
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
    }
}
