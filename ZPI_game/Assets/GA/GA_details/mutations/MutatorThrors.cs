using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GA.Utils;

namespace GA.mutations
{
    public class MutatorThrors<T> : IMutator<T> where T : IComparable
    {
        private static readonly Random Random = new Random(); // nazewnictwo zmiennej niezgodne z konwencją

        public static List<T> SwipeOneRight(List<T> genotype, List<int> indexes)
        {
            RecordedList<T> recordedList = RecordedList<T>.Dummy;
            return SwipeOneRight(genotype, indexes, ref recordedList);
        }

        public static List<T> SwipeOneRight(List<T> genotype, List<int> indexes, ref RecordedList<T> recordedList)
        {
            var genotypeCopy = new List<T>(genotype);
            var temp = genotype[indexes[^1]];
            for (int i = 0; i < indexes.Count - 1; i++)
            {
                genotypeCopy[indexes[i + 1]] = genotype[indexes[i]];
                if (recordedList is not DummyRecordedList<T>)
                {
                    recordedList[indexes[i + 1]] = genotype[indexes[i]];
                }
            }

            genotypeCopy[indexes[0]] = temp;
            if (recordedList is not DummyRecordedList<T>)
            {
                recordedList[indexes[0]] = temp;
            }

            return genotypeCopy;
        }

        public List<T> Get(List<T> genotype)
        {
            var numbOfIndexesToSwipe = Random.Next(genotype.Count - 2) + 1;
            var indexes = SelectionCommonMethods.GetRandom(genotype.Count, numbOfIndexesToSwipe).ToList();
            return SwipeOneRight(genotype, indexes);
        }
    }
}