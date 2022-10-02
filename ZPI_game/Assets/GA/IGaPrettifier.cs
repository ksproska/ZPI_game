namespace GA
{
    public interface IGaPrettifier
    {
        string GetCurrentIterationLog();
        string GetCurrentIterationLogIfNewBestFound();
        string GetIterationLogHeader();
    }
}