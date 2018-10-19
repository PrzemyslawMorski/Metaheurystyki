namespace Metaheuristics.Problem
{
    public struct CityCity
    {
        public int City1Id { get; set; }
        public int City2Id { get; set; }

        public CityCity(int city1Id, int city2Id)
        {
            City1Id = city1Id;
            City2Id = city2Id;
        }
    }
}