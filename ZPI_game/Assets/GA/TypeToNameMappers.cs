using System;
using System.Collections.Generic;

namespace GA
{
    public abstract class TypeToNameMappers
    {
        public static Dictionary<String, CrossoverType> GetCrossoverDescriptionMapper()
        {
            return FactoryCrossover.GetTypeToNameMap();
        }
        public static Dictionary<String, MutationType> GetMutationDescriptionMapper()
        {
            return FactoryMutation.GetTypeToNameMap();
        }
        public static Dictionary<String, SelectionType> GetSelectionDescriptionMapper()
        {
            return FactorySelection.GetTypeToNameMap();
        }
        public static Dictionary<SelectionType, String[]> GetSelectionArgsMapper<T>()
        {
            return FactorySelection.GetTypeToArgsMap<T>();
        }
    }
}