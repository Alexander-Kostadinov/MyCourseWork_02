using System;
using System.IO;
using System.Drawing;
using MyCourseWork_02;
using System.Windows.Forms;
using HtmlElement = MyCourseWork_02.HtmlElement;

namespace HtmlGraphic
{
    public partial class Form1 : Form
    {
        Node<HtmlElement> Element;
        LinkedList<HtmlElement> Elements;
        LinkedList<LinkLabel> LinkLabels;

        public Form1()
        {
            AutoScroll = true;
            InitializeComponent();
            Elements = new LinkedList<HtmlElement>();
            LinkLabels = new LinkedList<LinkLabel>();
        }

        private void PaintElements()
        {
            if (Elements.Count == 0)
            {
                return;
            }

            var x = 20;
            var y = 30;
            var element = Elements.First;

            for (int i = 0; i < Elements.Count; i++)
            {
                if (element.Value.TagName == "img")
                {
                    PictureBox picture = new PictureBox();
                    PaintBitmap(element.Value, picture);
                    picture.Location = new Point(x, y);
                    y += picture.Size.Height + 10;
                    Controls.Add(picture);
                }
                else if (element.Value.TagName == "table")
                {
                    TableLayoutPanel table = new TableLayoutPanel();
                    PaintTable(element.Value, table);
                    table.Location = new Point(x, y);
                    y += table.Height + 10;
                    Controls.Add(table);
                }
                else if (element.Value.TagName == "a")
                {
                    LinkLabel linkLabel = new LinkLabel();
                    LinkLabel.Link link = new LinkLabel.Link();
                    link.LinkData = GetLinkUrl(element.Value);
                    linkLabel.Links.Add(link);
                    linkLabel.AutoSize = true;
                    linkLabel.Font = new Font("Arial",
                        12, FontStyle.Underline);
                    linkLabel.ForeColor = Color.Blue;
                    linkLabel.Location = new Point(x, y);
                    linkLabel.Text = element.Value.Content;
                    linkLabel.MouseClick += Label_MouseClick;
                    LinkLabels.Add(linkLabel);
                    Controls.Add(linkLabel);
                    y += linkLabel.Height + 10;
                }
                else
                {
                    Label label = new Label();
                    label.Location = new Point(x, y);
                    label.Font = new Font("Arial", 14);
                    label.Text += element.Value.Content;
                    y += label.Height + 10;
                    label.AutoSize = true;
                    Controls.Add(label);
                }

                element = element.Next;
            }
        }

        private void Label_MouseClick(object sender, MouseEventArgs e)
        {
            var link = LinkLabels.First;

            for (int i = 0; i < LinkLabels.Count; i++)
            {
                if (link.Value == sender)
                {
                    break;
                }

                link = link.Next;
            }

            try
            {
                var url = link.Value.Links[0];

                if (url != null)
                {
                    System.Diagnostics.Process.Start(url.LinkData.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetLinkUrl(HtmlElement element)
        {
            var link = "";
            var attrName = "";
            bool isAttrValue = false;
            var attribute = element.Attributes.First;

            for (int i = 0; i < element.Attributes.Count; i++)
            {
                for (int j = 0; j < attribute.Value.Length - 1; j++)
                {
                    if (attribute.Value[j] == '=' && attribute.Value[j + 1] == '\'')
                    {
                        if (attrName != "href")
                        {
                            break;
                        }

                        isAttrValue = true; j++;
                    }
                    else if (isAttrValue)
                    {
                        link += attribute.Value[j];
                    }
                    else attrName += attribute.Value[j];
                }

                attribute = attribute.Next;
            }

            return link;
        }

        private void PaintTable(HtmlElement element, TableLayoutPanel table)
        {
            var y = 0;
            table.Width = 0;
            table.Height = 0;
            var tableChild = element.Children.First;

            for (int i = 0; i < element.Children.Count; i++)
            {
                if (tableChild.Value.TagName == "tr")
                {
                    TableLayoutPanel panel = new TableLayoutPanel();
                    var rowChild = tableChild.Value.Children.First;
                    panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                    panel.RowCount = 1;
                    table.RowCount++;

                    for (int j = 0; j < tableChild.Value.Children.Count; j++)
                    {
                        if (rowChild.Value.TagName == "td")
                        {
                            panel.ColumnCount++;
                            Label label = new Label();
                            label.Font = new Font("Arial", 14);
                            label.Text = rowChild.Value.Content;
                            label.AutoSize = true;
                            panel.Controls.Add(label, j, 0);
                        }

                        rowChild = rowChild.Next;
                    }

                    panel.AutoSize = true;
                    table.Controls.Add(panel, 0, i);

                    if (i == 0)
                    {
                        y = panel.Bounds.Y;
                    }
                    if (table.Width < panel.Width)
                    {
                        table.Width = panel.Width + (2 * panel.Bounds.X);
                    }

                    table.Height = 2 * panel.Bounds.Y - y;
                }

                tableChild = tableChild.Next;
            }

            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
        }

        private void PaintBitmap(HtmlElement element, PictureBox picture)
        {
            var attrName = "";
            var fileName = "";
            bool isFileName = false;
            var source = element.Attributes.First;

            for (int j = 0; j < source.Value.Length - 1; j++)
            {
                if (source.Value[j] == '=' && source.Value[j + 1] == '\'')
                {
                    isFileName = true; j++;
                }
                else if (isFileName && attrName == "src")
                {
                    fileName += source.Value[j];
                }
                else attrName += source.Value[j];
            }

            var path = Path.GetDirectoryName(openFileDialog1.FileName);

            if (File.Exists(path + '\\' + fileName))
            {
                Bitmap bitmap = new Bitmap(path + '\\' + fileName);;
                picture.Height = bitmap.Height;
                picture.Width = bitmap.Width;
                picture.Image = bitmap;
            }
        }

        private void GetElementsContent(HtmlElement element)
        {
            if (element == null) return;

            if (element.TagName == "table")
            {
                Elements.Add(element);
            }
            else if (element.Children.Count == 0)
            {
                if (element.Content != null ||
                    element.TagName == "img" || element.TagName == "a")
                {
                    Elements.Add(element);
                }
            }
            else
            {
                var child = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    GetElementsContent(child.Value);
                    child = child.Next;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Elements.Count > 0)
            {
                Exception ex = new Exception("There is an already open file! " +
                    "Please, close it befoere open a new file!");
                MessageBox.Show(ex.Message);
                return;
            }

            var html = string.Empty;
            HtmlTreeBuilder treeBuilder = null;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                html = reader.ReadToEnd();
            }

            if (html != null && html != "")
            {
                treeBuilder = new HtmlTreeBuilder(html);
                Element = treeBuilder.Elements.First;

                if (Element.Value.TagName != "html")
                {
                    return;
                }

                GetElementsContent(treeBuilder.Elements.First.Value);
            }

            if (Elements == null) 
                return;

            PaintElements();
            openFileDialog1.Dispose();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = menuStrip1;
            Elements.Clear();
            Controls.Clear();
            Controls.Add(menu);
            Refresh();
        }
    }
}
