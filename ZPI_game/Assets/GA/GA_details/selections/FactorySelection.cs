using System;
using System.Collections.Generic;

namespace GA
{
    public enum SelectionType
    {
        Tournament, Roulette
    }
    public abstract class FactorySelection
    {
        public static ISelector Get<T>(SelectionType selectionType, params double[] parameters)
        {
            ISelector selector;
            switch (selectionType)
            {
                case SelectionType.Tournament:
                    selector = new SelectorTournament();
                    break;
                case SelectionType.Roulette:
                    selector = new SelectorRoulette();
                    break;
                default:
                    throw new NotImplementedException();
            }
            selector.SetArgs(parameters);
            return selector;
        }

        public static Dictionary<string, SelectionType> GetTypeToNameMap()
        {
            return new Dictionary<string, SelectionType>()
            {
                { "Tournament", SelectionType.Tournament },
                { "Roulette", SelectionType.Roulette },
            };
        }
        public static Dictionary<SelectionType, string[]> GetTypeToArgsMap<T>()
        {
            return new Dictionary<SelectionType, string[]>()
            {
                { SelectionType.Tournament, Get<T>(SelectionType.Tournament).GetArgs()},
                { SelectionType.Roulette, Get<T>(SelectionType.Roulette).GetArgs()},
            };
        }
    }
}