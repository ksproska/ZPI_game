using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.GA.Utils
{
    public class LabeledRecordedList<TL, T> where T: IComparable
    {
        private List<T> currentState;
        private List<(int, T, TL)> history;
        
        public List<(int, T, TL)> History => history;
        public List<T> CurrentState => currentState;
        public static DummyLabeledRecordedList<TL, T> Dummy => new();

        public LabeledRecordedList()
        {
            currentState = new List<T>();
            history = new List<(int, T, TL)>();
        }

        public LabeledRecordedList(IEnumerable<T> args)
        {
            currentState = args.ToList();
            history = new List<(int, T, TL)>();
        }

        public T this[int index]
        {
            get => currentState[index];
            set => SetDefaultElementAt(index, value);
        }

        public (T, TL) GetLabeled(int index)
        {
            var indexHistory = history.Where(elem => elem.Item1 == index).ToList();
            if (indexHistory.Count == 0) return default;
            return (currentState[index], indexHistory.Last().Item3);
        }

        public void SetLabeled(int index, T value, TL label)
        {
            currentState[index] = value;
            history.Add((index, value, label));
        }

        public void Add(T value)
        {
            currentState.Add(value);
            history.Add((currentState.Count - 1, value, default));
        }

        public void AddLabeled(T value, TL label)
        {
            currentState.Add(value);
            history.Add((currentState.Count - 1, value, label));
        }

        public void AddRange(IEnumerable<T> args)
        {
            int beforeAddCount = currentState.Count;
            currentState.AddRange(args);

            args.Select((v, i) => (i, v))
                .ToList()
                .ForEach(elem => history.Add((beforeAddCount + elem.i, elem.v, default)));
        }

        public void AddRangeLabeled(IEnumerable<(T, TL)> args)
        {
            int beforeAddCount = currentState.Count;
            currentState.AddRange(args.Select(elem => elem.Item1));

            args.Select((pair, i) => (i, pair))
                .ToList()
                .ForEach(elem => history.Add((beforeAddCount + elem.i, elem.pair.Item1, elem.pair.Item2)));
        }
        
        public List<(int, T, TL)> GetFullHistory()
        {
            var unchanged = GetUnchangedIndices()
                .Select(index => (index, currentState[index]))
                .ToList();
            var ret = new List<(int, T, TL)>(history);
            unchanged.ForEach(pair => ret.Add((pair.index, pair.Item2, default)));
            return ret;
        }

        private List<int> GetUnchangedIndices()
        {
            var ret = new List<int>();
            var swappedIndices = history.Select(elem => elem.Item1).ToList();
            for(int i = 0; i < currentState.Count; i++)
            {
                if(!swappedIndices.Contains(i))
                {
                    ret.Add(i);
                }
            }
            return ret;
        }

        private void SetDefaultElementAt(int index, T elem)
        {
            currentState[index] = elem;
            history.Add((index, elem, default));
        }
    }
    
    public class DummyLabeledRecordedList<TL, T>: LabeledRecordedList<TL, T> where T: IComparable
    {

    }
}