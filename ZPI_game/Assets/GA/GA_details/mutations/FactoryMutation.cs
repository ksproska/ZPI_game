using System;
using System.Collections.Generic;
using GA.mutations;

namespace GA
{
    public enum MutationType
    {
        PartialReverser
    }
    public abstract class FactoryMutation
    {
        public static IMutator<T> Get<T>(MutationType mutationType)
        {
            switch (mutationType)
            {
                case MutationType.PartialReverser:
                    return new MutatorPartialReverser<T>();
            }

            throw new NotImplementedException(); // To nie lepiej w defaulcie?
        }

        public static Dictionary<string, MutationType> GetTypeToNameMap()
        {
            return new Dictionary<string, MutationType>()
            {
                { "Reverse random part of genotype", MutationType.PartialReverser }
            };
        }
    }
}