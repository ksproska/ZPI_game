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

    ///<summary>A list that remembers all the modifications</summary>
    public class RecordedList<T> where T: IComparable
    {
        private List<(int, T)> history;
        private List<T> currentState;

        /// <summary>
        /// Returns the history of modyfications as a list of pairs: index of the
        /// value that has changed and the changed value.
        /// </summary>
        public List<(int, T)> History => history;
        /// <summary>
        /// Retrurns the current state of the list.
        /// </summary>
        public List<T> CurrentState => currentState;

        public static DummyRecordedList<T> Dummy => new DummyRecordedList<T>();

        ///<summary>Creates an empty recorded list. Initial state is
        ///saved in history.</summary>
        public RecordedList()
        {
            currentState = new List<T>();
            history = new List<(int, T)>();
        }

        ///<summary>Creates a <c>RecordedList</c> that contains all the elements of
        ///the given <c>IEnumerable</c>. Initial state is saved in history</summary>
        ///<param></param>
        public RecordedList(IEnumerable<T> args)
        {
            currentState = args.ToList();
            history = new List<(int, T)>();
        }

        public T this[int index]
        {
            set => SetElementAt(index, value);
            get => currentState[index];
        }

        /// <summary>
        /// Adds an element at the end of the list.
        /// </summary>
        /// <param name="elem">An element that is to be added.</param>
        public void Add(T elem)
        {
            currentState.Add(elem);
            history.Add((currentState.Count - 1, elem));
        }

        /// <summary>
        /// Adds all the elements of the given <c>IEnumerable</c> to the <c>RecordedList.</c>
        /// </summary>
        /// <param name="args"></param>
        public void AddRange(IEnumerable<T> args)
        {
            int beforeAddCount = currentState.Count;
            currentState.AddRange(args);

            args.Select((v, i) => (i, v))
                .ToList()
                .ForEach(elem => history.Add((beforeAddCount + elem.i, elem.v)));
        }

        private void SetElementAt(int index, T element)
        {
            currentState[index] = element;
            history.Add((index, element));
        }

        public List<(int, T)> GetFullHistory()
        {
            var unchanged = GetUnchangedIndices().Select(index => (index, currentState[index])).ToList();
            var ret = new List<(int, T)>(history);
            ret.AddRange(unchanged);
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

        //private List<(int, T)> GetSingleDifference(int historyStartIndex)
        //{
        //    var ret = new List<(int, T)>();
        //    var his1 = history[historyStartIndex];
        //    var his2 = history[historyStartIndex + 1];

        //    for(int i = 0; i < his1.Count; i++)
        //    {
        //        if (his1[i].CompareTo(his2[i]) != 0)
        //        {
        //            ret.Add((i, his2[i]));
        //        }
        //    }
        //    if(his2.Count > his1.Count)
        //    {
        //        for(int i = 0; i < his2.Count - his1.Count; i++)
        //        {
        //            ret.Add((i + his1.Count, his2[i + his1.Count]));
        //        }
        //    }

        //    return ret;
        //}

        //private void AddToHistory()
        //{
        //    List<T> newList = new List<T>();
        //    newList.AddRange(currentState);
        //    history.Add(newList);
        //}
    }

    public class DummyRecordedList<T>: RecordedList<T> where T: IComparable
    {

    }
}
