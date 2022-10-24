using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GA.Utils
{
    class RecordedList<T>
    {
        private List<List<T>> history;
        private List<T> currentState;

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

        private void SetElementAt(int index, T element)
        {
            currentState[index] = element;
            var newState = new List<T>(currentState);
            history.Add(newState);
        }
    }
}
