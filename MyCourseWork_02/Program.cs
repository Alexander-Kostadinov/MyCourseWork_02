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
                HtmlElement htmlElement = null;
                var path = Console.ReadLine();
                var file = System.IO.File.ReadAllText(path);
                var htmlTree = new HtmlTreeBuilder(file);
                htmlElement = htmlTree.Root;

                if (htmlElement == null)
                    throw new Exception("Invalid HTML document!");

                while (true)
                {
                    var newCommand = Console.ReadLine();
                    Console.WriteLine();

                    if (newCommand == "")
                        break;

                    Command command = new Command(newCommand, htmlElement);
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
