using System;

namespace MyCourseWork_02
{
    public class HtmlElement
    {
        public bool IsVoid;
        public string TagName;
        private string _content;
        public HtmlElement Parent;
        public LinkedList<string> Attributes;
        public LinkedList<HtmlElement> Children;

        public string Content 
        {
            get => _content; 
            set
            {
                if (IsEmpty(value))
                {
                    _content = null;
                }
                else
                {
                    _content = value;
                }
            }
        }

        public HtmlElement(string tag)
        {
            Attributes = new LinkedList<string>();
            Children = new LinkedList<HtmlElement>();

            GetTagAttributes(tag);
        }

        private void ValidateTagName()
        {
            if (TagName == null || TagName == "")
                throw new Exception("Tag name cannot be empty!");

            for (int i = 0; i < TagName.Length; i++)
            {
                if ((TagName[i] < 48 || TagName[i] > 57) &&
                    (TagName[i] < 65 || TagName[i] > 90) &&
                    (TagName[i] < 97 || TagName[i] > 122))
                {
                    throw new Exception("Not allowed characters in tag name!");
                }
            }
        }

        private bool IsEmpty(string text)
        {
            if (text != null)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != '\r' && text[i] != '\n' &&
                        text[i] != '\t' && text[i] != ' ') return false;
                }
            }

            return true;
        }

        private void GetTagAttributes(string tag)
        {
            var attribute = "";
            var quotsCount = 0;
            var isTagName = true;

            for (int i = 0; i < tag.Length; i++)
            {
                if (tag[i] == ' ')
                {
                    isTagName = false;                    

                    if (attribute == "") continue;
                }
                else if (tag[i] == '\'' || tag[i] == '"')
                {
                    quotsCount++;
                    attribute += tag[i];

                    if (quotsCount % 2 == 0)
                    {
                        ValidateAttribute(attribute);
                        Attributes.Add(attribute);
                        attribute = "";
                    } 
                    continue;
                }
                else if (isTagName)
                {
                    TagName += tag[i]; continue;
                }
                attribute += tag[i];
            }
            ValidateTagName();

            if (attribute != "")
            {
                ValidateAttribute(attribute);
                Attributes.Add(attribute);
            }
        }

        private bool Contains(string value, char ch)
        {
            if (value == null || value == string.Empty)
                return false;

            for (int i = 0; i < value.Length; i++)
                if (value[i] == ch) return true;

            return false;
        }

        private void ValidateAttribute(string attribute)
        {
            var attributeName = "";
            var attributeValue = "";
            bool isAttributeName = true;

            for (int i = 0; i < attribute.Length; i++)
            {
                if (attribute[i] == '=')
                {
                    isAttributeName = false;
                    continue;
                }
                else if (isAttributeName)
                {
                    attributeName += attribute[i];
                    continue;
                }
                attributeValue += attribute[i];
            }

            for (int i = 0; i < attributeName.Length; i++)
            {
                if ((attributeName[i] < 48 || attributeName[i] > 57) &&
                    (attributeName[i] < 65 || attributeName[i] > 90) &&
                    (attributeName[i] < 97 || attributeName[i] > 122) &&
                    attributeName[i] != 95 && attributeName[i] != 45 && attributeName[i] != 46)
                {
                    throw new Exception("Error detected! Not allowed characters used in attribute name!");
                }
            }

            if (attributeValue != "")
            {
                if ((attributeValue[0] != '"' && attributeValue[0] != '\'') ||
                (attributeValue[0] != attributeValue[attributeValue.Length - 1]) || Contains(attributeValue, '<'))
                    throw new Exception("Error detected! Invalid attribute value!");
            }
        }
    }
}
