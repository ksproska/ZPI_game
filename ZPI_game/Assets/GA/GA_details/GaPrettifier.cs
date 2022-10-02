namespace GA
{
    public class GaPrettifier<T>: IGaPrettifier
    {
        private IGeneticAlgorithm<T> _geneticAlgorithm;
        private const int padding = 8;

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
                return $"{_geneticAlgorithm.GetIterationNumber(), padding}" +
                       $"\t{$"{_geneticAlgorithm.GetBestScore():0.00}", padding}" +
                       $"\t{$"{_geneticAlgorithm.GetDecreasePercentage():0.00}%", padding}" +
                       $"\t{_geneticAlgorithm.NumbOfIterationsSincePrevChange(), padding}" + "\n";
            }

            return "";
        }

        public string GetIterationLogHeader()
        {
            return $"{"ITERATION", padding}\t{"BEST", padding}\t{"DECREASE", padding}\t{"GAP", padding}\n";
        }
    }
}