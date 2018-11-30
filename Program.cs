using Metaheuristics.Metaheuristics.Genetic.TTP1;
using Metaheuristics.Metaheuristics.Hybrids.GeneticPeriodicTabu;
using Metaheuristics.Metaheuristics.Hybrids.GeneticThenSA;
using Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1;
using Metaheuristics.Metaheuristics.TabuSearch.Ttp1;
using Metaheuristics.Problem.TTP1;
using System;
using System.Collections.Generic;

namespace Metaheuristics
{
    internal static class Program
    {
        private static void Main()
        {
            var algorithmType =
                Environment.GetEnvironmentVariable("METAHEURISTIC_TYPE", EnvironmentVariableTarget.User);
            var logOutputType =
                Environment.GetEnvironmentVariable("METAHEURISTIC_LOG_OUTPUT_TYPE", EnvironmentVariableTarget.User);
            var problemSrcFilePath =
                Environment.GetEnvironmentVariable("METAHEURISTICS_PROBLEM_SRC_FILE", EnvironmentVariableTarget.User);
            var algorithmSrcFilePath =
                Environment.GetEnvironmentVariable("METAHEURISTICS_ALGORITHM_SRC_FILE", EnvironmentVariableTarget.User);
            var outputFilePath =
                Environment.GetEnvironmentVariable("METAHEURISTICS_LOG_OUTPUT_FILE", EnvironmentVariableTarget.User);

            if (problemSrcFilePath == null)
            {
                Console.WriteLine(
                    "Please set environment variable METAHEURISTICS_PROBLEM_SRC_FILE to be an absolute path to the problem's configuration file.");
                return;
            }

            if (logOutputType == null)
            {
                Console.WriteLine(
                    "Please set environment variable LOG_OUTPUT_TYPE to be the type of log output you want: 'ALGORITHM' or 'FINAL_SOLUTION'");
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
                    TabuSearchTtp1(problem, algorithmSrcFilePath, outputFilePath, logOutputType);
                    break;
                case "GENETIC_TTP1":
                    GeneticTtp1(problem, algorithmSrcFilePath, outputFilePath, logOutputType);
                    break;
                case "ANNEALING_TTP1":
                    SimulatedAnnealingTtp1(problem, algorithmSrcFilePath, outputFilePath, logOutputType);
                    break;
                case "GENETIC_THEN_ANNEALING_TTP1":
                    GeneticThenAnnealingTtp1(problem, algorithmSrcFilePath, outputFilePath, logOutputType);
                    break;
                case "GENETIC_PERIODIC_TABU_TTP1":
                    GeneticPeriodicTabuTtp1(problem, algorithmSrcFilePath, outputFilePath, logOutputType);
                    break;
                default:
                    Console.WriteLine(
                        "Unknown METAHEURISTIC_TYPE");
                    return;
            }

            Console.WriteLine("Num fitness calls: " + ProblemTtp1._numFitnessCalls);
        }

