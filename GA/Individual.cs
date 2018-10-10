using System;

namespace Metaheuristics.GA
{
    public class Individual
    {
        /*
            RoadTaken contains Id's of cities that were visited.
            Value 1 at RoadTaken[0] means that City with Id 1 was visited first
            Value 5 at RoadTaken[1] means that City with Id 5 was visited second
            RoadTaken is always the size of ProblemStats.NumCities
        */
        public int[] RoadTaken { get; set; }

        /*
            ItemsTaken contains Tuples of type <int,bool>
            Tuples correspond to an item Id (int) and the fact that this item was picked up in it's assigned city (bool)
            ItemsAssignment[0] == <1, true> when item with Id 1 was picked up in it's assigned city
            ItemsAssignment[1] == <2, false> when item with Id 2 wasn't picked up in it's assigned city
            ItemsTaken is always the size of ProblemStats.NumItems
        */
        public Tuple<int,bool>[] ItemsTaken { get; set; }

        public static Tuple<Individual, Individual> Mutate(Individual indiv1, Individual indiv2)
        {
            return null;
        }

        public static Tuple<Individual, Individual> Cross(Individual indiv1, Individual indiv2)
        {
            return null;
        }
    }
}