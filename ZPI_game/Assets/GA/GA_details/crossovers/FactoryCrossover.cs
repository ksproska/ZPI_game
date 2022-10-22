using System;
using System.Collections.Generic;

namespace GA
{
    public enum CrossoverType
    {
        PartiallyMatched,
        Order
    }
    public class FactoryCrossover
    {
        public static ICrosser<int> Get(CrossoverType crossoverType)
        {
            switch (crossoverType)
            {
                case CrossoverType.PartiallyMatched:
                    return new CrosserPartiallyMatched();
                case CrossoverType.Order:
                    return new CrosserOrder();
            }

            throw new NotImplementedException(); // To nie lepiej w defaulcie?
        }

        public static Dictionary<string, CrossoverType> GetTypeToNameMap()
        {
            return new Dictionary<string, CrossoverType>()
            {
                { "PMX", CrossoverType.PartiallyMatched },
                { "OX", CrossoverType.Order},
            };
        }
    }
}