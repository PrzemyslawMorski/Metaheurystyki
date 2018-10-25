using System;
using System.Collections.Generic;
using System.IO;
using Metaheuristics.GA;
using Metaheuristics.Metaheuristics.TabuSearch.Ttp1;
using Metaheuristics.Problem;
using Metaheuristics.Problem.TTP1;

namespace Metaheuristics.Loader
{
    public static class Loader
    {
        private const string NumCitiesPrefix = "DIMENSION:";
        private const string NumItemsPrefix = "NUMBER OF ITEMS:";
        private const string KnapsackCapacityPrefix = "CAPACITY OF KNAPSACK:";
        private const string MaxSpeedPrefix = "MIN SPEED:";
        private const string MinSpeedPrefix = "MAX SPEED:";
        private const string KnapsackRentingRatioPrefix = "RENTING RATIO:";
        private const string CitiesOpeningLineFormat = "NODE_COORD_SECTION	(INDEX, X, Y):";
        private const string ItemsOpeningLineFormat = "ITEMS SECTION	(INDEX, PROFIT, WEIGHT, ASSIGNED NODE NUMBER):";

        private const string NumAlgorithmIterationsPrefix = "NUMBER OF ITERATIONS:";
        private const string PopulationSizePrefix = "POPULATION SIZE:";
        private const string NumGenerationsPrefix = "NUMBER OF GENERATIONS:";
        private const string MutationProbabilityPrefix = "MUTATION PROBABILITY:";
        private const string CrossProbabilityPrefix = "CROSSING PROBABILITY:";
        private const string TournamentSizePrefix = "TOURNAMENT SIZE:";

        private const string NeighbourhoodSizePrefix = "NEIGHBOURHOOD SIZE:";
        private const string TabuListSizePrefix = "TABU LIST SIZE:";
        private const string NumTabuSearchesPrefix = "NUMBER OF TABU SEARCHES:";

        public static Problem.Problem LoadProblem(string srcFilePath)
        {
            return ExtractProblem(LoadSrcFileLines(srcFilePath));
        }

        private static List<string> LoadSrcFileLines(string srcFilePath)
        {
            Console.WriteLine($"Reading lines from {srcFilePath}");

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

            return srcFileLines;
        }

        public static GA.GaParameters LoadGeneticTtp1Params(string srcFilePath)
        {
            return ExtractGeneticTtp1Parameters(LoadSrcFileLines(srcFilePath));
        }

        private static GaParameters ExtractGeneticTtp1Parameters(List<string> srcFileLines)
        {
            try
            {
                var numAlgorithmIterationsLine =
                    srcFileLines.Find(line => line.StartsWith(NumAlgorithmIterationsPrefix));
                var populationSizeLine = srcFileLines.Find(line => line.StartsWith(PopulationSizePrefix));
                var numGenerationsLine = srcFileLines.Find(line => line.StartsWith(NumGenerationsPrefix));
                var mutationProbabilityLine = srcFileLines.Find(line => line.StartsWith(MutationProbabilityPrefix));
                var crossProbabilityLine = srcFileLines.Find(line => line.StartsWith(CrossProbabilityPrefix));
                var tournamentSizeLine = srcFileLines.Find(line => line.StartsWith(TournamentSizePrefix));

                if (numAlgorithmIterationsLine == null ||
                    populationSizeLine == null ||
                    numGenerationsLine == null ||
                    mutationProbabilityLine == null ||
                    crossProbabilityLine == null ||
                    tournamentSizeLine == null)
                {
                    Console.WriteLine($"Genetic ttp1 configuration file format error - first 6 lines need to be: " +
                                      $"\n {NumAlgorithmIterationsPrefix} INT" +
                                      $"\n {PopulationSizePrefix} INT" +
                                      $"\n {NumGenerationsPrefix} INT" +
                                      $"\n {MutationProbabilityPrefix} REAL" +
                                      $"\n {CrossProbabilityPrefix} REAL" +
                                      $"\n {TournamentSizePrefix} INT" +
                                      "\n Not necessarily in this order but the lines need to be present and they need to end with values corresponding to the line\'s content");
                    return null;
                }

                var numAlgorithmIterations =
                    int.Parse(numAlgorithmIterationsLine.Replace(NumAlgorithmIterationsPrefix, "").Trim());
                var populationSize = int.Parse(populationSizeLine.Replace(PopulationSizePrefix, "").Trim());
                var numGenerations = int.Parse(numGenerationsLine.Replace(NumGenerationsPrefix, "").Trim());
                var mutationProbability =
                    double.Parse(mutationProbabilityLine.Replace(MutationProbabilityPrefix, "").Trim());
                var crossProbability = double.Parse(crossProbabilityLine.Replace(CrossProbabilityPrefix, "").Trim());
                var tournamentSize = int.Parse(tournamentSizeLine.Replace(TournamentSizePrefix, "").Trim());

                if (populationSize % 2 != 0)
                {
                    populationSize++;
                }

                return new GaParameters
                {
                    NumAlgorithmIterations = numAlgorithmIterations,
                    PopulationSize = populationSize,
                    NumGenerations = numGenerations,
                    MutationProbability = mutationProbability,
                    CrossProbability = crossProbability,
                    TournamentSize = tournamentSize
                };
            }
            catch (FormatException)
            {
                Console.WriteLine($"Genetic ttp1 configuration file format error - first 6 lines need to be: " +
                                  $"\n {NumAlgorithmIterationsPrefix} INT" +
                                  $"\n {PopulationSizePrefix} INT" +
                                  $"\n {NumGenerationsPrefix} INT" +
                                  $"\n {MutationProbabilityPrefix} REAL" +
                                  $"\n {CrossProbabilityPrefix} REAL" +
                                  $"\n {TournamentSizePrefix} INT" +
                                  "\n Not necessarily in this order but the lines need to be present and they need to end with values corresponding to the line\'s content");
                return null;
            }
        }

