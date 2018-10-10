using System;

namespace Metaheuristics
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please supply an absolute path to the problem's configuration file.");
            }
            else
            {
                Console.WriteLine($"Supplied argument: {args[0]}");
                var loadedProblem = Loader.Loader.Load(args[0]);
                Console.WriteLine("Done reading src file.");
            }
        }
    }
}