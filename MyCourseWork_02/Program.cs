using System;

namespace MyCourseWork_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path of the file:");

            try
            {
                var path = Console.ReadLine();
                var file = System.IO.File.ReadAllText(path);
                var htmlTree = new HtmlTreeBuilder(file);

                if (htmlTree.Elements.Count == 0 || htmlTree.Elements.Count > 1)
                    throw new Exception("Invalid HTML document!");

                var htmlElement = htmlTree.Elements.First;

                while (true)
                {
                    var newCommand = Console.ReadLine();
                    Console.WriteLine();

                    if (newCommand == "")
                        break;

                    Command command = new Command(path, newCommand, htmlElement);
                    command.Execute();
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
