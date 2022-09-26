using System.Collections.Generic;

namespace GA
{
    public interface IMutator<T>
    {
        List<T> Get(List<T> genotype);
    }
}