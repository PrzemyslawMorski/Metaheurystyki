using Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1;
using Metaheuristics.Metaheuristics.TabuSearch.Ttp1;

namespace Metaheuristics.Logger
{
    public class Logger
    {
        private string OutputPath { get; }

        public Logger(string outputPath)
        {
            OutputPath = outputPath;
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
                file.WriteLine("GENERATION;BEST FITNESS;AVERAGE FITNESS; WORST FITNESS;");
            }
        }

        public void LogTabuTtp1Intro(TabuParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("TABU LIST SIZE:" + parameters.TabuSize);
                file.WriteLine("NEIGHBOURHOOD SIZE:" + parameters.NeighbourhoodSize);
                file.WriteLine("NUM_SEARCH;BEST_FITNESS; CURRENT_FITNESS;");
            }
        }
        
        public void LogAnnealingTtp1Intro(AnnealingParameters parameters)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine("NEIGHBOURHOOD SIZE:" + parameters.NeighbourhoodSize);
                file.WriteLine("INITIAL TEMPERATURE:" + parameters.InitialTemperature);
                file.WriteLine("TEMPERATURE PERCENTAGE DROP PER ANNEALING CYCLE:" + parameters.TemperaturePercentageDropPerCycle);
                file.WriteLine("NUM_CYCLE;BEST_FITNESS; CURRENT_FITNESS;TEMPERATURE;");
            }
        }

        public void LogGeneticTtp1Generation(int generation, double bestFitness, double avgFitness, double worstFitness)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine($"{generation};{bestFitness};{avgFitness};{worstFitness};");
            }
        }

        public void LogTabuTtp1Search(int numSearch, double bestFitness, double currentFitness)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine($"{numSearch};{bestFitness};{currentFitness};");
            }
        }

        public void LogAnnealingTtp1Cycle(int numAnnealingCycles, double bestFitness, double currentFitness, double temperature)
        {
            using (var file = new System.IO.StreamWriter(OutputPath, true))
            {
                file.WriteLine($"{numAnnealingCycles};{bestFitness};{currentFitness};{temperature}");
            }
        }
    }
}