        private static Problem.Problem ExtractProblem(List<string> srcFileLines)
        {
            var problemStats = ExtractProblemStats(srcFileLines);
            if (problemStats == null) return null;

            var cities = ExtractCities(srcFileLines);
            if (cities == null) return null;

            var items = ExtractItems(srcFileLines, cities);
            if (items == null) return null;

            return new ProblemTtp1(problemStats, cities, items);
        }

        private static ProblemStats ExtractProblemStats(List<string> srcFileLines)
        {
            try
            {
                var numCitiesLine = srcFileLines.Find(line => line.StartsWith(NumCitiesPrefix));
                var numItemsLine = srcFileLines.Find(line => line.StartsWith(NumItemsPrefix));
                var knapsackCapacityLine = srcFileLines.Find(line => line.StartsWith(KnapsackCapacityPrefix));
                var minSpeedLine = srcFileLines.Find(line => line.StartsWith(MinSpeedPrefix));
                var maxSpeedLine = srcFileLines.Find(line => line.StartsWith(MaxSpeedPrefix));
                var knapsackRentingRatioLine = srcFileLines.Find(line => line.StartsWith(KnapsackRentingRatioPrefix));

                if (numCitiesLine == null ||
                    numItemsLine == null ||
                    knapsackCapacityLine == null ||
                    minSpeedLine == null ||
                    maxSpeedLine == null ||
                    knapsackRentingRatioLine == null)
                {
                    Console.WriteLine($"Src file format error - first 6 lines need to be: " +
                                      $"\n {NumCitiesPrefix} INT" +
                                      $"\n {NumItemsPrefix} INT" +
                                      $"\n {KnapsackCapacityPrefix} INT" +
                                      $"\n {MinSpeedPrefix} REAL" +
                                      $"\n {MaxSpeedPrefix} REAL" +
                                      $"\n {KnapsackRentingRatioPrefix} REAL" +
                                      "\n Not necessarily in this order but the lines need to be present and they need to end with values corresponding to the line\'s content" +
                                      "\n eg. DIMENSION: 8 means there are 8 cities.");
                    return null;
                }

                var numCities = int.Parse(numCitiesLine.Replace(NumCitiesPrefix, "").Trim());
                var numItems = int.Parse(numItemsLine.Replace(NumItemsPrefix, "").Trim());
                var knapsackCapacity = int.Parse(knapsackCapacityLine.Replace(KnapsackCapacityPrefix, "").Trim());
                var minSpeed = double.Parse(minSpeedLine.Replace(MinSpeedPrefix, "").Trim());
                var maxSpeed = double.Parse(maxSpeedLine.Replace(MaxSpeedPrefix, "").Trim());
                var knapsackRentingRatio =
                    double.Parse(knapsackRentingRatioLine.Replace(KnapsackRentingRatioPrefix, "").Trim());

                return new ProblemStats
                {
                    NumCities = numCities,
                    NumItems = numItems,
                    KnapsackCapacity = knapsackCapacity,
                    KnapsackRentingRatio = knapsackRentingRatio,
                    MinSpeed = minSpeed,
                    MaxSpeed = maxSpeed
                };
            }
            catch (FormatException)
            {
                Console.Write("Src file format error - first 6 lines need to be: " +
                              $"\n {NumCitiesPrefix} INT" +
                              $"\n {NumItemsPrefix} INT" +
                              $"\n {KnapsackCapacityPrefix} INT" +
                              $"\n {MinSpeedPrefix} REAL" +
                              $"\n {MaxSpeedPrefix} REAL" +
                              $"\n {KnapsackRentingRatioPrefix} REAL" +
                              "\n Not necessarily in this order but the lines need to be present and they need to end with values corresponding to the line\'s content" +
                              "\n eg. DIMENSION: 8 means there are 8 cities.\n");
                return null;
            }
        }

