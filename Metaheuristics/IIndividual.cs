namespace Metaheuristics.Metaheuristics
{
    public interface IIndividual
    {
        IIndividual DeepCopy();
        bool Equals(IIndividual other);
    }
}