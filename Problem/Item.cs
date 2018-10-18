using System;

namespace Metaheuristics.Problem
{
    public struct Item : IComparable<Item>
    {
        public int Id { get; set; }
        public int Profit { get; set; }
        public int Weight { get; set; }
        public int AssignedCityId { get; set; }

        public int CompareTo(Item other)
        {
            return Profit / Weight - other.Profit / other.Weight;
        }
    }
}