using System.Collections.Generic;
using Metaheuristics.Metaheuristics;

namespace Metaheuristics.Problem.TTP2
{
    public class ProblemTtp2 : Problem
    {
        public ProblemTtp2(ProblemStats stats, IReadOnlyCollection<City> cities, List<Item> items) : base(stats, cities, items)
        {
        }

        public ProblemTtp2(ProblemStats stats, List<int> cityIds, Dictionary<CityCity, double> interCityDistances, List<Item> items) : base(stats, cityIds, interCityDistances, items)
        {
        }

        public override double Fitness(IIndividual indiv, List<Item> itemsTaken = null)
        {
            throw new System.NotImplementedException();
        }

        protected override double ItemsProfit(IEnumerable<Item> itemsTaken)
        {
            throw new System.NotImplementedException();
        }

        protected override double TravelTime(IIndividual indiv, List<Item> itemsTaken)
        {
            throw new System.NotImplementedException();
        }
    }
}