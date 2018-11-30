using Metaheuristics.Metaheuristics.Genetic.TTP1;
using Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1;
using System.Collections.Generic;

namespace Metaheuristics.Metaheuristics.Hybrids.GeneticThenSA
{
    public class GaThenSaTtp1
    {
        private readonly GaTtp1 Genetic;
        private readonly AnnealingTtp1 Annealing;
        private readonly Problem.Problem Problem;
        private readonly GaThenSATtp1Parameters Parameters;

        public GaThenSaTtp1(Problem.Problem problem, GaThenSATtp1Parameters parameters)
        {
            Problem = problem;
            Genetic = new GaTtp1(problem, parameters.GeneticParameters);

            var annealingParams = new AnnealingParameters
            {
                InitialTemperature = parameters.AnnealingParameters.InitialTemperature,
                NeighbourhoodSize = parameters.AnnealingParameters.NeighbourhoodSize,
                NumAlgorithmIterations = 1,
                NumAnnealingCycles = parameters.AnnealingParameters.NumAnnealingCycles,
                TemperaturePercentageDropPerCycle = parameters.AnnealingParameters.TemperaturePercentageDropPerCycle,
            };

            Parameters = parameters;
            Parameters.AnnealingParameters = annealingParams;

            Annealing = new AnnealingTtp1(problem, annealingParams);
        }

        public void Execute(Logger.Logger logger)
        {
            void logGeneration(int generation, double bestFitness, double averageFitness, double worstFitness)
            {
                logger.LogGeneticThenSaTtp1(generation, bestFitness, averageFitness, worstFitness, null, null);
            }

            void logOutro(List<double> bestFitnesses)
            {
                logger.LogOutro(bestFitnesses);
            }

            var finalPopulations = Genetic.Execute(logGeneration, logOutro);

            void logAnnealingCycle(int annealingCycle, double globalBestFitness, double bestFitness, double currentFitness)
            {
                logger.LogGeneticThenSaTtp1(annealingCycle, globalBestFitness, null, null, bestFitness, currentFitness);
            }

            foreach (var population in finalPopulations)
            {
                var bestFitness = double.MinValue;
                IIndividual bestIndiv = null;
                population.ForEach(indiv =>
                {
                    var fitness = Problem.Fitness(indiv);
                    if (fitness > bestFitness)
                    {
                        bestFitness = fitness;
                        bestIndiv = indiv;
                    }
                });

                if (bestIndiv == null)
                {
                    continue;
                }

                Annealing.Execute(logAnnealingCycle, logOutro, bestIndiv, Parameters.GeneticParameters.NumGenerations);
            }

        }

    }
}