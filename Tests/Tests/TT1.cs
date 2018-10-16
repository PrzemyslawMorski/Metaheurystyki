using System;
using System.Collections.Generic;
using Metaheuristics.GA;
using Metaheuristics.Problem;
using Xunit;

namespace Tests
{
    public class TT1
    {
        [Fact]
        public void Fitness()
        {
            var problemStats = new ProblemStats
            {
                NumCities = 4,
                NumItems = 5,
                KnapsackCapacity = 3,
                KnapsackRentingRatio = 1,
                MaxSpeed = 1,
                MinSpeed = 0.1
            };

            var cities = new List<int>
            {
                1, 2, 3, 4
            };

            var interCityDistances = new List<Tuple<int, int, double>>
            {
                new Tuple<int, int, double>(1, 2, 5),
                new Tuple<int, int, double>(1, 3, 6),
                new Tuple<int, int, double>(1, 4, 6),
                new Tuple<int, int, double>(2, 1, 5),
                new Tuple<int, int, double>(2, 3, 6),
                new Tuple<int, int, double>(2, 4, 6),
                new Tuple<int, int, double>(3, 1, 6),
                new Tuple<int, int, double>(3, 2, 5),
                new Tuple<int, int, double>(3, 4, 4),
                new Tuple<int, int, double>(4, 1, 6),
                new Tuple<int, int, double>(4, 2, 6),
                new Tuple<int, int, double>(4, 3, 4)
            };

            var items = new List<Item>
            {
                new Item {Id = 1, Profit = 100, Weight = 3, AssignedCityId = 3},
                new Item {Id = 2, Profit = 40, Weight = 1, AssignedCityId = 3},
                new Item {Id = 3, Profit = 40, Weight = 1, AssignedCityId = 3},
                new Item {Id = 4, Profit = 20, Weight = 2, AssignedCityId = 2},
                new Item {Id = 5, Profit = 20, Weight = 2, AssignedCityId = 4},
                new Item {Id = 6, Profit = 30, Weight = 3, AssignedCityId = 2}
            };

            var program = new Problem(problemStats, cities, interCityDistances, items);

            var individual = new Individual
            {
                RoadTaken = new List<int> {1, 3, 2, 4}
            };

            var itemsTaken = new Dictionary<int, bool>
            {
                {1, false},
                {2, true},
                {3, false},
                {4, true},
                {5, false},
                {6, false}
            };

            Assert.Equal(-73.14, Math.Round(program.FitnessTtp1(individual), 2));
        }
    }
}