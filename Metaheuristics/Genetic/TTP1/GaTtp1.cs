using System;
using System.Collections.Generic;
using System.Linq;
using Metaheuristics.GA;

namespace Metaheuristics.Metaheuristics.Genetic.TTP1
{
    public class GaTtp1 : Ga
    {
        public GaTtp1(Problem.Problem problem, GaParameters parameters) : base(problem, parameters)
        {
        }

        public override List<List<IIndividual>> Execute(Action<int, double, double, double> logGeneration,
            Action<List<double>> logOutro, int startingGenerationForLogging = 0, List<List<IIndividual>> initialPopulations = null)
        {
            var bestFitnesses = new List<double>();
            var finalPopulations = new List<List<IIndividual>>();

            for (var i = 0; i < Parameters.NumAlgorithmIterations; i++)
            {
                var generation = 0;
                var population = initialPopulations == null ? InitializePopulation() : initialPopulations[i];

                EvaluatePopulation(population, generation, logGeneration, startingGenerationForLogging);
                generation++;

                while (generation < Parameters.NumGenerations)
                {
                    Console.WriteLine($"Iteration {i} Generation {generation}");

                    population = Evolve(population);

                    var bestFitness = EvaluatePopulation(population, generation, logGeneration, startingGenerationForLogging);
                    generation++;

                    if (generation == Parameters.NumGenerations - 1)
                    {
                        bestFitnesses.Add(bestFitness);
                    }
                }

                finalPopulations.Add(population);
            }

            logOutro(bestFitnesses);

            return finalPopulations;
        }

        public override List<IIndividual> InitializePopulation()
        {
            var population = new List<IIndividual>();
            var numCities = Problem.CityIds.Count;
            for (var i = 0; i < Parameters.PopulationSize; i++)
            {
                var randomRoadTaken = new List<int>(numCities);

                var mutableListCityIds = new List<int>(Problem.CityIds);

                for (var j = 0; j < numCities; j++)
                {
                    var randomIndex = RandomNumGenerator.Next(0, mutableListCityIds.Count);
                    var randomCityId = mutableListCityIds[randomIndex];
                    mutableListCityIds.RemoveAt(randomIndex);
                    randomRoadTaken.Add(randomCityId);
                }

                var individual = new Ttp1Individual(randomRoadTaken);
                population.Add(individual);
            }

            return population;
        }

        protected override IEnumerable<Tuple<IIndividual, IIndividual>> SelectForCrossing(
            List<IIndividual> currentPopulation)
        {
            var numPairs = currentPopulation.Count / 2;
            var resultingPairs = new List<Tuple<IIndividual, IIndividual>>();

            for (var i = 0; i < numPairs; i++)
            {
                var firstIndiv = TournamentSelect(currentPopulation) as Ttp1Individual;
                var secondIndiv = TournamentSelect(currentPopulation) as Ttp1Individual;

                if (firstIndiv == null || secondIndiv == null)
                {
                    Console.WriteLine("Wrong type of individuals population passed to TTP1. It need TTP2 population.");
                    return null;
                }

                while (secondIndiv.RoadTaken.Equals(firstIndiv.RoadTaken))
                {
                    secondIndiv =
                        currentPopulation[RandomNumGenerator.Next(0, currentPopulation.Count)] as Ttp1Individual;
                    if (secondIndiv != null) continue;

                    Console.WriteLine("Wrong type of individuals population passed to TTP1. It need TTP2 population.");
                    return null;
                }

                resultingPairs.Add(new Tuple<IIndividual, IIndividual>(firstIndiv, secondIndiv));
            }

            return resultingPairs;
        }

        protected override IIndividual MutateIndividual(IIndividual indiv, bool guaranteedMutation = false)
        {
            if (!guaranteedMutation && !ShouldMutate()) return indiv;

            if (!(indiv is Ttp1Individual indivTtp1))
            {
                Console.WriteLine("Wrong type of individuals population passed to TTP1. It need TTP2 population.");
                return null;
            }

            var firstRandomSwapIndex = RandomNumGenerator.Next(0, indivTtp1.RoadTaken.Count);
            var secondRandomSwapIndex = RandomNumGenerator.Next(0, indivTtp1.RoadTaken.Count);

            while (firstRandomSwapIndex == secondRandomSwapIndex)
            {
                secondRandomSwapIndex = RandomNumGenerator.Next(0, indivTtp1.RoadTaken.Count);
            }

            var tmp = indivTtp1.RoadTaken[firstRandomSwapIndex];
            indivTtp1.RoadTaken[firstRandomSwapIndex] = indivTtp1.RoadTaken[secondRandomSwapIndex];
            indivTtp1.RoadTaken[secondRandomSwapIndex] = tmp;

            return indiv;
        }


