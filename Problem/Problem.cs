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
        public ProblemStats Stats { get; }
        public List<City> Cities { get; }
        public List<Item> Items { get; }

        public Problem(ProblemStats stats, List<City> cities, List<Item> items)
        {
            Stats = stats;
            Cities = cities;
            Items = items;
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
                var currentCity = Cities.Find(city => city.Id == indiv.RoadTaken[i]);

                var lastCityOnTheRoad = i + 1 == indiv.RoadTaken.Count;
                
                var nextCityId = lastCityOnTheRoad ? indiv.RoadTaken[0] : indiv.RoadTaken[i + 1];
                var nextCity = Cities.Find(city => city.Id == nextCityId);
                
                var itemsPickedUpInCurrentCity = itemsPickedUp.Where(item => item.AssignedCityId == currentCity.Id);

                //PICK UP
                currentKnapsackWeight = itemsPickedUpInCurrentCity
                    .Aggregate(currentKnapsackWeight, (current, item) => current + item.Weight);

                //TRAVEL
                var speed = Stats.MaxSpeed -
                            currentKnapsackWeight * ((Stats.MaxSpeed - Stats.MinSpeed) / Stats.KnapsackCapacity);
                var travelTime = City.Distance(currentCity, nextCity) / speed;
                
                totalTravelTime += travelTime;
            }

            return totalTravelTime;
        }
    }
}