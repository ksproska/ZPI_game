using System;
using System.Collections.Generic;

namespace GA
{
    public interface IMutator<T> where T: IComparable
    {
        List<T> Get(List<T> genotype);
    }
}