namespace Metaheurystyki.Problem
{
    public struct City
    {
        public int Id { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }

        public double Distance(City c1, City c2)
        {
            return 0d;
        }
    }
}