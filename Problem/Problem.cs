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
        private ProblemStats Stats { get; }
        private List<int> CityIds { get; }
        private List<Tuple<int, int, double>> InterCityDistances { get; }
        private List<Item> Items { get; }

        public Problem(ProblemStats stats, IReadOnlyCollection<City> cities, List<Item> items)
        {
            Stats = stats;
            CityIds = cities.Select(city => city.Id).ToList();
            InterCityDistances = BuildInterCityDistances(cities);
            Items = items;
        }

        public Problem(ProblemStats stats, List<int> cityIds, List<Tuple<int, int, double>> interCityDistances,
            List<Item> items)
        {
            Stats = stats;
            CityIds = cityIds;
            InterCityDistances = interCityDistances;
            Items = items;
        }

        private double InterCityDistance(int city1Id, int city2Id)
        {
            return InterCityDistances.Find(interCityDistance =>
                interCityDistance.Item1 == city1Id && interCityDistance.Item2 == city2Id).Item3;
        }

        private static List<Tuple<int, int, double>> BuildInterCityDistances(IReadOnlyCollection<City> cities)
        {
            return (from city in cities
                from otherCity in cities
                where city.Id != otherCity.Id
                select new Tuple<int, int, double>(city.Id, otherCity.Id, City.Distance(city, otherCity))).ToList();
        }

        public double FitnessTT1(Individual indiv)
        {
            return ItemsProfitTT1(indiv) - Stats.KnapsackRentingRatio * TravelTimeTT1(indiv);
        }

        public double FitnessTT2(Individual indiv)
        {
            //TODO
            return 0d;
        }

        private double ItemsProfitTT1(Individual indiv)
        {
            var itemsOfThisIndiv = Items.Where(item => indiv.ItemsTaken[item.Id]);
            return itemsOfThisIndiv.Sum(item => item.Profit);
        }

        private double TravelTimeTT1(Individual indiv)
        {
            var totalTravelTime = 0d;
            var currentKnapsackWeight = 0d;
            var itemsPickedUp = Items.Where(item => indiv.ItemsTaken[item.Id]).ToArray();

            for (var i = 0; i < indiv.RoadTaken.Count; i++)
            {
                var currentCityId = indiv.RoadTaken[i];

                var lastCityOnTheRoad = i + 1 == indiv.RoadTaken.Count;
                var nextCityId = lastCityOnTheRoad ? indiv.RoadTaken[0] : indiv.RoadTaken[i + 1];

                var itemsPickedUpInCurrentCity = itemsPickedUp.Where(item => item.AssignedCityId == currentCityId);

                //PICK UP
                currentKnapsackWeight = itemsPickedUpInCurrentCity
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