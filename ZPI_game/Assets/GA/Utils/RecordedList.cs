using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GA.Utils
{
    public class RecordedList<T>
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
            history.Add(currentState);
        }

        public RecordedList(IEnumerable<T> args)
        {
            currentState = args.ToList();
            history = new List<List<T>>();
            history.Add(currentState);
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
        }

        private void SetElementAt(int index, T element)
        {
            currentState[index] = element;
            AddToHistory();
        }

        private void AddToHistory()
        {
            history.Add(new List<T>(currentState));
        }
    }

    public class DummyRecordedList<T>: RecordedList<T>
    {

    }
}
