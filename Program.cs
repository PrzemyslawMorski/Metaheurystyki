using System;
using Metaheuristics.GA;

namespace Metaheuristics
{
    internal static class Program
    {
        private static void Main()
        {
            var problemSrcFilePath = Environment.GetEnvironmentVariable("METAHEURISTICS_PROBLEM_SRC_FILE", EnvironmentVariableTarget.User);
            var algorithmSrcFilePath = Environment.GetEnvironmentVariable("METAHEURISTICS_ALGORITHM_SRC_FILE", EnvironmentVariableTarget.User);
            var outputFilePath = Environment.GetEnvironmentVariable("METAHEURISTICS_LOG_OUTPUT_FILE", EnvironmentVariableTarget.User);


            if (problemSrcFilePath == null)
            {
                Console.WriteLine(
                    "Please set environment variable METAHEURISTICS_PROBLEM_SRC_FILE to be an absolute path to the problem's configuration file.");
                return;
            }

            if (algorithmSrcFilePath == null)
            {
                Console.WriteLine(
                    "Please set environment variable METAHEURISTICS_ALGORITHM_SRC_FILE to be an absolute path to the algorithm's configuration file.");
                return;
            }

            if (outputFilePath == null)
            {
                Console.WriteLine(
                    "Please set environment variable METAHEURISTICS_LOG_OUTPUT_FILE to be an absolute path to the program's log output file.");
                return;
            }

            Console.WriteLine($"Supplied problem's configuration src path: {problemSrcFilePath}");
            Console.WriteLine($"Supplied algorithm's configuration src path: {algorithmSrcFilePath}");
            Console.WriteLine($"Supplied log output path: {outputFilePath}");

            var problem = Loader.Loader.LoadProblem(problemSrcFilePath);
            if (problem == null)
            {
                Console.WriteLine("Error reading problem's src file.");
                return;
            }

            Console.WriteLine("Done reading problem's src file.");

            var algorithmParams = Loader.Loader.LoadAlgorithm(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading algorithm's src file.");
                return;
            }

            Console.WriteLine("Done reading algorithm's src file.");

            var logger = new Logger.Logger(outputFilePath);

            try
            {
                logger.LogOpeningStatsInfoAndPopulationStatsHeader(algorithmParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error writing to the log output file.");
                return;
            }

            var algorithm = new Ga(problem, algorithmParams);

            algorithm.Execute(logger);
        }
    }
}