        private static void SimulatedAnnealingTtp1(Problem.Problem problem, string algorithmSrcFilePath,
            string outputFilePath, string logOutputType)
        {
            var algorithmParams = Loader.Loader.LoadAnnealingTtp1Params(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading annealing ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading annealing ttp1's configuration src file.");

            var logger = new Logger.Logger(outputFilePath, logOutputType);

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

            void logAnnealingCycle(int annealingCycle, double globalBestFitness, double bestFitness, double currentFitness)
            {
                logger.LogGeneticThenSaTtp1(annealingCycle, globalBestFitness, null, null, bestFitness, currentFitness);
            }

            void logOutro(List<double> bestFitnesses)
            {
                logger.LogOutro(bestFitnesses);
            }

            algorithm.Execute(logAnnealingCycle, logOutro);
        }

        private static void TabuSearchTtp1(Problem.Problem problem, string algorithmSrcFilePath, string outputFilePath,
            string logOutputType)
        {
            var algorithmParams = Loader.Loader.LoadTabuTtp1Params(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading tabu ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading tabu ttp1's configuration src file.");

            var logger = new Logger.Logger(outputFilePath, logOutputType);

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

            void logTabuSearch(int numTabuSearch, double bestFitness, double currentFitness)
            {
                logger.LogTabuTtp1Search(numTabuSearch, bestFitness, currentFitness);
            }

            void logOutro(List<double> bestFitnesses)
            {
                logger.LogOutro(bestFitnesses);
            }

            var algorithm = new TabuTtp1(problem, algorithmParams);
            algorithm.Execute(logTabuSearch, logOutro);
        }

        private static void GeneticTtp1(Problem.Problem problem, string algorithmSrcFilePath, string outputFilePath,
            string logOutputType)
        {
            var algorithmParams = Loader.Loader.LoadGeneticTtp1Params(algorithmSrcFilePath);
            if (algorithmParams == null)
            {
                Console.WriteLine("Error reading genetic ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading genetic ttp1's configuration src file.");

            var logger = new Logger.Logger(outputFilePath, logOutputType);

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

            void logGeneration(int generation, double bestFitness, double averageFitness, double worstFitness)
            {
                logger.LogGeneticTtp1Generation(generation, bestFitness, averageFitness, worstFitness);
            }

            void logOutro(List<double> bestFitnesses)
            {
                logger.LogOutro(bestFitnesses);
            }

            var algorithm = new GaTtp1(problem, algorithmParams);
            algorithm.Execute(logGeneration, logOutro);
        }

        private static void GeneticThenAnnealingTtp1(Problem.Problem problem, string algorithmSrcFilePath,
            string outputFilePath,
            string logOutputType)
        {
            var geneticTtp1Params = Loader.Loader.LoadGeneticTtp1Params(algorithmSrcFilePath);
            if (geneticTtp1Params == null)
            {
                Console.WriteLine("Error reading genetic ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading genetic ttp1's configuration src file.");

            var annealingTtp1Params = Loader.Loader.LoadAnnealingTtp1Params(algorithmSrcFilePath);
            if (annealingTtp1Params == null)
            {
                Console.WriteLine("Error reading annealing ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading annealing ttp1's configuration src file.");

            var hybridParams = new GaThenSATtp1Parameters
            {
                GeneticParameters = geneticTtp1Params,
                AnnealingParameters = annealingTtp1Params,
            };

            var logger = new Logger.Logger(outputFilePath, logOutputType);

            try
            {
                logger.LogGeneticThenSaTTp1Intro(hybridParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error writing to the log output file.");
                return;
            }

            var hybrid = new GaThenSaTtp1(problem, hybridParams);

            hybrid.Execute(logger);
        }

        private static void GeneticPeriodicTabuTtp1(Problem.Problem problem, string algorithmSrcFilePath,
            string outputFilePath,
            string logOutputType)
        {
            var geneticTtp1Params = Loader.Loader.LoadGeneticTtp1Params(algorithmSrcFilePath);
            if (geneticTtp1Params == null)
            {
                Console.WriteLine("Error reading genetic ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading genetic ttp1's configuration src file.");

            var tabuTtp1Params = Loader.Loader.LoadTabuTtp1Params(algorithmSrcFilePath);
            if (tabuTtp1Params == null)
            {
                Console.WriteLine("Error reading genetic ttp1's configuration src file.");
                return;
            }

            Console.WriteLine("Done reading genetic ttp1's configuration src file.");

            var hybridParams = new GeneticPeriodicTabuParameters
            {
                GeneticParameters = geneticTtp1Params,
                TabuParameters = tabuTtp1Params,
            };

            var logger = new Logger.Logger(outputFilePath, logOutputType);

            try
            {
                logger.LogGeneticPeriodicTabuIntro(hybridParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error writing to the log output file.");
                return;
            }

            var hybrid = new GeneticPeriodicTabu(problem, hybridParams);
            hybrid.Execute(logger);
        }
    }
}