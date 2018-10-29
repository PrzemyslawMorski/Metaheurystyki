namespace Metaheuristics.Metaheuristics.SimulatedAnnealing.Ttp1
{
    public class AnnealingParameters
    {
        public int NumAlgorithmIterations { get; set; }
        public int NumAnnealingCycles { get; set; }
        public int NeighbourhoodSize { get; set; }
        public double InitialTemperature { get; set; }
        public double TemperaturePercentageDropPerCycle { get; set; }
    }
}