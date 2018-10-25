using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.Algorithms.TabuSearch.Ttp1
{
    public class TabuTtp1
    {
        public TabuTtp1(Problem.Problem problem, TabuParameters parameters)
        {
            Problem = problem;
            Parameters = parameters;
            RandomNumGenerator = new Random();
        }

        private Problem.Problem Problem { get; }
        private TabuParameters Parameters { get; }
        private Random RandomNumGenerator { get; }

        public void Execute(Logger.Logger logger)
        {
            for (var i = 0; i < Parameters.NumAlgorithmIterations; i++)
            {
                var numTabuSearches = 0;

                var currentSolution = InitialSolution(Problem.CityIds);

                var bestSolution = currentSolution.DeepCopy();
                var bestFitness = Problem.Fitness(bestSolution);

                var tabuList = new LinkedList<IIndividual>();
                tabuList.AddLast(bestSolution);

                var neighbourhoodHasPotentialSolutions = true;
                
                while (numTabuSearches < Parameters.NumTabuSearches && neighbourhoodHasPotentialSolutions)
                {
                    var neighbourhoodWithFitness = NeighbourhoodWithFitness(currentSolution);
                    var bestOfNeighbourhood = Best(neighbourhoodWithFitness, tabuList);

                    if (bestOfNeighbourhood.Item1 == null)
                    {
                        neighbourhoodHasPotentialSolutions = false;
                        continue;
                    }

                    currentSolution = bestOfNeighbourhood.Item1;

                    if (bestOfNeighbourhood.Item2 > bestFitness)
                    {
                        bestSolution = currentSolution;
                        bestFitness = bestOfNeighbourhood.Item2;
                    }

                    logger.LogTabuTtp1Search(numTabuSearches, bestFitness, bestOfNeighbourhood.Item2);

                    tabuList = UpdateTabuList(tabuList, bestSolution);

                    Console.WriteLine($"TABU SEARCH iteration: {i} tabu search: {numTabuSearches}");
                    numTabuSearches++;
                }
            }
        }

        private LinkedList<IIndividual> UpdateTabuList(LinkedList<IIndividual> tabuList,
            IIndividual bestSolution)
        {
            tabuList.AddLast(bestSolution);

            if (tabuList.Count >= Parameters.TabuSize)
            {
                tabuList.RemoveFirst();
            }

            return tabuList;
        }

        private IIndividual InitialSolution(IReadOnlyCollection<int> cityIds)
        {
            var numCities = cityIds.Count;
            var randomRoadTaken = new List<int>(numCities);

            var mutableListCityIds = new List<int>(cityIds);

            for (var i = 0; i < numCities; i++)
            {
                var randomIndex = RandomNumGenerator.Next(0, mutableListCityIds.Count);
                var randomCityId = mutableListCityIds[randomIndex];
                mutableListCityIds.RemoveAt(randomIndex);
                randomRoadTaken.Add(randomCityId);
            }

            var individual = new TabuTtpIndividual
            {
                RoadTaken = randomRoadTaken
            };

            return individual;
        }

        private Dictionary<IIndividual, double> NeighbourhoodWithFitness(IIndividual solution)
        {
            var neighbourhood = new List<IIndividual>();

            for (var i = 0; i < Parameters.NeighbourhoodSize; i++)
            {
                var neighbour = Neighbour(solution);

                while (neighbourhood.Contains(neighbour))
                {
                    neighbour = Neighbour(solution: solution, deepCopy: false);
                }

                neighbourhood.Add(neighbour);
            }

            var neighbourhoodWithFitness = new Dictionary<IIndividual, double>();

            foreach (var neighbour in neighbourhood)
            {
                neighbourhoodWithFitness[neighbour] = Problem.Fitness(neighbour);
            }

            return neighbourhoodWithFitness;
        }

        private static Tuple<IIndividual, double> Best(Dictionary<IIndividual, double> neighbourhood,
            ICollection<IIndividual> tabuList)
        {
            var neighboursNotInTabu = neighbourhood.Where(neighbourFitness => !tabuList.Contains(neighbourFitness.Key));

            IIndividual bestNeighbour = null;
            var bestNeighbourFitness = double.MinValue;

            foreach (var neighbourFitness in neighboursNotInTabu)
            {
                if (!(neighbourFitness.Value > bestNeighbourFitness)) continue;
                
                bestNeighbour = neighbourFitness.Key;
                bestNeighbourFitness = neighbourFitness.Value;
            }
            
            return new Tuple<IIndividual, double>(bestNeighbour, bestNeighbourFitness);
        }

        private IIndividual Neighbour(IIndividual solution, bool deepCopy = true)
        {
            TabuTtpIndividual neighbour = null;

            if (deepCopy)
            {
                neighbour = solution.DeepCopy() as TabuTtpIndividual;
            }
            else
            {
                neighbour = solution as TabuTtpIndividual;
            }

            if (neighbour == null)
            {
                return null;
            }

            var firstRandomSwapIndex = RandomNumGenerator.Next(0, neighbour.RoadTaken.Count);
            var secondRandomSwapIndex = RandomNumGenerator.Next(0, neighbour.RoadTaken.Count);

            while (firstRandomSwapIndex == secondRandomSwapIndex)
            {
                secondRandomSwapIndex = RandomNumGenerator.Next(0, neighbour.RoadTaken.Count);
            }

            var tmp = neighbour.RoadTaken[firstRandomSwapIndex];
            neighbour.RoadTaken[firstRandomSwapIndex] = neighbour.RoadTaken[secondRandomSwapIndex];
            neighbour.RoadTaken[secondRandomSwapIndex] = tmp;

            return neighbour;
        }
    }
}