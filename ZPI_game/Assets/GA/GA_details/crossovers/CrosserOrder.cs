using Assets.GA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class CrosserOrder: ICrosser<int>
    {
        private static readonly Random Random = new Random(); // nazewnictwo zmiennej niezgodne z konwencją

        public static List<int> Cross(List<int> parent1, List<int> parent2, int startInx, int segmentLength)
        {
            LabeledRecordedList<int, int> labeledRecordedList = LabeledRecordedList<int, int>.Dummy;
            return Cross(parent1, parent2, startInx, segmentLength, ref labeledRecordedList);
        }

        public static List<int> Cross(List<int> parent1, List<int> parent2, int startInx, int segmentLength, ref LabeledRecordedList<int, int> labeledRecordedList ) //Czy startInx nie lepiej nazwać startOffset? Bardziej jasne jak dla mnie
        {
            if (startInx + segmentLength >= parent1.Count) //TODO ugly, można wywalić, jeżeli zrobi się to tak jak zaproponowałem w Get
            {
                segmentLength = parent1.Count - 1 - startInx;
            }
            var child = Enumerable.Repeat(-1, parent1.Count).ToList(); //TODO change int -> T (generic), jeżeli tutaj miałoby być generycznie, to chyba pusty element to null
            for (int i = 0; i < segmentLength; i++) {
                int inx = i + startInx;
                child[inx] = parent1[inx];
                if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                {
                    labeledRecordedList.SetLabeled(inx, parent1[inx], 0);
                }
            }

            int lastNotContained = 0;
            for (int i = 0; i < child.Count; i++) {
                if(child[i] == -1) {
                    for (int j = lastNotContained; j < parent2.Count; j++) //ja bym to wydzielił do osobnej funkcji, tak jak w przypadku CrosserOrder.py
                    {
                        lastNotContained = j + 1;
                        if (!child.Contains(parent2[j]))  
                        {
                            child[i] = parent2[j];
                            if (labeledRecordedList is not DummyLabeledRecordedList<int, int>)
                            {
                                labeledRecordedList.SetLabeled(i, parent2[j], 1);
                            }
                            break; //todo ugly
                        }
                    }
                }
            }
            return child;
        }
        public List<int> Get(List<int> parent1, List<int> parent2)
        {
            var startInx = Random.Next(parent1.Count);
            var segmentLen = Random.Next(parent1.Count); // Gdyby dać parent1.Count - startInx w argumencie, wtedy początek w Cross do wywalenia
            return Cross(parent1, parent2, startInx, segmentLen);
        }
    }
}