using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GA.Utils;

namespace GA
{
    public class CrosserPartiallyMatched : ICrosser<int>
    {
        private static readonly Random Random = new Random();

        public static List<int> Cross(List<int> parent1, List<int> parent2, int startInx, int segmentLength)
        {
            LabeledRecordedList<int, int> labeledRecordedList = LabeledRecordedList<int, int>.Dummy;
            return Cross(parent1, parent2, startInx, segmentLength, ref labeledRecordedList);
        }
        public static List<int> Cross(List<int> parent1, List<int> parent2, int startInx, int segmentLength, ref LabeledRecordedList<int, int> labeledRecordedList)
        {
            if (startInx + segmentLength >= parent1.Count) //TODO ugly
            {
                segmentLength = parent1.Count - 1 - startInx;
            }
            var child = Enumerable.Repeat(-1, parent1.Count).ToList(); //TODO change int -> T (generic)
            for (int i = 0; i < segmentLength; i++) {
                int inx = i + startInx;
                child[inx] = parent1[inx];
                if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                {
                    labeledRecordedList.SetLabeled(inx, parent1[inx], 0);
                }
            }
            for (int i = 0; i < segmentLength; i++) {
                int inx = i + startInx;
                int valToAdd = parent2[inx];
    
                if(!child.Contains(valToAdd)) {
                    while (child[inx] != -1)
                    {
                        inx = parent2.IndexOf(parent1[inx]);
                    }
                    child[inx] = valToAdd;
                    if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                    {
                        labeledRecordedList.SetLabeled(inx, valToAdd, 1);
                    }
                }
            }
            for (int i = 0; i < child.Count; i++) {
                if(child[i] == -1) {
                    child[i] = parent2[i];
                    if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                    {
                        labeledRecordedList.SetLabeled(i, parent2[i], 1);
                    }
                }
            }
            return child;
        }
        public List<int> Get(List<int> parent1, List<int> parent2)
        {
            var startInx = Random.Next(parent1.Count);
            var segmentLen = Random.Next(parent1.Count);
            return Cross(parent1, parent2, startInx, segmentLen);
        }
    }
}