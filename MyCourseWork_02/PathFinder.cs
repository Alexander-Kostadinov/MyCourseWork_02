using System;

namespace MyCourseWork_02
{
    public class PathFinder
    {
        private LinkedList<Tag> _tags;
        public LinkedList<HtmlElement> Elements;

        public PathFinder(string path, HtmlElement element) 
        {
            _tags = new LinkedList<Tag>();
            Elements = new LinkedList<HtmlElement>();

            GetTagsFromPath(path);
            FindPathElements(element, 0);
        }

        private Tag GetTagElements(string tag)
        {
            var index = "";
            var isIndex = false;
            var isAttribute = false;
            Tag htmlTag = new Tag();

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

        private void GetTagsFromPath(string path)
        {
            if (path == null || path.Length < 2)
                return;

            else if (path[0] != '/' || path[1] != '/')
                return;

            else if (path == "//")
            {
                var root = new Tag();
                root.Name = "html";
                _tags.Add(root);
                return;
            }

            var tag = "";

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    if (tag != "")
                    {
                        _tags.Add(GetTagElements(tag));
                        tag = "";
                    }
                    continue;
                }
                tag += path[i];
            }

            if (tag != "")
            {
                _tags.Add(GetTagElements(tag));
            }
        }

        private bool IsCorrectPath(HtmlElement element)
        {
            if (element == null) return false;

            var tag = _tags.Last;

            for (int i = 0; i < _tags.Count; i++)
            {
                if (element.TagName != tag.Value.Name)
                    return false;

                tag = tag.Previous;
                element = element.Parent;
            }

            return true;
        }

        private void FindPathElements(HtmlElement element, int level)
        {
            if (element == null || _tags.Count == 0) return;

            if (element.TagName == _tags.Last.Value.Name)
            {
                if (IsCorrectPath(element))
                {
                    Elements.Add(element);
                }
            }

            if (element.Children.Count > 0)
            {
                level++;
                var tag = _tags.First;
                var child = element.Children.First;


                for (int j = 0; j < _tags.Count; j++)
                {
                    if (tag.Index == level) break;
                    tag = tag.Next;
                }

                if (level > _tags.Count || tag == null) return;

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
