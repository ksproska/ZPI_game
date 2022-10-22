using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class SelectorTournament : ISelector
    {
        private double SizePercentage; // nazewnictwo zmiennej niezgodne z konwencją
        
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
            if (parameters.Length < GetArgs().Length)
            {
                throw new Exception("Not enough params passed");
            }
            // TODO other exceptions
            // Pytanie, czy nie lepiej rzucić tutaj ArgumentException, sprawdzając, czy parameters.Length == GetArgs().Length
            // Jeżeli SizePercentage nie jest z zakresu (0, 1), można rzucić ArgumentOutOfRangeException
            SizePercentage = parameters[0];
        }

        public string[] GetArgs()
        {
            return new string[] { "percentage for tournament" };
        }
    }
}