using System;
using System.Collections.Generic;
using Metaheuristics.Metaheuristics;
using Metaheuristics.Problem;
using Metaheuristics.Problem.TTP1;
using Xunit;

namespace Tests
{
    public class Ttp1
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

            var interCityDistances = new Dictionary<CityCity, double>
            {
                {new CityCity(1, 2), 5},
                {new CityCity(1, 3), 6},
                {new CityCity(1, 4), 6},
                {new CityCity(2, 1), 5},
                {new CityCity(2, 3), 6},
                {new CityCity(2, 4), 6},
                {new CityCity(3, 1), 6},
                {new CityCity(3, 2), 5},
                {new CityCity(3, 4), 4},
                {new CityCity(4, 1), 6},
                {new CityCity(4, 2), 6},
                {new CityCity(4, 3), 4}
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

            var problemTtp1 = new ProblemTtp1(problemStats, cities, interCityDistances, items);

            var individual = new Ttp1Individual
            {
                RoadTaken = new List<int> {1, 3, 2, 4}
            };

            var itemsTaken = new List<Item>
            {
                new Item {Id = 2, Profit = 40, Weight = 1, AssignedCityId = 3},
                new Item {Id = 4, Profit = 20, Weight = 2, AssignedCityId = 2}
            };

            Assert.Equal(-73.14, Math.Round(problemTtp1.Fitness(individual, itemsTaken), 2));
        }
    }
}