using System;

namespace GA
{
    public enum CrossoverType
    {
        PartiallyMatched
    }
    public class FactoryCrossover
    {
        public static ICrosser<int> Get(CrossoverType crossoverType)
        {
            switch (crossoverType)
            {
                case CrossoverType.PartiallyMatched:
                    return new CrosserPartiallyMatched();
            }

            throw new NotImplementedException();
        }
    }
}