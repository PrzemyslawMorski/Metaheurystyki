using System.Collections.Generic;
using Metaheuristics.Algorithms;

namespace Metaheuristics.GA.TTP2
{
    public class IndividualTtp2 : IIndividual
    {
        /*
            RoadTaken contains Id's of cities that were visited in order of the time they were visited.
            Value 1 at RoadTaken[0] means that City with Id 1 was visited first
            Value 5 at RoadTaken[1] means that City with Id 5 was visited second
            RoadTaken is always the size of ProblemStats.NumCities
        */
        public List<int> RoadTaken { get; set; }

        /*
           ItemsTaken is a Dictionary mapping an Item's Id to the fact that this item was picked up in it's assigned city
           ItemsTaken[0] == true when item with Id 1 was picked up in it's assigned city
           ItemsTaken[1] == false when item with Id 2 wasn't picked up in it's assigned city
           ItemsTaken is always the size of ProblemStats.NumItems
       */
        public Dictionary<int, bool> ItemsTaken { get; private set; }

        public IIndividual DeepCopy()
        {
            return new IndividualTtp2
            {
                RoadTaken = new List<int>(RoadTaken),
                ItemsTaken = new Dictionary<int, bool>(ItemsTaken)
            };
        }

        public bool Equals(IIndividual other)
        {
            throw new System.NotImplementedException();
        }
    }
}