using System.Collections.Generic;

namespace GA
{
    public interface IGeneticAlgorithm<T>
    {
        void RunIteration();
        int GetIterationNumber();
        double GetBestScore();
        double GetBestForIterationScore();
        List<T> GetBestGenotype();
    }
}