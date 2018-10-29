using System;
using Metaheuristics.Algorithms.Genetic.TTP1;
using Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1;
using Metaheuristics.Metaheuristics.TabuSearch.Ttp1;

namespace Metaheuristics
{
    internal static class Program
    {
        private static void Main()
        {
            var algorithmType = Environment.GetEnvironmentVariable("METAHEURISTIC_TYPE");
            var problemSrcFilePath = Environment.GetEnvironmentVariable("METAHEURISTICS_PROBLEM_SRC_FILE");
            var algorithmSrcFilePath = Environment.GetEnvironmentVariable("METAHEURISTICS_ALGORITHM_SRC_FILE");
            var outputFilePath = Environment.GetEnvironmentVariable("METAHEURISTICS_LOG_OUTPUT_FILE");

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
            
            switch (algorithmType)
            {
                case "TABU_SEARCH_TTP1":
                    TabuSearchTtp1(problem, algorithmSrcFilePath, outputFilePath);
                    break;
                case "GENETIC_TTP1":
                    GeneticTtp1(problem, algorithmSrcFilePath, outputFilePath);
                    break;
                case "ANNEALING_TTP1":
                    SimulatedAnnealingTtp1(problem, algorithmSrcFilePath, outputFilePath);
                    break;
                default:
                    Console.WriteLine(
                        "Please set environment variable METAHEURISTIC_TYPE to be either 'TABU_SEARCH_TTP1' or 'GENETIC_TTP1'.");
                    return;
            }
        }

        private static void SimulatedAnnealingTtp1(Problem.Problem problem, string algorithmSrcFilePath, string outputFilePath)
        {
            var algorithmParams = Loader.Loader.LoadAnnealingTtp1Params(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading annealing ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading annealing ttp1's configuration src file.");

            var logger = new Logger.Logger(outputFilePath);

            try
            {
                logger.LogAnnealingTtp1Intro(algorithmParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error writing to annealing ttp1's log output file.");
                return;
            }

            var algorithm = new AnnealingTtp1(problem, algorithmParams);
            algorithm.Execute(logger);
        }

        private static void TabuSearchTtp1(Problem.Problem problem, string algorithmSrcFilePath, string outputFilePath)
        {
            var algorithmParams = Loader.Loader.LoadTabuTtp1Params(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading tabu ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading tabu ttp1's configuration src file.");

            var logger = new Logger.Logger(outputFilePath);

            try
            {
                logger.LogTabuTtp1Intro(algorithmParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error writing to tabu ttp1's log output file.");
                return;
            }

            var algorithm = new TabuTtp1(problem, algorithmParams);
            algorithm.Execute(logger);
        }

        private static void GeneticTtp1(Problem.Problem problem, string algorithmSrcFilePath, string outputFilePath)
        {
            var algorithmParams = Loader.Loader.LoadGeneticTtp1Params(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading genetic ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading genetic ttp1's configuration src file.");

            var logger = new Logger.Logger(outputFilePath);

            try
            {
                logger.LogGeneticTtp1Intro(algorithmParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error writing to the log output file.");
                return;
            }

            var algorithm = new GaTtp1(problem, algorithmParams);
            algorithm.Execute(logger);
        }
    }
}