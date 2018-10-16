using System;

namespace Metaheuristics.Logger
{
    public class Logger
    {
        private string OutputPath { get; }

        public Logger(string outputPath)
        {
            OutputPath = outputPath;

            using (new System.IO.StreamWriter(OutputPath, true))
            {
                Console.WriteLine($"Successfully created output file at {OutputPath}");
            }
        }

        public void LogOpeningStatsInfoAndPopulationStatsHeader(GA.GaParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("NUMBER OF ITERATIONS:" + parameters.NumAlgorithmIterations);
                file.WriteLine("POPULATION SIZE:" + parameters.PopulationSize);
                file.WriteLine("NUMBER OF GENERATIONS:" + parameters.NumGenerations);
                file.WriteLine("MUTATION PROBABILITY:" + parameters.MutationProbability);
                file.WriteLine("CROSSING PROBABILITY:" + parameters.CrossProbability);
                file.WriteLine("TOURNAMENT SIZE:" + parameters.TournamentSize);
                file.WriteLine("GENERATION;BEST FITNESS;AVERAGE FITNESS; WORST FITNESS;");
            }
        }

        public void LogPopulationStats(int generation, double bestFitness, double avgFitness, double worstFitness)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine($"{generation};{bestFitness};{avgFitness};{worstFitness};");
            }
        }
    }
}