        private static List<City> ExtractCities(List<string> srcFileLines)
        {
            try
            {
                var numCitiesLine = srcFileLines.Find(line => line.StartsWith(NumCitiesPrefix));
                var citiesOpeningLineIndex =
                    srcFileLines.IndexOf(srcFileLines.Find(line => line.StartsWith(CitiesOpeningLineFormat)));
                var itemsOpeningLineIndex =
                    srcFileLines.IndexOf(srcFileLines.Find(line => line.StartsWith(ItemsOpeningLineFormat)));

                if (numCitiesLine == null)
                {
                    Console.WriteLine(
                        $"Src file format error - '{NumCitiesPrefix}' line not found.");
                    return null;
                }

                if (citiesOpeningLineIndex == -1)
                {
                    Console.WriteLine(
                        $"Src file format error - '{CitiesOpeningLineFormat}' line not found.");
                    return null;
                }

                if (itemsOpeningLineIndex == -1)
                {
                    Console.WriteLine(
                        $"Src file format error - '{ItemsOpeningLineFormat}' line not found.");
                    return null;
                }

                var numCities = int.Parse(numCitiesLine.Replace(NumCitiesPrefix, "").Trim());

                if (itemsOpeningLineIndex <= citiesOpeningLineIndex + numCities)
                {
                    Console.WriteLine(
                        $"Src file format error - didn't specify enough cities according to '{NumCitiesPrefix}' line");
                    return null;
                }

                var cities = new List<City>();

                for (var i = 1; i <= numCities; i++)
                {
                    var nthCityLine = srcFileLines[citiesOpeningLineIndex + i];
                    var cityParams = nthCityLine.Split(null); // uses whitespace as delimiter if passed null

                    try
                    {
                        var cityId = int.Parse(cityParams[0]);
                        var cityCoordsX = double.Parse(cityParams[1]);
                        var cityCoordsY = double.Parse(cityParams[2]);

                        cities.Add(new City
                        {
                            Id = cityId,
                            CoordsX = cityCoordsX,
                            CoordsY = cityCoordsY
                        });
                    }
                    catch (FormatException)
                    {
                        Console.Write(
                            $"Src file format error - not following '{CitiesOpeningLineFormat}' format when specifying cities." +
                            $"\nCity line: {nthCityLine} \n");
                        return null;
                    }
                }


                return cities;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Src file format error - {NumCitiesPrefix} INT, sth other than INT specified.");
                return null;
            }
        }

