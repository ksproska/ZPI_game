using System;
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

            throw new NotImplementedException();
        }
    }
}