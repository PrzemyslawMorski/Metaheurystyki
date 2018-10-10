using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.GA
{
    public class GaParameters
    {
        public double MutationProbability {get;}
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
        
        public double FitnessTT1(Individual indiv)
        {
            return ItemsProfitTT1(indiv) - Problem.Stats.KnapsackRentingRatio * TravelTimeTT1(indiv);
        }
        
        public double FitnessTT2(Individual indiv)
        {
            //TODO
            return 0d;
        }

        private double ItemsProfitTT1(Individual indiv)
        {
            var itemsOfThisIndiv = Problem.Items.Where(item => indiv.ItemsTaken[item.Id]);
            return itemsOfThisIndiv.Sum(item => item.Profit);
        }

        private double TravelTimeTT1(Individual indiv)
        {
            //TODO
            return 0d;
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
            else
            {
                return indiv;
            }
            
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