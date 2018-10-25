using System;
using System.Collections.Generic;
using Metaheuristics.Algorithms;

namespace Metaheuristics.GA.TTP2
{
    public class GaTtp2 : Ga
    {
        public GaTtp2(Problem.Problem problem, GaParameters parameters) : base(problem, parameters)
        {
        }

        public override void Execute(Logger.Logger logger)
        {
            throw new NotImplementedException();
        }

        protected override IList<IIndividual> InitializePopulation(IList<int> cityIds)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Tuple<IIndividual, IIndividual>> SelectForCrossing(IList<IIndividual> currentPopulation)
        {
            throw new NotImplementedException();
        }

        protected override Tuple<IIndividual, IIndividual> CrossIndividuals(IIndividual indiv1, IIndividual indiv2)
        {
            throw new NotImplementedException();
        }

        protected override IIndividual MutateIndividual(IIndividual indiv, bool guaranteedMutation = false)
        {
            throw new NotImplementedException();
        }
    }
}