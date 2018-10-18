using System;
using System.Collections.Generic;
using System.Linq;
using Metaheuristics.GA;

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
        public struct CityCity
        {
            public int City1Id { get; set; }
            public int City2Id { get; set; }

            public CityCity(int city1Id, int city2Id)
            {
                City1Id = city1Id;
                City2Id = city2Id;
            }
        }

        private ProblemStats Stats { get; }
        public List<int> CityIds { get; }
        private Dictionary<CityCity, double> InterCityDistances { get; }
        private List<Item> Items { get; }
        private List<Item> GreedyBestItems { get; }
        private double GreedyBestItemsProfit { get; }

        public Problem(ProblemStats stats, IReadOnlyCollection<City> cities, List<Item> items)
        {
            Stats = stats;
            CityIds = cities.Select(city => city.Id).ToList();
            InterCityDistances = BuildInterCityDistances(cities);
            Items = items;
            GreedyBestItems = GreedyKnp();
            GreedyBestItemsProfit = ItemsProfitTtp1(GreedyBestItems);
        }

        public Problem(ProblemStats stats, List<int> cityIds, Dictionary<CityCity, double> interCityDistances,
            List<Item> items)
        {
            Stats = stats;
            CityIds = cityIds;
            InterCityDistances = interCityDistances;
            Items = items;
            GreedyBestItems = GreedyKnp();
            GreedyBestItemsProfit = ItemsProfitTtp1(GreedyBestItems);
        }

        private double InterCityDistance(int city1Id, int city2Id)
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

        public double FitnessTtp1(Individual indiv, List<Item> itemsTaken = null)
        {
            if (itemsTaken == null)
            {
                return GreedyBestItemsProfit - Stats.KnapsackRentingRatio * TravelTimeTtp1(indiv, GreedyBestItems);
            }

            return ItemsProfitTtp1(itemsTaken) - Stats.KnapsackRentingRatio * TravelTimeTtp1(indiv, itemsTaken);
        }

        private List<Item> GreedyKnp()
        {
            var bestItems = new List<Item>();
            var currentCapacity = 0;

            var itemsSortedByGreedyCriterium = new List<Item>(Items);
            itemsSortedByGreedyCriterium.Sort();

            foreach (var item in itemsSortedByGreedyCriterium)
            {
                if (currentCapacity + item.Weight > Stats.KnapsackCapacity)
                {
                    continue;
                }

                bestItems.Add(item);
                currentCapacity += item.Weight;
            }

            return bestItems;
        }


        private static double ItemsProfitTtp1(IEnumerable<Item> itemsTaken)
        {
            return itemsTaken.Sum(item => item.Profit);
        }

        private double TravelTimeTtp1(Individual indiv, List<Item> itemsTaken)
        {
            var totalTravelTime = 0d;
            var currentKnapsackWeight = 0d;

            for (var i = 0; i < indiv.RoadTaken.Count; i++)
            {
                var currentCityId = indiv.RoadTaken[i];

                var lastCityOnTheRoad = i + 1 == indiv.RoadTaken.Count;
                var nextCityId = lastCityOnTheRoad ? indiv.RoadTaken[0] : indiv.RoadTaken[i + 1];

                //PICK UP
                currentKnapsackWeight = itemsTaken
                    .Where(item => item.AssignedCityId == currentCityId)
                    .Aggregate(currentKnapsackWeight, (current, item) => current + item.Weight);

                //TRAVEL
                var speed = Stats.MaxSpeed -
                            currentKnapsackWeight * ((Stats.MaxSpeed - Stats.MinSpeed) / Stats.KnapsackCapacity);
                var distance = InterCityDistance(currentCityId, nextCityId);
                var travelTime = distance / speed;

                totalTravelTime += travelTime;
            }

            return totalTravelTime;
        }
    }
}