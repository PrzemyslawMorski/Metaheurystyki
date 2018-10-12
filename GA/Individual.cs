using System.Collections.Generic;

namespace Metaheuristics.GA
{
    public class Individual
    {
        /*
            RoadTaken contains Id's of cities that were visited in order of the time they were visited.
            Value 1 at RoadTaken[0] means that City with Id 1 was visited first
            Value 5 at RoadTaken[1] means that City with Id 5 was visited second
            RoadTaken is always the size of ProblemStats.NumCities
        */
        public List<int> RoadTaken { get; set; }

        public Individual DeepCopy()
        {
            return new Individual
            {
                RoadTaken = new List<int>(RoadTaken)
            };
        }
    }
}