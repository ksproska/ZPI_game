using Assets.GA.Utils;
using System;
using System.Collections.Generic;
namespace GA.mutations
{
    public class MutatorReverseSequence<T> : IMutator<T> where T: IComparable
    {
        private static readonly Random Random = new Random(); // nazewnictwo zmiennej niezgodne z konwencją
        public static List<T> ReversePartOrder(List<T> genotype, int startInx, int endInx)
        {
            RecordedList<T> recordedList = RecordedList<T>.Dummy;
            return ReversePartOrder(genotype, startInx, endInx, ref recordedList);
        }
        public static List<T> ReversePartOrder(List<T> genotype, int startInx, int endInx, ref RecordedList<T> recordedList)
        {
            var genotypeCopy = new List<T>(genotype);
            var len = endInx - startInx + 1;
            if (endInx < startInx)
            {
                len = (endInx - startInx + 1 + genotypeCopy.Count) % genotypeCopy.Count;
            } //to można chyba tak var len = endInx - startInx + 1 ? startInx <= endInx : (endInx - startInx + 1 + genotypeCopy.Count) % genotypeCopy.Count\
            
            for (int i = 0; i < (len + 1) / 2; i++)
            {
                var inx1 = (startInx + i) % genotype.Count;
                var inx2 = (endInx - i + genotypeCopy.Count) % genotype.Count;
                (genotypeCopy[inx1], genotypeCopy[inx2]) = (genotypeCopy[inx2], genotypeCopy[inx1]);
                if (recordedList is not DummyRecordedList<T>)
                {
                    (recordedList[inx1], recordedList[inx2]) = (recordedList[inx2], recordedList[inx1]);
                }
            }
    
            return genotypeCopy;
        }
        public List<T> Get(List<T> genotype)
        {
            var startInx = Random.Next(genotype.Count);
            var endInx = Random.Next(genotype.Count);
            return ReversePartOrder(genotype, startInx, endInx);
        }
    }
}