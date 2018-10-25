using System.Collections.Generic;
using Metaheuristics.GA;

namespace Metaheuristics.Algorithms.Genetic.TTP1
{
    public class GeneticTtp1Individual : IIndividual
    {
        /*
            RoadTaken contains Id's of cities that were visited in order of the time they were visited.
            Value 1 at RoadTaken[0] means that City with Id 1 was visited first
            Value 5 at RoadTaken[1] means that City with Id 5 was visited second
            RoadTaken is always the size of ProblemStats.NumCities
        */
        public List<int> RoadTaken { get; set; }

        public IIndividual DeepCopy()
        {
            return new GeneticTtp1Individual
            {
                RoadTaken = new List<int>(RoadTaken)
            };
        }

        public bool Equals(IIndividual other)
        {
            return other is GeneticTtp1Individual otherTtp1 && RoadTaken.Equals(otherTtp1.RoadTaken);
        }
    }
}