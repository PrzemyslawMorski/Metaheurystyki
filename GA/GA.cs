using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.GA
{
    public class GaParameters
    {
        public int NumAlgorithmIterations { get; set; }
        public int PopulationSize { get; set; }
        public int NumGenerations { get; set; }
        public double MutationProbability { get; set; }
        public double CrossProbability { get; set; }
        public int TournamentSize { get; set; }
    }

    public class Ga
    {
        private Problem.Problem Problem { get; }
        private GaParameters Parameters { get; }
        private Random RandomNumGenerator { get; }


        public Ga(Problem.Problem problem, GaParameters parameters)
        {
            Problem = problem;
            Parameters = parameters;
            RandomNumGenerator = new Random();
        }

        public void Execute(Logger.Logger logger)
        {
            for (var i = 0; i < Parameters.NumAlgorithmIterations; i++)
            {
                var generation = 0;
                var population = InitializePopulation(Problem.CityIds).ToList();

                EvaluatePopulation(population, generation, logger);

                while (generation < Parameters.NumGenerations)
                {
                    Console.WriteLine($"Iteration {i} Generation {generation}");

                    population = Evolve(population);

                    EvaluatePopulation(population, generation, logger);

                    generation++;
                }
            }
        }

        private List<Individual> Evolve(IList<Individual> population)
        {
            var selectedIndividuals = SelectForCrossing(population);
            var crossedIndividuals = Cross(selectedIndividuals);
            var mutatedIndividuals = Mutate(crossedIndividuals);
            return mutatedIndividuals;
        }

        private IEnumerable<Individual> InitializePopulation(IReadOnlyCollection<int> cityIds)
        {
            var population = new List<Individual>();
            var numCities = cityIds.Count;
            for (var i = 0; i < Parameters.PopulationSize; i++)
            {
                var randomRoadTaken = new List<int>(numCities);

                var mutableListCityIds = new List<int>(cityIds);

                for (var j = 0; j < numCities; j++)
                {
                    var randomIndex = RandomNumGenerator.Next(0, mutableListCityIds.Count);
                    var randomCityId = mutableListCityIds[randomIndex];
                    mutableListCityIds.RemoveAt(randomIndex);
                    randomRoadTaken.Add(randomCityId);
                }

                var individual = new Individual
                {
                    RoadTaken = randomRoadTaken
                };

                population.Add(individual);
            }

            return population;
        }

        private void EvaluatePopulation(IEnumerable<Individual> population, int generation, Logger.Logger logger)
        {
            var populationsFitness = population.Select(individual => Problem.FitnessTtp1(individual)).ToArray();

            var bestFitness = populationsFitness.Max();
            var avgFitness = populationsFitness.Average();
            var worstFitness = populationsFitness.Min();

            logger.LogPopulationStats(generation, bestFitness, avgFitness, worstFitness);
        }

        private IEnumerable<Tuple<Individual, Individual>> SelectForCrossing(IList<Individual> currentPopulation)
        {
            var numPairs = currentPopulation.Count / 2;
            var resultingPairs = new List<Tuple<Individual, Individual>>();

            for (var i = 0; i < numPairs; i++)
            {
                var firstIndiv = TournamentSelect(currentPopulation);
                var secondIndiv = TournamentSelect(currentPopulation);

                while (secondIndiv.RoadTaken.Equals(firstIndiv.RoadTaken))
                {
                    secondIndiv = currentPopulation[RandomNumGenerator.Next(0, currentPopulation.Count)];
                }

                resultingPairs.Add(new Tuple<Individual, Individual>(firstIndiv, secondIndiv));
            }

            return resultingPairs;
        }

        private Individual TournamentSelect(IList<Individual> population)
        {
            var bestIndiv = population[RandomNumGenerator.Next(0, population.Count)];
            var bestFitness = Problem.FitnessTtp1(bestIndiv);

            for (var i = 0; i < Parameters.TournamentSize; i++)
            {
                var randomIndiv = population[RandomNumGenerator.Next(0, population.Count)];

                if (randomIndiv.RoadTaken.Equals(bestIndiv.RoadTaken))
                {
                    randomIndiv = population[RandomNumGenerator.Next(0, population.Count)];
                }

                var randomIndivFitness = Problem.FitnessTtp1(randomIndiv);

                if (randomIndivFitness <= bestFitness) continue;

                bestIndiv = randomIndiv;
                bestFitness = randomIndivFitness;
            }

            return bestIndiv;
        }

        private bool ShouldMutate()
        {
            return RandomNumGenerator.NextDouble() <= Parameters.MutationProbability;
        }

        private bool ShouldCross()
        {
            return RandomNumGenerator.NextDouble() <= Parameters.CrossProbability;
        }

        private List<Individual> Mutate(IEnumerable<Tuple<Individual, Individual>> crossedIndividuals)
        {
            var mutatedPopulation = new List<Individual>();
            foreach (var tupleOfIndividuals in crossedIndividuals)
            {
                var firstMutated = MutateIndividual(tupleOfIndividuals.Item1);
                var secondMutated = MutateIndividual(tupleOfIndividuals.Item2);

                while (firstMutated.RoadTaken.Equals(secondMutated.RoadTaken))
                {
                    secondMutated = MutateIndividual(secondMutated, true);
                }

                mutatedPopulation.Add(firstMutated);
                mutatedPopulation.Add(secondMutated);
            }

            return mutatedPopulation;
        }

        private Individual MutateIndividual(Individual indiv, bool guaranteedMutation = false)
        {
            if (!guaranteedMutation && !ShouldMutate()) return indiv;

            var firstRandomSwapIndex = RandomNumGenerator.Next(0, indiv.RoadTaken.Count);
            var secondRandomSwapIndex = RandomNumGenerator.Next(0, indiv.RoadTaken.Count);

            while (firstRandomSwapIndex == secondRandomSwapIndex)
            {
                secondRandomSwapIndex = RandomNumGenerator.Next(0, indiv.RoadTaken.Count);
            }

            var tmp = indiv.RoadTaken[firstRandomSwapIndex];
            indiv.RoadTaken[firstRandomSwapIndex] = indiv.RoadTaken[secondRandomSwapIndex];
            indiv.RoadTaken[secondRandomSwapIndex] = tmp;

            return indiv;
        }

        private IEnumerable<Tuple<Individual, Individual>> Cross(
            IEnumerable<Tuple<Individual, Individual>> individualsToBeCrossed)
        {
            return individualsToBeCrossed
                .Select(tupleOfIndividuals => CrossIndividuals(tupleOfIndividuals.Item1, tupleOfIndividuals.Item2));
        }

        private Tuple<Individual, Individual> CrossIndividuals(Individual indiv1, Individual indiv2)
        {
            if (!ShouldCross()) return new Tuple<Individual, Individual>(indiv1, indiv2);

            var indiv1Child = CrossPmx(indiv1, indiv2);
            var indiv2Child = CrossPmx(indiv2, indiv1);

            return new Tuple<Individual, Individual>(indiv1Child, indiv2Child);
        }

        private Individual CrossPmx(Individual parent1, Individual parent2)
        {
            var child = parent1.DeepCopy();
            if (parent1.RoadTaken.Count != parent2.RoadTaken.Count) return child;

            var roadLength = parent1.RoadTaken.Count;
            var road1 = parent1.RoadTaken;
            var road2 = parent1.RoadTaken;

            var firstChildFirstCutIndex = RandomNumGenerator.Next(0, roadLength);
            var firstChildSecondCutIndex = RandomNumGenerator.Next(firstChildFirstCutIndex, roadLength);

            var cutValuesFromIndiv1 = road1.GetRange(firstChildFirstCutIndex, firstChildSecondCutIndex - firstChildFirstCutIndex);
            var cutValuesFromIndiv2 = road2.GetRange(firstChildFirstCutIndex, firstChildSecondCutIndex - firstChildFirstCutIndex)
                .Where(valueFromIndiv2 => !cutValuesFromIndiv1.Contains(valueFromIndiv2)).ToList();

            foreach (var value in cutValuesFromIndiv2)
            {
                var currentValueFromIndiv2 = value;
                var valueAtTheSamePositionInIndiv1 =
                    road1[road2.IndexOf(currentValueFromIndiv2)];
                var indexOfThatValueInIndiv2 = road2.IndexOf(valueAtTheSamePositionInIndiv1);

                while (indexOfThatValueInIndiv2 >= firstChildFirstCutIndex ||
                       indexOfThatValueInIndiv2 < firstChildSecondCutIndex)
                {
                    currentValueFromIndiv2 = road1[indexOfThatValueInIndiv2];
                    valueAtTheSamePositionInIndiv1 =
                        road1[road2.IndexOf(currentValueFromIndiv2)];
                    indexOfThatValueInIndiv2 = road2.IndexOf(valueAtTheSamePositionInIndiv1);
                }

                child.RoadTaken[indexOfThatValueInIndiv2] = value;
            }

            var valuesLeftInIndiv2 = road2.Where(value => !cutValuesFromIndiv1.Contains(value) &&
                                                                      !cutValuesFromIndiv2.Contains(value)).ToList();

            var indexInIndiv1 = 0;
            foreach (var valueLeftInIndiv2 in valuesLeftInIndiv2)
            {
                var insertedValue = false;

                while (indexInIndiv1 < roadLength && !insertedValue)
                {
                    var valueFromIndiv1 = child.RoadTaken[indexInIndiv1];
                    if (cutValuesFromIndiv1.Contains(valueFromIndiv1) || cutValuesFromIndiv2.Contains(valueFromIndiv1))
                    {
                        indexInIndiv1++;
                        continue;
                    }

                    child.RoadTaken[indexInIndiv1] = valueLeftInIndiv2;
                    indexInIndiv1++;
                    insertedValue = true;
                }
            }

            return child;
        }
    }
}