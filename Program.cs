using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Metaheuristics.GA;

namespace Metaheuristics
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(
                    "Please supply an absolute path to the problem's configuration file and the algorithm's configuration file.");
            }
            else
            {
                Console.WriteLine($"Supplied problem's configuration src path: {args[0]}");
                Console.WriteLine($"Supplied algorithm's configuration src path: {args[1]}");

                var problem = Loader.Loader.LoadProblem(args[0]);
                if (problem == null)
                {
                    Console.WriteLine("Error reading problem's src file.");
                    return;
                }

                Console.WriteLine("Done reading problem's src file.");

                var algorithmParams = Loader.Loader.LoadAlgorithm(args[1]);
                if (algorithmParams == null)
                {
                    Console.WriteLine("Error reading algorithm's src file.");
                    return;
                }

                Console.WriteLine("Done reading algorithm's src file.");

                var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var inputFileFullName = args[0].Split("/").Last();
                var inputFileExtension = inputFileFullName.Split(".").Last();
                var inputFileShortName = inputFileFullName.Replace("." + inputFileExtension, "");
                var outputPath = executableLocation + "Output/" + inputFileShortName +
                                 DateTime.Now.ToLongDateString() + ".csv";

                var logger = new Logger.Logger(outputPath);
                var algorithm = new Ga(problem, algorithmParams);
                algorithm.Execute(logger);

                Console.Read();
            }
        }
    }
}