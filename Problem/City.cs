using System;

namespace Metaheuristics.Problem
{
    public struct City
    {
        public int Id { get; set; }
        public double CoordsX { private get; set; }
        public double CoordsY { private get; set; }

        public double Distance(City c1, City c2)
        {
            return Math.Sqrt(Math.Pow(c1.CoordsX - c2.CoordsX, 2) + Math.Pow(c1.CoordsY - c2.CoordsY, 2));
        }
    }
}