using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class SelectorTournament : ISelector
    {
        private double SizePercentage;
        
        public static Individual GetTournament(List<Individual> generation, int size) {
            var selectedIndexes = SelectionCommonMethods.GetRandom(generation.Count, size);
            var selected = SelectionCommonMethods.GetForIndexes(generation, selectedIndexes);
            var selectedSorted = selected.OrderBy(o=>o.Score).ToList();
            //TODO size <= 0; generation.Count <= 0
            return selectedSorted[0];
        }
    
        public Individual Get(List<Individual> generation)
        {
            return GetTournament(generation, (int)(generation.Count * SizePercentage));
        }

        public void SetArgs(params double[] parameters)
        {
            if (parameters.Length < 1)
            {
                throw new Exception("Not enough params passed");
            }
            // TODO other exceptions
            SizePercentage = parameters[0];
        }
    }
}