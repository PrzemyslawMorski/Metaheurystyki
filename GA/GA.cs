namespace Metaheuristics.GA
{
    public class GA
    {
        public Problem.Problem Problem { get; }
        
        public GA(Problem.Problem problem)
        {
            Problem = problem;
        }
        
        public double FitnessTT1(Individual indiv)
        {
            return ItemsValueTT1(indiv) - Problem.Stats.KnapsackRentingRatio * TravelTimeTT1(indiv);
        }
        
        public double FitnessTT2(Individual indiv)
        {
            //TODO
            return 0d;
        }

        private double ItemsValueTT1(Individual indiv)
        {
            //TODO
            return 0d;
        }

        private double TravelTimeTT1(Individual indiv)
        {
            //TODO
            return 0d;
        }

        public Individual[] Selection(Individual[] currentPopulation)
        {
            //TODO
            return null;
        }
    }
}