        protected override Tuple<IIndividual, IIndividual> CrossIndividuals(IIndividual indiv1, IIndividual indiv2)
        {
            if (!ShouldCross()) return new Tuple<IIndividual, IIndividual>(indiv1, indiv2);

            var indiv1Child = CrossPmx(indiv1, indiv2);
            var indiv2Child = CrossPmx(indiv2, indiv1);

            return new Tuple<IIndividual, IIndividual>(indiv1Child, indiv2Child);
        }

        private IIndividual CrossPmx(IIndividual parent1, IIndividual parent2)
        {
            var child = parent1.DeepCopy();

            if (!(parent1 is Ttp1Individual parent1Ttp1) || !(parent2 is Ttp1Individual parent2Ttp1) ||
                !(child is Ttp1Individual childTtp1)) return child;
            if (parent1Ttp1.RoadTaken.Count != parent2Ttp1.RoadTaken.Count) return childTtp1;

            var roadLength = parent1Ttp1.RoadTaken.Count;
            var road1 = parent1Ttp1.RoadTaken;
            var road2 = parent1Ttp1.RoadTaken;

            var firstChildFirstCutIndex = RandomNumGenerator.Next(0, roadLength);
            var firstChildSecondCutIndex = RandomNumGenerator.Next(firstChildFirstCutIndex, roadLength);

            var cutValuesFromIndiv1 =
                road1.GetRange(firstChildFirstCutIndex, firstChildSecondCutIndex - firstChildFirstCutIndex);
            var cutValuesFromIndiv2 = road2
                .GetRange(firstChildFirstCutIndex, firstChildSecondCutIndex - firstChildFirstCutIndex)
                .Where(valueFromIndiv2 => !cutValuesFromIndiv1.Contains(valueFromIndiv2)).ToList();

            foreach (var value in cutValuesFromIndiv2)
            {
                var currentValueFromIndiv2 = value;
                var valueAtTheSamePositionInIndiv1 =
                    road1[road2.IndexOf(currentValueFromIndiv2)];
                var indexOfThatValueInIndiv2 = road2.IndexOf(valueAtTheSamePositionInIndiv1);

                while (indexOfThatValueInIndiv2 >= firstChildFirstCutIndex ||
                       indexOfThatValueInIndiv2 < firstChildSecondCutIndex)
                {
                    currentValueFromIndiv2 = road1[indexOfThatValueInIndiv2];
                    valueAtTheSamePositionInIndiv1 =
                        road1[road2.IndexOf(currentValueFromIndiv2)];
                    indexOfThatValueInIndiv2 = road2.IndexOf(valueAtTheSamePositionInIndiv1);
                }

                childTtp1.RoadTaken[indexOfThatValueInIndiv2] = value;
            }

            var valuesLeftInIndiv2 = road2.Where(value => !cutValuesFromIndiv1.Contains(value) &&
                                                          !cutValuesFromIndiv2.Contains(value)).ToList();

            var indexInIndiv1 = 0;
            foreach (var valueLeftInIndiv2 in valuesLeftInIndiv2)
            {
                var insertedValue = false;

                while (indexInIndiv1 < roadLength && !insertedValue)
                {
                    var valueFromIndiv1 = childTtp1.RoadTaken[indexInIndiv1];
                    if (cutValuesFromIndiv1.Contains(valueFromIndiv1) || cutValuesFromIndiv2.Contains(valueFromIndiv1))
                    {
                        indexInIndiv1++;
                        continue;
                    }

                    childTtp1.RoadTaken[indexInIndiv1] = valueLeftInIndiv2;
                    indexInIndiv1++;
                    insertedValue = true;
                }
            }

            return childTtp1;
        }
    }
}