namespace Metaheuristics.GA
{
    public class GaParameters
    {
        public int NumAlgorithmIterations { get; set; }
        public int PopulationSize { get; set; }
        public int NumGenerations { get; set; }
        public double MutationProbability { get; set; }
        public double CrossProbability { get; set; }
        public int TournamentSize { get; set; }
    }
}