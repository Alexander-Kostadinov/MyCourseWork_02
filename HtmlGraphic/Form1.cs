using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyCourseWork_02;
using HtmlElement = MyCourseWork_02.HtmlElement;

namespace HtmlGraphic
{
    public partial class Form1 : Form
    {
        Node<HtmlElement> Element;
        LinkedList<HtmlElement> Elements;

        public Form1()
        {
            InitializeComponent();
            Elements = new LinkedList<HtmlElement>();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 1);
            e.Graphics.DrawRectangle(pen, 20, menuStrip1.Location.Y + 
                menuStrip1.Height + 10, (Elements.Count + 1) * 45, (Elements.Count + 1) * 45);

            var element = Elements.First;

            for (int i = 0; i < Elements.Count; i++)
            {
                if (element.Value.TagName == "img")
                {
                    var src = element.Value.Attributes.First.Value;
                    bool isImgName = false;
                    var imgName = "";

                    for (int j = 0; j < src.Length; j++)
                    {
                        if (src[j] == '\'')
                        {
                            isImgName = true;
                        }
                        else if (isImgName)
                        {
                            imgName += src[j];
                        }
                    }

                    Image image = Image.FromFile(imgName);
                    e.Graphics.DrawImage(image, 150, (i + 1) * 30);
                }
                else if (element.Value.TagName == "a")
                {
                    Font linkFont = new Font("Arial", 18, FontStyle.Underline);
                    Brush linkBrush = new SolidBrush(Color.Blue);
                    e.Graphics.DrawString(element.Value.Content, linkFont, linkBrush, 20, (i + 1) * 50);
                }
                else
                {
                    Font textFont = new Font("Arial", 20);
                    Brush textBrush = new SolidBrush(Color.Black);
                    e.Graphics.DrawString(element.Value.Content, textFont, textBrush, 20, (i + 1) * 50);
                }

                element = element.Next;
            }
        }

        private void GetElementsContent(HtmlElement element)
        {
            if (element == null) return;

            if (element.Children.Count == 0)
            {
                if (element.Content != null || element.Attributes.Count > 0)
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var command = new Command(string.Empty, string.Empty, Element);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var html = string.Empty;
                LinkedList<string> elements = new LinkedList<string>();
                command.Save(Element.Value, elements, 0);

                var element = elements.First;

                for (int i = 0; i < elements.Count; i++)
                {
                    html += element.Value;
                    element = element.Next;
                }

                saveFileDialog1.AddExtension = true;
                File.WriteAllText(saveFileDialog1.FileName, html);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var html = string.Empty;
            Graphics graphics = CreateGraphics();
            graphics.Clear(Form1.DefaultBackColor);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                html = reader.ReadToEnd();
            }

            var treeBuilder = new HtmlTreeBuilder(html);
            Element = treeBuilder.Elements.First;
            GetElementsContent(treeBuilder.Elements.First.Value);

            var g = new PaintEventArgs(CreateGraphics(), new Rectangle());
            base.OnPaint(g);

            if (Elements == null) return;

            Form1_Paint(sender: sender, g);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Elements.Clear();
            openFileDialog1.Dispose();
            Graphics graphics = CreateGraphics();
            graphics.Clear(Form1.DefaultBackColor);
        }
    }
}
