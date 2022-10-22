using System;
using System.Collections.Generic;

namespace GA
{
    public class WeightsGrid
    {
        private List<List<double>> _weights; // Nie lepiej przechowywać macierzy wag jako dwuwymiarowa tablica/tablica tablic?
        public int Size { get; }

        public WeightsGrid(List<List<double>> grid)
        {
            _weights = grid;

            // Wydzieliłbym dodatkowe metody do walidacji, np. ValidateOnDiagonal, ValidateSize, ValidateSymetry 
            for (int i = grid.Count - 1; i >= 0; i--)
            {
                if (grid[i].Count != grid.Count)
                {
                    throw new InvalidOperationException();
                }
                if (grid[i][i] != 0)
                {
                    throw new InvalidOperationException();
                }
            }

            for (int i = grid.Count - 1; i >= 0; i--)
            {
                for (int j = grid.Count - 1; j >= 0; j--)
                {
                    if (Math.Abs(grid[i][j] - grid[j][i]) > 0.01)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            Size = _weights.Count;
        }

        public double GetWeight(int inx1, int inx2)
        {
            return _weights[inx1][inx2];
        }
    }
}
