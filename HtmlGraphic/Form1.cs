using System;
using System.IO;
using MyCourseWork_02;
using System.Windows.Forms;
using System.Windows.Controls;
using HtmlElement = MyCourseWork_02.HtmlElement;

namespace HtmlGraphic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFile.FileName);

                var html = reader.ReadToEnd();
                var htmlTree = new HtmlTreeBuilder(html);
            }
        }

        private void GetElementsContent(HtmlElement element)
        {
            if (element == null) return;

            if (element.Content != null)
            {

            }

            if (element.Children.Count > 0)
            {
                var children = element.Children.First;

                for (int i = 0; i < element.Children.Count; i++)
                {
                    GetElementsContent(children.Value);
                    children = children.Next;
                }
            }
        }
    }
}
