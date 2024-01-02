using System;

namespace MyCourseWork_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path of the file:");

            HtmlElement root = null;

            try
            {
                var path = Console.ReadLine();
                var file = System.IO.File.ReadAllText(path);
                var htmlTree = new HtmlTreeBuilder(file);
                root = htmlTree.Root;

                while (true)
                {
                    var newCommand = Console.ReadLine();

                    if (newCommand == "")
                        break;

                    Command command = new Command(newCommand, root);
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
