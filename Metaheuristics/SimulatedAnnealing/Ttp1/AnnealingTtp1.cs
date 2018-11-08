using System;
using System.Collections.Generic;

namespace Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1
{
    public class AnnealingTtp1
    {
        public AnnealingTtp1(Problem.Problem problem, AnnealingParameters parameters)
        {
            Problem = problem;
            Parameters = parameters;
            RandomNumGenerator = new Random();
        }

        private Problem.Problem Problem { get; }
        private AnnealingParameters Parameters { get; }
        private Random RandomNumGenerator { get; }

        public void Execute(Logger.Logger logger)
        {
            for (var i = 0; i < Parameters.NumAlgorithmIterations; i++)
            {
                var numAnnealingCycles = 0;
                var temperature = Parameters.InitialTemperature;

                var bestSolution = InitialSolution(Problem.CityIds);
                var bestFitness = Problem.Fitness(bestSolution);

                while (numAnnealingCycles < Parameters.NumAnnealingCycles)
                {
                    var neighbourhoodWithFitness = NeighbourhoodWithFitness(bestSolution);

                    if (neighbourhoodWithFitness == null)
                    {
                        Console.WriteLine("NeighbourhoodWithFitness is null.");
                        return;
                    }

                    var currentSolution = Best(neighbourhoodWithFitness);

                    if (currentSolution.Item2 > bestFitness ||
                        ShouldTryLeaveLocalOptima(bestFitness, currentSolution.Item2, temperature))
                    {
                        bestSolution = currentSolution.Item1;
                        bestFitness = currentSolution.Item2;
                    }

                    logger.LogAnnealingTtp1Cycle(numAnnealingCycles, bestFitness, currentSolution.Item2,
                        temperature);

                    Console.WriteLine(
                        $"SIMULATED ANNEALING iteration: {i} annealing cycle: {numAnnealingCycles} bestFitness: {bestFitness} temperature: {temperature}");

                    numAnnealingCycles++;
                    temperature = NextTemperature(temperature, numAnnealingCycles);
                }
            }
        }

        private bool ShouldTryLeaveLocalOptima(double bestFitness, double currentSolutionFitness, double temperature)
        {
            var randomChance = RandomNumGenerator.NextDouble();
            var chanceForWorseSolutionAccepted =
                1D / (1D + Math.Exp((bestFitness - currentSolutionFitness) / temperature));
            return randomChance < chanceForWorseSolutionAccepted;
        }

        private double NextTemperature(double currentTemperature, int annealingCycle)
        {
            var nextTemperatureAsPercentageOfInitialTemperature =
                (100D - Parameters.TemperaturePercentageDropPerCycle * annealingCycle) / 100D;
            var nextTemperature = Parameters.InitialTemperature * nextTemperatureAsPercentageOfInitialTemperature;

            return nextTemperature < 0D ? 0D : nextTemperature;
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

            return new Ttp1Individual(randomRoadTaken);
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

        private static Tuple<IIndividual, double> Best(Dictionary<IIndividual, double> neighbourhood)
        {
            IIndividual bestNeighbour = null;
            var bestNeighbourFitness = double.MinValue;

            foreach (var neighbourFitness in neighbourhood)
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