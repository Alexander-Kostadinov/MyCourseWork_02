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

                if (htmlTree.Roots.Count == 0 || htmlTree.Roots.Count > 1)
                    throw new Exception("Invalid HTML document!");

                var htmlElement = htmlTree.Roots.First.Value;

                while (true)
                {
                    var newCommand = Console.ReadLine();
                    Console.WriteLine();

                    if (newCommand == "")
                        break;

                    Command command = new Command(newCommand, htmlElement);
                    command.Execute();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
