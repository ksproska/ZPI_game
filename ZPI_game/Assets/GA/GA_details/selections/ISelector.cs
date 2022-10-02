using System.Collections.Generic;

namespace GA
{
    public interface ISelector
    {
        Individual Get(List<Individual> generation);

        void SetArgs(params double[] parameters);
        string[] GetArgs();
    }
}