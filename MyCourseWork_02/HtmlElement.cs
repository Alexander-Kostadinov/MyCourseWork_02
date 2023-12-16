using System;

namespace MyCourseWork_02
{
    public class HtmlElement
    {
        public string Tag;
        public string Content;
        public LinkedList<string> Attributes;
        public LinkedList<HtmlElement> Children;

        public HtmlElement(string tag, string content)
        {
            this.Tag = tag;
            this.Content = content;
            Attributes = new LinkedList<string>();
            Children = new LinkedList<HtmlElement>();

            ValidateTagName();
            ValidateAttributes();
            CheckForEmptyContent();
        }

        private void ValidateTagName()
        {
            if (Tag == null)
                throw new ArgumentNullException();

            if ((Tag[0] < 65 || Tag[0] > 90) && (Tag[0] < 97 || Tag[0] > 122))
                throw new Exception("Error was found! Invalid HTML tag!");

            for (int i = 1; i < Tag.Length; i++)
            {
                if ((Tag[i] < 48 || Tag[i] > 57) &&
                    (Tag[i] < 65 || Tag[i] > 90) &&
                    (Tag[i] < 97 || Tag[i] > 122))
                    throw new Exception("Error was found! Not allowed characters in tag name!");
            }
        }

        private void ValidateAttributes()
        {
            var attribute = Attributes.First;

            while (attribute != null)
            {
                var attributeName = "";
                var attributeValue = "";
                bool isAttributeName = true;

                for (int i = 0; i < attribute.Value.Length; i++)
                {
                    if (attribute.Value[i] == '=')
                    {
                        isAttributeName = false;
                        continue;
                    }
                    else if (isAttributeName)
                    {
                        attributeName += attribute.Value[i];
                        continue;
                    }
                    attributeValue += attribute.Value[i];
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
                    attributeValue[0] != attributeValue[attributeValue.Length - 1])
                        throw new Exception("Error detected! Invalid attribute value!");
                }

                attribute = attribute.Next;
            }
        }

        private void CheckForEmptyContent()
        {
            if (Content != null)
            {
                for (int i = 0; i < Content.Length; i++)
                {
                    if (Content[i] != '\r' && Content[i] != '\n' &&
                        Content[i] != '\t' && Content[i] != ' ')
                        return;
                }
            }

            Content = null;
        }

        private void GetTagAndAttributes(string tag)
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

                    if (quotsCount % 2 == 0 && quotsCount > 0)
                    {
                        Attributes.Add(attribute);
                        attribute = "";
                    }
                    continue;
                }
                else if (isTagName)
                {
                    Tag += tag[i];
                    continue;
                }
                else if (i == tag.Length - 1 && attribute != "")
                {
                    Attributes.Add(attribute);
                }
                attribute += tag[i];
            }
        }
    }
}
