namespace Metaheuristics.Algorithms
{
    public interface IIndividual
    {
        IIndividual DeepCopy();
        bool Equals(IIndividual other);
    }
}