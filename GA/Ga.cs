using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.GA
{
    public abstract class Ga
    {
        protected Ga(Problem.Problem problem, GaParameters parameters)
        {
            Problem = problem;
            Parameters = parameters;
            RandomNumGenerator = new Random();
        }

        protected Problem.Problem Problem { get; }
        protected GaParameters Parameters { get; }
        protected Random RandomNumGenerator { get; }

        public abstract void Execute(Logger.Logger logger);
        protected abstract IList<IIndividual> InitializePopulation(IList<int> cityIds);

        protected abstract IEnumerable<Tuple<IIndividual, IIndividual>> SelectForCrossing(
            IList<IIndividual> currentPopulation);

        protected abstract Tuple<IIndividual, IIndividual> CrossIndividuals(IIndividual indiv1, IIndividual indiv2);
        protected abstract IIndividual MutateIndividual(IIndividual indiv, bool guaranteedMutation = false);

        protected IList<IIndividual> Evolve(IList<IIndividual> population)
        {
            var selectedIndividuals = SelectForCrossing(population);
            var crossedIndividuals = Cross(selectedIndividuals);
            var mutatedIndividuals = Mutate(crossedIndividuals);
            return mutatedIndividuals;
        }

        protected void EvaluatePopulation(IEnumerable<IIndividual> population, int generation, Logger.Logger logger)
        {
            var populationsFitness = population.Select(individual => Problem.Fitness(individual)).ToArray();

            var bestFitness = populationsFitness.Max();
            var avgFitness = populationsFitness.Average();
            var worstFitness = populationsFitness.Min();

            logger.LogPopulationStats(generation, bestFitness, avgFitness, worstFitness);
        }

        protected bool ShouldMutate()
        {
            return RandomNumGenerator.NextDouble() <= Parameters.MutationProbability;
        }

        protected bool ShouldCross()
        {
            return RandomNumGenerator.NextDouble() <= Parameters.CrossProbability;
        }


        private IEnumerable<Tuple<IIndividual, IIndividual>> Cross(
            IEnumerable<Tuple<IIndividual, IIndividual>> individualsToBeCrossed)
        {
            return individualsToBeCrossed
                .Select(tupleOfIndividuals => CrossIndividuals(tupleOfIndividuals.Item1, tupleOfIndividuals.Item2))
                .ToList();
        }

        private List<IIndividual> Mutate(IEnumerable<Tuple<IIndividual, IIndividual>> crossedIndividuals)
        {
            var mutatedPopulation = new List<IIndividual>();
            foreach (var tupleOfIndividuals in crossedIndividuals)
            {
                var firstMutated = MutateIndividual(tupleOfIndividuals.Item1);
                var secondMutated = MutateIndividual(tupleOfIndividuals.Item2);

                while (firstMutated.Equals(secondMutated))
                {
                    secondMutated = MutateIndividual(secondMutated, true);
                }

                mutatedPopulation.Add(firstMutated);
                mutatedPopulation.Add(secondMutated);
            }

            return mutatedPopulation;
        }

        protected IIndividual TournamentSelect(IList<IIndividual> population)
        {
            var bestIndiv = population[RandomNumGenerator.Next(0, population.Count)];
            var bestFitness = Problem.Fitness(bestIndiv);

            for (var i = 0; i < Parameters.TournamentSize; i++)
            {
                var randomIndiv = population[RandomNumGenerator.Next(0, population.Count)];

                if (randomIndiv.Equals(bestIndiv))
                {
                    randomIndiv = population[RandomNumGenerator.Next(0, population.Count)];
                }

                var randomIndivFitness = Problem.Fitness(randomIndiv);

                if (randomIndivFitness <= bestFitness) continue;

                bestIndiv = randomIndiv;
                bestFitness = randomIndivFitness;
            }

            return bestIndiv;
        }
    }
}