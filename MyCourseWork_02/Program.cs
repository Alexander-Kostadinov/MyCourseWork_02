using System;

namespace MyCourseWork_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path of the file:");

            var path = Console.ReadLine();
            var file = System.IO.File.ReadAllText(path);

            Console.WriteLine();
        }
    }
}
