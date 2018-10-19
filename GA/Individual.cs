using System.Collections.Generic;

namespace Metaheuristics.GA
{
    public interface IIndividual
    {
        IIndividual DeepCopy();
        bool Equals(IIndividual other);
    }
}