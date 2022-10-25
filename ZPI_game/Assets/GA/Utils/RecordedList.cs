using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GA.Utils
{
    public class RecordedListException: Exception
    {
        public RecordedListException(string message = "") : base(message)
        {

        }
    }
    public class RecordedList<T> where T: IComparable
    {
        private List<List<T>> history;
        private List<T> currentState;

        public List<T> InitState => history[0];
        public List<List<T>> History => history;
        public List<T> CurrentState => currentState;

        public static DummyRecordedList<T> Dummy => new DummyRecordedList<T>();

        public RecordedList()
        {
            currentState = new List<T>();
            history = new List<List<T>>();
            var newList = new List<T>();
            history.Add(newList);
        }

        public RecordedList(IEnumerable<T> args)
        {
            currentState = args.ToList();
            history = new List<List<T>>();
            var newList = new List<T>();
            newList.AddRange(currentState);
            history.Add(newList);
        }

        public T this[int index]
        {
            set => SetElementAt(index, value);
            get => currentState[index];
        }

        public void Add(T elem)
        {
            currentState.Add(elem);
            AddToHistory();
        }

        public void AddRange(IEnumerable<T> args)
        {
            currentState.AddRange(args);
            AddToHistory();
        }

        private void SetElementAt(int index, T element)
        {
            currentState[index] = element;
            AddToHistory();
        }

        public List<(int, T)> GetHistoryDifference()
        {
            var ret = new List<(int, T)>();
            for(int i = 0; i < history.Count - 1; i++)
            {
                ret.AddRange(GetSingleDifference(i));
            }
            return ret;
        }

        private List<(int, T)> GetSingleDifference(int historyStartIndex)
        {
            var ret = new List<(int, T)>();
            var his1 = history[historyStartIndex];
            var his2 = history[historyStartIndex + 1];

            for(int i = 0; i < his1.Count; i++)
            {
                if (his1[i].CompareTo(his2[i]) != 0)
                {
                    ret.Add((i, his2[i]));
                }
            }
            if(his2.Count > his1.Count)
            {
                for(int i = 0; i < his2.Count - his1.Count; i++)
                {
                    ret.Add((i + his1.Count, his2[i + his1.Count]));
                }
            }

            return ret;
        }

        private void AddToHistory()
        {
            List<T> newList = new List<T>();
            newList.AddRange(currentState);
            history.Add(newList);
        }
    }

    public class DummyRecordedList<T>: RecordedList<T> where T: IComparable
    {

    }
}
