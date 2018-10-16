using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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
            logger.LogOpeningStatsInfoAndPopulationStatsHeader(Parameters);

            for (var i = 0; i < Parameters.NumAlgorithmIterations; i++)
            {
                var generation = 0;
                var population = InitializePopulation(Problem.CityIds).ToList();

                EvaluatePopulation(population, generation, logger);

                while (generation < Parameters.NumGenerations)
                {
                    population = Evolve(population);

                    EvaluatePopulation(population, generation, logger);

                    generation++;
                }
            }
        }

        private List<Individual> Evolve(List<Individual> population)
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
            var populationsFitness = population.Select(individual => Problem.FitnessTT1(individual)).ToList();

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
            var bestFitness = Problem.FitnessTT1(bestIndiv);

            for (var i = 0; i < Parameters.TournamentSize; i++)
            {
                var randomIndiv = population[RandomNumGenerator.Next(0, population.Count)];

                if (randomIndiv.RoadTaken.Equals(bestIndiv.RoadTaken))
                {
                    randomIndiv = population[RandomNumGenerator.Next(0, population.Count)];
                }

                var randomIndivFitness = Problem.FitnessTT1(randomIndiv);

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
            var crossedIndividuals = new List<Tuple<Individual, Individual>>();

            foreach (var tupleOfIndividuals in individualsToBeCrossed)
            {
                CrossIndividuals(tupleOfIndividuals.Item1, tupleOfIndividuals.Item2, out var child1, out var child2);
                crossedIndividuals.Add(new Tuple<Individual, Individual>(child1, child2));
            }

            return crossedIndividuals;
        }

        private void CrossIndividuals(Individual indiv1, Individual indiv2, out Individual indiv1Child,
            out Individual indiv2Child)
        {
            indiv1Child = indiv1.DeepCopy();
            indiv2Child = indiv2.DeepCopy();

            if (!ShouldCross()) return;

            var roadLength = indiv1.RoadTaken.Count;
            var indiv1Road = new List<int>(indiv1.RoadTaken);
            var indiv2Road = new List<int>(indiv2.RoadTaken);

            CrossPmx(indiv1Child, roadLength, indiv1Road, indiv2Road);
            CrossPmx(indiv2Child, roadLength, indiv2Road, indiv1Road);
        }

        private void CrossPmx(Individual indiv, int roadLength, List<int> indiv1Road, List<int> indiv2Road)
        {
            var firstChildFirstCutIndex = RandomNumGenerator.Next(0, roadLength);
            var firstChildSecondCutIndex = RandomNumGenerator.Next(firstChildFirstCutIndex, roadLength);

            var cutValuesFromIndiv1 = indiv1Road.GetRange(firstChildFirstCutIndex, firstChildSecondCutIndex);
            var cutValuesFromIndiv2 = indiv2Road.GetRange(firstChildFirstCutIndex, firstChildSecondCutIndex)
                .Where(valueFromIndiv2 => !cutValuesFromIndiv1.Contains(valueFromIndiv2)).ToList();

            foreach (var value in cutValuesFromIndiv2)
            {
                var currentValueFromIndiv2 = value;
                var valueAtTheSamePositionInIndiv1 = indiv1Road[indiv2Road.IndexOf(currentValueFromIndiv2)];
                var indexOfThatValueInIndiv2 = indiv2Road.IndexOf(valueAtTheSamePositionInIndiv1);

                while (indexOfThatValueInIndiv2 >= firstChildFirstCutIndex ||
                       indexOfThatValueInIndiv2 < firstChildSecondCutIndex)
                {
                    currentValueFromIndiv2 = indiv1Road[indexOfThatValueInIndiv2];
                    valueAtTheSamePositionInIndiv1 = indiv1Road[indiv2Road.IndexOf(currentValueFromIndiv2)];
                    indexOfThatValueInIndiv2 = indiv2Road.IndexOf(valueAtTheSamePositionInIndiv1);
                }

                indiv.RoadTaken[indexOfThatValueInIndiv2] = value;
            }

            var valuesLeftInIndiv2 = indiv2Road.Where(value => !cutValuesFromIndiv1.Contains(value) &&
                                                                 !cutValuesFromIndiv2.Contains(value)).ToList();

            var indexInIndiv1 = 0;
            foreach (var valueLeftInIndiv2 in valuesLeftInIndiv2)
            {
                var insertedValue = false;

                while (indexInIndiv1 < roadLength && !insertedValue)
                {
                    var valueFromIndiv1 = indiv.RoadTaken[indexInIndiv1];
                    if (cutValuesFromIndiv1.Contains(valueFromIndiv1) || cutValuesFromIndiv2.Contains(valueFromIndiv1))
                    {
                        indexInIndiv1++;
                        continue;
                    }

                    indiv.RoadTaken[indexInIndiv1] = valueLeftInIndiv2;
                    indexInIndiv1++;
                    insertedValue = true;
                }
            }
        }
    }
}