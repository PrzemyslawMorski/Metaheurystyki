using System;
using System.Collections.Generic;

namespace Metaheuristics.GA
{
    public class GaParameters
    {
        public int PopulationSize { get; }
        public int NumGenerations { get; }
        public double MutationProbability { get; }
        public double CrossProbability { get; }

        public GaParameters(int populationSize, int numGenerations, double mutationProbability, double crossProbability)
        {
            PopulationSize = populationSize;
            NumGenerations = numGenerations;
            MutationProbability = mutationProbability;
            CrossProbability = crossProbability;
        }
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

        private List<Individual> InitializePopulation(IReadOnlyCollection<int> cityIds)
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

                var individual = new Individual()
                {
                    RoadTaken = randomRoadTaken
                };

                population.Add(individual);
            }

            return population;
        }

        private List<Individual> Selection(List<Individual> currentPopulation)
        {
            //TODO
            return null;
        }

        private bool ShouldMutate()
        {
            return RandomNumGenerator.NextDouble() <= Parameters.MutationProbability;
        }

        private bool ShouldCross()
        {
            return RandomNumGenerator.NextDouble() <= Parameters.CrossProbability;
        }

        private Individual Mutate(Individual indiv)
        {
            if (!ShouldMutate()) return indiv;

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

        private Tuple<Individual, Individual> Cross(Individual indiv1, Individual indiv2)
        {
            var indiv1Child = indiv1.DeepCopy();
            var indiv2Child = indiv2.DeepCopy();
            var crossedIndivs = new Tuple<Individual, Individual>(indiv1Child, indiv2Child);

            if (ShouldCross())
            {
                CrossPmx(indiv1Child, indiv2Child);
            }

            return crossedIndivs;
        }

        private static void CrossPmx(Individual indiv1, Individual indiv2)
        {
            //TODO CROSSING
        }
    }
}