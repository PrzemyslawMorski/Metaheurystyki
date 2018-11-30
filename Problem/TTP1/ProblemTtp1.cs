using System;
using System.Collections.Generic;
using System.Linq;
using Metaheuristics.Metaheuristics;

namespace Metaheuristics.Problem.TTP1
{
    public class ProblemTtp1 : Problem
    {
        private List<Item> GreedyBestItems { get; }
        private double GreedyBestItemsProfit { get; }
        public static int _numFitnessCalls = 0;

        public ProblemTtp1(ProblemStats stats, IReadOnlyCollection<City> cities, List<Item> items) : base(stats, cities,
            items)
        {
            GreedyBestItems = GreedyKnp();
            GreedyBestItemsProfit = ItemsProfit(GreedyBestItems);
        }

        public ProblemTtp1(ProblemStats stats, List<int> cityIds, Dictionary<CityCity, double> interCityDistances,
            List<Item> items) : base(stats, cityIds, interCityDistances, items)
        {
            GreedyBestItems = GreedyKnp();
            GreedyBestItemsProfit = ItemsProfit(GreedyBestItems);
        }

        public override double Fitness(IIndividual indiv, List<Item> itemsTaken = null)
        {
            _numFitnessCalls++;

            if (itemsTaken == null)
            {
                return GreedyBestItemsProfit - Stats.KnapsackRentingRatio * TravelTime(indiv, GreedyBestItems);
            }

            return ItemsProfit(itemsTaken) - Stats.KnapsackRentingRatio * TravelTime(indiv, itemsTaken);
        }

        private List<Item> GreedyKnp()
        {
            var bestItems = new List<Item>();
            var currentCapacity = 0;

            var itemsSortedDescendingByGreedyCriterium = new List<Item>(Items);
            itemsSortedDescendingByGreedyCriterium.Sort((c1, c2) =>
                c2.Profit / c2.Weight - c1.Profit / c1.Weight); // descending according to Profit/Weight

            foreach (var item in itemsSortedDescendingByGreedyCriterium)
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

        protected sealed override double ItemsProfit(IEnumerable<Item> itemsTaken)
        {
            return itemsTaken.Sum(item => item.Profit);
        }

        protected override double TravelTime(IIndividual indiv, List<Item> itemsTaken)
        {
            var totalTravelTime = 0d;
            var currentKnapsackWeight = 0d;

            if (!(indiv is Ttp1Individual indivTtp1))
            {
                return 0d;
            }

            for (var i = 0; i < indivTtp1.RoadTaken.Count; i++)
            {
                var currentCityId = indivTtp1.RoadTaken[i];

                var lastCityOnTheRoad = i + 1 == indivTtp1.RoadTaken.Count;
                var nextCityId = lastCityOnTheRoad ? indivTtp1.RoadTaken[0] : indivTtp1.RoadTaken[i + 1];

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