using System.Collections.Generic;

namespace Metaheuristics.Problem
{
    public class ProblemStats
    {
        public int NumCities { get; set; }
        public int NumItems { get; set; }
        public int KnapsackCapacity { get; set; }
        public double MaxSpeed { get; set; }
        public double MinSpeed { get; set; }
        public double KnapsackRentingRatio { get; set; }
    }
    
    public class Problem
    {
        public ProblemStats Stats { get; }
        public List<City> Cities { get; }
        public List<Item> Items { get; }

        public Problem(ProblemStats stats, List<City> cities, List<Item> items)
        {
            Stats = stats;
            Cities = cities;
            Items = items;
        }
    }
}