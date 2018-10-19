using System;
using System.Collections.Generic;
using System.Linq;
using Metaheuristics.GA;

namespace Metaheuristics.Problem
{
    public abstract class Problem
    {
        protected ProblemStats Stats { get; }
        public List<int> CityIds { get; }
        private Dictionary<CityCity, double> InterCityDistances { get; }
        protected List<Item> Items { get; }

        public abstract double Fitness(IIndividual indiv, List<Item> itemsTaken = null);
        protected abstract double ItemsProfit(IEnumerable<Item> itemsTaken);
        protected abstract double TravelTime(IIndividual indiv, List<Item> itemsTaken);

        protected Problem(ProblemStats stats, IReadOnlyCollection<City> cities, List<Item> items)
        {
            Stats = stats;
            CityIds = cities.Select(city => city.Id).ToList();
            InterCityDistances = BuildInterCityDistances(cities);
            Items = items;
        }

        protected Problem(ProblemStats stats, List<int> cityIds, Dictionary<CityCity, double> interCityDistances,
            List<Item> items)
        {
            Stats = stats;
            CityIds = cityIds;
            InterCityDistances = interCityDistances;
            Items = items;
        }
        
        protected double InterCityDistance(int city1Id, int city2Id)
        {
            return InterCityDistances[new CityCity {City1Id = city1Id, City2Id = city2Id}];
        }

        private static Dictionary<CityCity, double> BuildInterCityDistances(IReadOnlyCollection<City> cities)
        {
            var cityCityDistance = new Dictionary<CityCity, double>();

            var cityCityDistanceTuples = (from city in cities
                from otherCity in cities
                where city.Id != otherCity.Id
                select new Tuple<int, int, double>(city.Id, otherCity.Id, City.Distance(city, otherCity))).ToList();

            foreach (var cityCityDistanceTuple in cityCityDistanceTuples)
            {
                cityCityDistance[
                        new CityCity {City1Id = cityCityDistanceTuple.Item1, City2Id = cityCityDistanceTuple.Item2}] =
                    cityCityDistanceTuple.Item3;
            }

            return cityCityDistance;
        }
    }
}