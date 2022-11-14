using Assets.GA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class CrosserCycle: ICrosser<int>
    {
        private static readonly Random Random = new Random();

        public static List<List<int>> GetCycles(List<int> parent1, List<int> parent2)
        {
            var cycles = new List<List<int>>();
            var currentInx = 0;
            var collectedIndexes = new List<int>(){};

            while (parent1.Count !=  collectedIndexes.Count)
            {
                var cycle_beginning = parent1[currentInx];
                var current_cycle = new List<int>() { currentInx };

                while (true)
                {
                    var nextInx = parent1.IndexOf(parent2[current_cycle[^1]]);
                    current_cycle.Add(nextInx);
                    collectedIndexes.Add(nextInx);

                    if (parent1[current_cycle[^1]] == cycle_beginning)
                    {
                        current_cycle.RemoveAt(current_cycle.Count - 1);
                        break;
                    }
                }
                cycles.Add(current_cycle);
                while (collectedIndexes.Contains(currentInx))
                {
                    currentInx += 1;
                }
            }
            return cycles;
        }

        public static List<int> Cross(List<int> parent1, List<int> parent2)
        {
            LabeledRecordedList<int, int> dummy = LabeledRecordedList<int, int>.Dummy;
            return Cross(parent1, parent2, ref dummy);
        }
        public static List<int> Cross(List<int> parent1, List<int> parent2, ref LabeledRecordedList<int, int> labeledRecordedList)
        {
            var cycles = GetCycles(parent1, parent2);
            var child = Enumerable.Repeat(-1, parent1.Count).ToList();
            for (int i = 0; i < cycles.Count; i++)
            {
                foreach (var inx in cycles[i])
                {
                    if (i % 2 == 0)
                    {
                        child[inx] = parent1[inx];
                        if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                        {
                            labeledRecordedList.SetLabeled(inx, parent1[inx], 0);
                        }
                    }
                    else
                    {
                        child[inx] = parent2[inx];
                        if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                        {
                            labeledRecordedList.SetLabeled(inx, parent2[inx], 1);
                        }
                    }
                }
            }
            return child;
        }
        public List<int> Get(List<int> parent1, List<int> parent2)
        {
            return Cross(parent1, parent2);
        }
    }
}
