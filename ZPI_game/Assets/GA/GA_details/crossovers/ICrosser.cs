using System.Collections.Generic;

namespace GA
{
    public interface ICrosser<T>
    {
        List<T> Get(List<T> parent1, List<T> parent2);
    }
}