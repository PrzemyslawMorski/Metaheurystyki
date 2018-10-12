using System;
using System.Collections.Generic;

namespace Metaheuristics.GA
{
    public class GaParameters
    {
        public int PopulationSize { get; }
        public double MutationProbability { get; }
        public double CrossProbability { get; }

        public GaParameters(int populationSize, double mutationProbability, double crossProbability)
        {
            PopulationSize = populationSize;
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

        private List<Individual> InitializePopulation()
        {
            //TODO
            return null;
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
            for (var geneIndex = 0; geneIndex < indiv.RoadTaken.Count; geneIndex++)
            {
                if (ShouldMutate())
                {
                    MutateSwap(indiv.RoadTaken, geneIndex);
                }
            }

            return indiv;
        }

        private void MutateSwap(IList<int> roadTaken, int geneIndex)
        {
            var randomSwapIndex = RandomNumGenerator.Next(0, roadTaken.Count);
            while (randomSwapIndex == geneIndex)
            {
                randomSwapIndex = RandomNumGenerator.Next(0, roadTaken.Count);
            }

            var tmp = roadTaken[geneIndex];
            roadTaken[geneIndex] = roadTaken[randomSwapIndex];
            roadTaken[randomSwapIndex] = tmp;
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