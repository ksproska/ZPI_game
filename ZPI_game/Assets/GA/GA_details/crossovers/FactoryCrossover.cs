using System;
using System.Collections.Generic;

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

        public static Dictionary<string, CrossoverType> GetTypeToNameMap()
        {
            return new Dictionary<string, CrossoverType>()
            {
                { "Partially matched", CrossoverType.PartiallyMatched }
            };
        }
    }
}