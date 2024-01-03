﻿using System;

namespace MyCourseWork_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path of the file:");

            try
            {
                HtmlElement html = null;
                var path = Console.ReadLine();
                var file = System.IO.File.ReadAllText(path);
                var htmlTree = new HtmlTreeBuilder(file);
                html = htmlTree.Root;

                while (true)
                {
                    var newCommand = Console.ReadLine();
                    Console.WriteLine();

                    if (newCommand == "")
                        break;

                    Command command = new Command(newCommand, html);
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
