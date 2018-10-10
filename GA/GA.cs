using System;
using System.Collections.Generic;

namespace Metaheuristics.GA
{
    public class GaParameters
    {
        public double MutationProbability { get; }
        public double CrossProbability { get; }

        public GaParameters(double mutationProbability, double crossProbability)
        {
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

        private Individual[] Selection(List<Individual> currentPopulation)
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

        public Individual Mutate(Individual indiv)
        {
            if (ShouldMutate())
            {
                //TODO MUTATION
            }

            return indiv;
        }

        public Tuple<Individual, Individual> Cross(Individual indiv1, Individual indiv2)
        {
            if (!ShouldCross()) return new Tuple<Individual, Individual>(indiv1, indiv2);

            var indiv1Child = indiv1.DeepCopy();
            var indiv2Child = indiv2.DeepCopy();

            //TODO CROSSING

            return new Tuple<Individual, Individual>(indiv1, indiv2);
        }
    }
}