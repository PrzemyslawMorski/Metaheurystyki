using System.Collections.Generic;

namespace Metaheuristics.GA
{
    public class IndividualTtp1 : IIndividual
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
            return new IndividualTtp1
            {
                RoadTaken = new List<int>(RoadTaken)
            };
        }

        public bool Equals(IIndividual other)
        {
            return other is IndividualTtp1 otherTtp1 && RoadTaken.Equals(otherTtp1.RoadTaken);
        }
    }
}