        private static List<Item> ExtractItems(List<string> srcFileLines, List<City> cities)
        {
            try
            {
                var numItemsLine = srcFileLines.Find(line => line.StartsWith(NumItemsPrefix));
                var itemsOpeningLineIndex =
                    srcFileLines.IndexOf(srcFileLines.Find(line => line.StartsWith(ItemsOpeningLineFormat)));

                if (numItemsLine == null)
                {
                    Console.WriteLine(
                        $"Src file format error - '{NumItemsPrefix}' line not found.");
                    return null;
                }

                if (itemsOpeningLineIndex == -1)
                {
                    Console.WriteLine(
                        $"Src file format error - '{ItemsOpeningLineFormat}' line not found.");
                    return null;
                }

                var numItems = int.Parse(numItemsLine.Replace(NumItemsPrefix, "").Trim());

                if (srcFileLines.Count <= itemsOpeningLineIndex + numItems)
                {
                    Console.WriteLine(
                        $"Src file format error - didn't specify enough items according to '{NumItemsPrefix}' line");
                    return null;
                }

                var items = new List<Item>();

                for (var i = 1; i <= numItems; i++)
                {
                    var nthItemLine = srcFileLines[itemsOpeningLineIndex + i];
                    var itemParams = nthItemLine.Split(null); // uses whitespace as delimiter if passed null

                    try
                    {
                        var itemId = int.Parse(itemParams[0]);
                        var itemValue = int.Parse(itemParams[1]);
                        var itemWeight = int.Parse(itemParams[2]);
                        var itemAssignedCity = int.Parse(itemParams[3]);

                        if (!cities.Exists(city => city.Id == itemAssignedCity))
                        {
                            Console.Write(
                                "Src file format error - item's assigned city doesn't exist. \n" +
                                $"Item line: {nthItemLine} \n");
                            return null;
                        }

                        items.Add(new Item
                        {
                            Id = itemId,
                            Profit = itemValue,
                            Weight = itemWeight,
                            AssignedCityId = itemAssignedCity
                        });
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine(
                            $"Src file format error - not following '{ItemsOpeningLineFormat}' format when specifying items." +
                            $"\nItem line: {nthItemLine} \n");
                        return null;
                    }
                }

                return items;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Src file format error - {NumItemsPrefix} INT, sth other than INT specified.");
                return null;
            }
        }

        public static TabuParameters LoadTabuTtp1Params(string algorithmSrcFilePath)
        {
            return ExtractTabuTtp1Params(LoadSrcFileLines(algorithmSrcFilePath));
        }

        private static TabuParameters ExtractTabuTtp1Params(List<string> srcFileLines)
        {
            try
            {
                var numAlgorithmIterationsLine =
                    srcFileLines.Find(line => line.StartsWith(NumAlgorithmIterationsPrefix));
                var numTabuSearchesLine = srcFileLines.Find(line => line.StartsWith(NumTabuSearchesPrefix));
                var neighbourhoodSizeLine = srcFileLines.Find(line => line.StartsWith(NeighbourhoodSizePrefix));
                var tabuListSizeLine = srcFileLines.Find(line => line.StartsWith(TabuListSizePrefix));

                if (neighbourhoodSizeLine == null ||
                    tabuListSizeLine == null)
                {
                    Console.WriteLine($"Tabu ttp1 configuration file format error - first 4 lines need to be: " +
                                      $"\n {NumAlgorithmIterationsPrefix} INT" +
                                      $"\n {NumTabuSearchesPrefix} INT" +
                                      $"\n {NeighbourhoodSizePrefix} INT" +
                                      $"\n {TabuListSizePrefix} INT" +
                                      "\n Not necessarily in this order but the lines need to be present and they need to end with values corresponding to the line\'s content");
                    return null;
                }

                var numAlgorithmIterations =
                    int.Parse(numAlgorithmIterationsLine.Replace(NumAlgorithmIterationsPrefix, "").Trim());
                var numTabuSearches = int.Parse(numTabuSearchesLine.Replace(NumTabuSearchesPrefix, "").Trim());
                var neighbourhoodSize = int.Parse(neighbourhoodSizeLine.Replace(NeighbourhoodSizePrefix, "").Trim());
                var tabuListSize = int.Parse(tabuListSizeLine.Replace(TabuListSizePrefix, "").Trim());

                return new TabuParameters()
                {
                    NumAlgorithmIterations = numAlgorithmIterations,
                    NumTabuSearches = numTabuSearches,
                    NeighbourhoodSize = neighbourhoodSize,
                    TabuSize = tabuListSize
                };
            }
            catch (FormatException)
            {
                Console.WriteLine($"Tabu ttp1 configuration file format error - first 4 lines need to be: " +
                                  $"\n {NumAlgorithmIterationsPrefix} INT" +
                                  $"\n {NumTabuSearchesPrefix} INT" +
                                  $"\n {NeighbourhoodSizePrefix} INT" +
                                  $"\n {TabuListSizePrefix} INT" +
                                  "\n Not necessarily in this order but the lines need to be present and they need to end with values corresponding to the line\'s content");
                return null;
            }
        }
    }
}