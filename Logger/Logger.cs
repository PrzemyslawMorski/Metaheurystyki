using Metaheuristics.Metaheuristics.Hybrids.GeneticPeriodicTabu;
using Metaheuristics.Metaheuristics.Hybrids.GeneticThenSA;
using Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1;
using Metaheuristics.Metaheuristics.TabuSearch.Ttp1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.Logger
{
    public class Logger
    {
        private string OutputPath { get; }
        private string LogOutputType { get; }

        private const string AlgorithmLogOutput = "ALGORITHM";
        private const string FinalSolutionLogOutput = "FINAL_SOLUTION";

        public delegate void LogGeneticTtp1GenerationDelegate(int generation, double bestFitness,
            double avgFitness, double worstFitness);

        public delegate void LogOutroDelegate(List<double> bestFitnessValues);

        public Logger(string outputPath, string logOutputType)
        {
            OutputPath = outputPath;
            LogOutputType = logOutputType;
        }

        public void LogGeneticTtp1Intro(GA.GaParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("NUMBER OF ITERATIONS:" + parameters.NumAlgorithmIterations);
                file.WriteLine("POPULATION SIZE:" + parameters.PopulationSize);
                file.WriteLine("NUMBER OF GENERATIONS:" + parameters.NumGenerations);
                file.WriteLine("MUTATION PROBABILITY:" + parameters.MutationProbability);
                file.WriteLine("CROSSING PROBABILITY:" + parameters.CrossProbability);
                file.WriteLine("TOURNAMENT SIZE:" + parameters.TournamentSize);
                if (LogOutputType == AlgorithmLogOutput)
                {
                    file.WriteLine("GENERATION;BEST FITNESS;AVERAGE FITNESS; WORST FITNESS;");
                }
            }
        }

        public void LogGeneticThenSaTTp1Intro(GaThenSATtp1Parameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("NUMBER OF ITERATIONS:" + parameters.GeneticParameters.NumAlgorithmIterations);
                file.WriteLine("POPULATION SIZE:" + parameters.GeneticParameters.PopulationSize);
                file.WriteLine("NUMBER OF GENERATIONS:" + parameters.GeneticParameters.NumGenerations);
                file.WriteLine("MUTATION PROBABILITY:" + parameters.GeneticParameters.MutationProbability);
                file.WriteLine("CROSSING PROBABILITY:" + parameters.GeneticParameters.CrossProbability);
                file.WriteLine("TOURNAMENT SIZE:" + parameters.GeneticParameters.TournamentSize);
                file.WriteLine("NEIGHBOURHOOD SIZE:" + parameters.AnnealingParameters.NeighbourhoodSize);
                file.WriteLine("INITIAL TEMPERATURE:" + parameters.AnnealingParameters.InitialTemperature);
                file.WriteLine("TEMPERATURE PERCENTAGE DROP PER ANNEALING CYCLE:" +
                               parameters.AnnealingParameters.TemperaturePercentageDropPerCycle);
                if (LogOutputType == AlgorithmLogOutput)
                {
                    file.WriteLine("GENERATION/ANNEALING CYCLE;BEST FITNESS;AVERAGE FITNESS; WORST FITNESS;" +
                        "GLOBAL BEST FITENSS; CURRENT FITNESS");
                }
            }
        }

        public void LogGeneticPeriodicTabuIntro(GeneticPeriodicTabuParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("NUMBER OF ITERATIONS:" + parameters.GeneticParameters.NumAlgorithmIterations);
                file.WriteLine("POPULATION SIZE:" + parameters.GeneticParameters.PopulationSize);
                file.WriteLine("NUMBER OF GENERATIONS:" + parameters.GeneticParameters.NumGenerations);
                file.WriteLine("MUTATION PROBABILITY:" + parameters.GeneticParameters.MutationProbability);
                file.WriteLine("CROSSING PROBABILITY:" + parameters.GeneticParameters.CrossProbability);
                file.WriteLine("TOURNAMENT SIZE:" + parameters.GeneticParameters.TournamentSize);
                file.WriteLine("NEIGHBOURHOOD SIZE:" + parameters.TabuParameters.NeighbourhoodSize);
                file.WriteLine("NUMBER OF TABU SEARCHES:" + parameters.TabuParameters.NumTabuSearches);
                file.WriteLine("TABU SIZE:" + parameters.TabuParameters.TabuSize);
                if (LogOutputType == AlgorithmLogOutput)
                {
                    file.WriteLine("GENERATION/TABU SEARCH;BEST FITNESS;AVERAGE FITNESS; WORST FITNESS; CURRENT FITNESS");
                }
            }
        }

        public void LogTabuTtp1Intro(TabuParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("TABU LIST SIZE:" + parameters.TabuSize);
                file.WriteLine("NEIGHBOURHOOD SIZE:" + parameters.NeighbourhoodSize);
                if (LogOutputType == AlgorithmLogOutput)
                {
                    file.WriteLine("NUM_SEARCH;BEST_FITNESS; CURRENT_FITNESS;");
                }
            }
        }

        public void LogAnnealingTtp1Intro(AnnealingParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("NEIGHBOURHOOD SIZE:" + parameters.NeighbourhoodSize);
                file.WriteLine("INITIAL TEMPERATURE:" + parameters.InitialTemperature);
                file.WriteLine("TEMPERATURE PERCENTAGE DROP PER ANNEALING CYCLE:" +
                               parameters.TemperaturePercentageDropPerCycle);
                if (LogOutputType == AlgorithmLogOutput)
                {
                    file.WriteLine("NUM_CYCLE;GLOBAL_BEST_FITNESS;BEST_FITNESS; CURRENT_FITNESS;TEMPERATURE;");
                }
            }
        }

        public void LogGeneticTtp1Generation(int generation, double bestFitness, double avgFitness, double worstFitness)
        {
            if (LogOutputType != AlgorithmLogOutput)
            {
                return;
            }

            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine($"{generation};{bestFitness};{avgFitness};{worstFitness};");
            }
        }

        public void LogTabuTtp1Search(int numSearch, double bestFitness, double currentFitness)
        {
            if (LogOutputType != AlgorithmLogOutput)
            {
                return;
            }

            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine($"{numSearch};{bestFitness};{currentFitness};");
            }
        }

        public void LogAnnealingTtp1Cycle(int numAnnealingCycles, double globalBestFitness, double bestFitness,
            double currentFitness, double temperature)
        {
            if (LogOutputType != AlgorithmLogOutput)
            {
                return;
            }

            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine(
                    $"{numAnnealingCycles};{globalBestFitness};{bestFitness};{currentFitness};{temperature}");
            }
        }

        public void LogGeneticThenSaTtp1(int numAnnealingCyclesOrGeneration, double bestFitness, double? averageFitness,
            double? worstFitness, double? globalBestFitness, double? currentFitness)
        {
            if (LogOutputType != AlgorithmLogOutput)
            {
                return;
            }

            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                var averageFitnessText = "";
                var worstFitnessText = "";
                var globalBestFitnessText = "";
                var currentFitnessText = "";

                if (averageFitness.HasValue)
                {
                    averageFitnessText = averageFitness.Value.ToString();
                }

                if (worstFitness.HasValue)
                {
                    worstFitnessText = worstFitness.Value.ToString();
                }

                if (globalBestFitness.HasValue)
                {
                    globalBestFitnessText = globalBestFitness.Value.ToString();
                }

                if (currentFitness.HasValue)
                {
                    currentFitnessText = currentFitness.Value.ToString();
                }

                file.WriteLine(
                    $"{numAnnealingCyclesOrGeneration};{bestFitness};{averageFitnessText};{worstFitnessText};" +
                    $"{globalBestFitnessText};{currentFitnessText}");
            }
        }

        public void LogGeneticPeriodicTabu(int numTabuSearchOrGeneration, double bestFitness, double? averageFitness,
    double? worstFitness, double? currentFitness)
        {
            if (LogOutputType != AlgorithmLogOutput)
            {
                return;
            }

            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                var averageFitnessText = "";
                var worstFitnessText = "";
                var currentFitnessText = "";

                if (averageFitness.HasValue)
                {
                    averageFitnessText = averageFitness.Value.ToString();
                }

                if (worstFitness.HasValue)
                {
                    worstFitnessText = worstFitness.Value.ToString();
                }

                if (currentFitness.HasValue)
                {
                    currentFitnessText = currentFitness.Value.ToString();
                }

                file.WriteLine(
                    $"{numTabuSearchOrGeneration};{bestFitness};{averageFitnessText};" +
                    $"{worstFitnessText};{currentFitnessText}");
            }
        }

        public void LogOutro(List<double> bestFitnessValues)
        {
            if (LogOutputType != FinalSolutionLogOutput)
            {
                return;
            }

            var finalBest = bestFitnessValues.Max();
            var avgBest = bestFitnessValues.Average();
            var worstBest = bestFitnessValues.Min();

            var standardDeviation =
                Math.Sqrt(bestFitnessValues.Sum(fitnessValue => Math.Pow(avgBest - fitnessValue, 2D)) /
                          (bestFitnessValues.Count - 1));

            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("BEST FINAL FITNESS; AVG FINAL FITNESS; STANDARD DEVIATION;WORST FINAL FITNESS;");
                file.WriteLine($"{finalBest};{avgBest};{standardDeviation};{worstBest};");
            }
        }
    }
}