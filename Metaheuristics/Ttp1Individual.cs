using System.Collections.Generic;

namespace Metaheuristics.Metaheuristics
{
    public class Ttp1Individual : IIndividual
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
            return new Ttp1Individual
            {
                RoadTaken = new List<int>(RoadTaken)
            };
        }

        public bool Equals(IIndividual other)
        {
            return other is Ttp1Individual otherTtp1 && RoadTaken.Equals(otherTtp1.RoadTaken);
        }
    }
}