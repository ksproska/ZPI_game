using System;

namespace GA
{
    public enum SelectionType
    {
        Tournament
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
                default:
                    throw new NotImplementedException();
            }
            selector.SetArgs(parameters);
            return selector;
        }
    }
}