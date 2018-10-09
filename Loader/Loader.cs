using System;
using System.Collections.Generic;
using System.IO;
using Metaheurystyki.Problem;

namespace Metaheurystyki.Loader
{
    public static class Loader
    {
        private static string NumCitiesPrefix = "DIMENSION:";
        private static string NumItemsPrefix = "NUMBER OF ITEMS:";
        private static string KnapsackCapacityPrefix = "CAPACITY OF KNAPSACK:";
        private static string MaxSpeedPrefix = "MIN SPEED:";
        private static string MinSpeedPrefix = "MAX SPEED:";
        private static string KnapsackRentingRatioPrefix = "RENTING RATIO:";
        private static string CitiesFirstLine = "NODE_COORD_SECTION	(INDEX, X, Y):";
        private static string ItemsFirstLine = "ITEMS SECTION	(INDEX, PROFIT, WEIGHT, ASSIGNED NODE NUMBER):";
        
        public static Problem.Problem Load(string srcFilePath)
        {
            var srcFileLines = new List<string>();
            try
            {
                using (var sr = new StreamReader(srcFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            srcFileLines.Add(line);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file at location {srcFilePath} could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }

            return ExtractProblem(srcFileLines);
        }

        private static Problem.Problem ExtractProblem(List<string> srcFileLines)
        {
            return new Problem.Problem(
                ExtractProblemStats(srcFileLines),
                ExtractCities(srcFileLines),
                ExtractItems(srcFileLines)
            );
        }
        
        private static ProblemStats ExtractProblemStats(List<string> srcFileLines)
        {
            //TODO
            return new ProblemStats();
        }
        
        private static List<City> ExtractCities(List<string> srcFileLines)
        {
            //TODO
            return new List<City>();
        }
        
        private static List<Item> ExtractItems(List<string> srcFileLines)
        {
            //TODO
            return new List<Item>();
        }
    }
}