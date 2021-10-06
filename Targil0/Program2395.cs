using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome2395();
            welcome0711();
            Console.ReadKey();
        }

        private static void welcome2395()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }

        static partial void welcome0711();
    }
}
