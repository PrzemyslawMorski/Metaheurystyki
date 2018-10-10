namespace Metaheuristics.Problem
{
    public struct City
    {
        public int Id { get; set; }
        public double CoordsX { get; set; }
        public double CoordsY { get; set; }

        public double Distance(City c1, City c2)
        {
            return 0d;
        }
    }
}