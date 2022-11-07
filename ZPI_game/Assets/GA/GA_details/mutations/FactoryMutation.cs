using System;
using System.Collections.Generic;
using GA.mutations;

namespace GA
{
    public enum MutationType
    {
        RSM,
        Thrors
    }
    public abstract class FactoryMutation
    {
        public static IMutator<T> Get<T>(MutationType mutationType) where T: IComparable
        {
            switch (mutationType)
            {
                case MutationType.RSM:
                    return new MutatorReverseSequence<T>();
                case MutationType.Thrors:
                    return new MutatorThrors<T>();
            }

            throw new NotImplementedException(); // To nie lepiej w defaulcie?
        }

        public static Dictionary<string, MutationType> GetTypeToNameMap()
        {
            return new Dictionary<string, MutationType>()
            {
                { "RSM", MutationType.RSM },
                { "Thrors", MutationType.Thrors },
            };
        }
    }
}