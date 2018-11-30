using Metaheuristics.GA;
using Metaheuristics.Metaheuristics.Genetic.TTP1;
using Metaheuristics.Metaheuristics.TabuSearch.Ttp1;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.Metaheuristics.Hybrids.GeneticPeriodicTabu
{
    public class GeneticPeriodicTabu
    {
        private readonly GaTtp1 Genetic;
        private readonly TabuTtp1 Tabu;
        private readonly Problem.Problem Problem;
        private readonly GeneticPeriodicTabuParameters Parameters;

        public GeneticPeriodicTabu(Problem.Problem problem, GeneticPeriodicTabuParameters parameters)
        {
            Problem = problem;
            Genetic = new GaTtp1(problem, parameters.GeneticParameters);

            var tabuParameters = new TabuParameters
            {
                NeighbourhoodSize = parameters.TabuParameters.NeighbourhoodSize,
                NumAlgorithmIterations = 1,
                NumTabuSearches = parameters.TabuParameters.NumTabuSearches,
                TabuSize = parameters.TabuParameters.TabuSize,
            };

            Parameters = parameters;
            Parameters.TabuParameters = tabuParameters;

            Tabu = new TabuTtp1(problem, tabuParameters);
        }

        public void Execute(Logger.Logger logger)
        {
            void logGeneration(int generation, double bestFitness, double averageFitness, double worstFitness)
            {
                logger.LogGeneticPeriodicTabu(generation, bestFitness, averageFitness, worstFitness, null);
            }

            void logOutro(List<double> bestFitnesses)
            {
                logger.LogOutro(bestFitnesses);
            }

            void logTabuCycle(int tabuCycle, double bestFitness, double currentFitness)
            {
                logger.LogGeneticPeriodicTabu(tabuCycle, bestFitness, null, null, currentFitness);
            }

            var populations = new List<List<IIndividual>>();

            for(var i = 0; i < Parameters.GeneticParameters.NumAlgorithmIterations; i++)
            {
                populations.Add(Genetic.InitializePopulation().Select(indiv =>
                {
                    return Tabu.Execute(logTabuCycle, logOutro, indiv)[0];
                }).ToList());
            }

            Genetic.Execute(logGeneration, logOutro, Parameters.TabuParameters.NumTabuSearches, populations);
        }
    }
}