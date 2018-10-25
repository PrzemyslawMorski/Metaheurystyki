using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaheuristics.Metaheuristics.TabuSearch.Ttp1
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
                tabuList.AddFirst(bestSolution);

                var neighbourhoodHasPotentialSolutions = true;
                var numConsequentVoidSearches = 0;

                while (numTabuSearches < Parameters.NumTabuSearches && neighbourhoodHasPotentialSolutions)
                {
                    var neighbourhoodWithFitness = NeighbourhoodWithFitness(currentSolution);

                    if (neighbourhoodWithFitness == null)
                    {
                        Console.WriteLine("NeighbourhoodWithFitness is null.");
                        return;
                    }

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
                        numConsequentVoidSearches = 0;
                    }
                    else
                    {
                        numConsequentVoidSearches++;
                    }

                    logger.LogTabuTtp1Search(numTabuSearches, bestFitness, bestOfNeighbourhood.Item2);

                    tabuList = UpdateTabuList(tabuList, bestSolution);

                    Console.WriteLine($"TABU SEARCH iteration: {i} tabu search: {numTabuSearches} bestFitness: {bestFitness}");
                    numTabuSearches++;

                    if (numConsequentVoidSearches > 40)
                    {
                        neighbourhoodHasPotentialSolutions = false;
                    }
                }
            }
        }

        private LinkedList<IIndividual> UpdateTabuList(LinkedList<IIndividual> tabuList,
            IIndividual bestSolution)
        {
            tabuList.AddFirst(bestSolution);

            if (tabuList.Count > Parameters.TabuSize)
            {
                tabuList.RemoveLast();
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

            var individual = new Ttp1Individual
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

                if (neighbour == null)
                {
                    Console.WriteLine("Neighbour is null.");
                    return null;
                }

                while (neighbourhood.Contains(neighbour))
                {
                    neighbour = Neighbour(solution);

                    if (neighbour != null) continue;

                    Console.WriteLine("Neighbour is null.");
                    return null;
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

        private IIndividual Neighbour(IIndividual solution)
        {
            Ttp1Individual neighbour = null;

            neighbour = solution.DeepCopy() as Ttp1Individual;

            if (neighbour == null)
            {
                Console.WriteLine("Solution passed to Neighbour is not of type TabuTtp1Individual");
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

            if (!neighbour.Equals(solution)) return neighbour;
            
            Console.WriteLine("Neighbour equals solution.");
            return null;
        }
    }
}