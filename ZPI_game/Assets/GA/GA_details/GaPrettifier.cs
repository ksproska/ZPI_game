using System;
using System.Collections;

namespace GA
{
    public class GaPrettifier<T>: IGaPrettifier
    {
        private IGeneticAlgorithm<T> _geneticAlgorithm; // Można dać readonly

        public GaPrettifier(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            _geneticAlgorithm = geneticAlgorithm;
        }

        public string GetCurrentIterationLog()
        {
            return $"Iteration: {_geneticAlgorithm.GetIterationNumber():0000}" +
                   $" Best: {_geneticAlgorithm.GetBestScore():0.00}" +
                   $" Gap: {_geneticAlgorithm.NumbOfIterationsSincePrevChange():000}" + "\n";
        }

        public string GetCurrentIterationLogIfNewBestFound()
        {
            if (_geneticAlgorithm.WasNewBestDiscovered())
            {
                return $"{_geneticAlgorithm.GetIterationNumber(), 11}" +
                       $"\t{$"{_geneticAlgorithm.GetBestScore():0.00}", 7}" +
                       $"\t{$"{_geneticAlgorithm.GetDecreasePercentage():0.00}%", 8}" +
                       $"\t{_geneticAlgorithm.NumbOfIterationsSincePrevChange(), 3}" + "\n";
            }

            return "";
        }

        public string GetIterationLogHeader()
        {
            return $"{"ITERATION", 11}\t{"BEST", 7}\t{"DECREASE", 8}\t{"GAP", 3}\n";
        }
    }
}