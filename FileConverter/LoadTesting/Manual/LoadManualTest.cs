namespace FileConverter.LoadTesting.Manual
{
    public class LoadManualTest(string inputName, string outputFileName) : BaseLoadManualTest, ILoadTest
    {
        public void Perform(int maxParallelism)
        {
            PerfomanceHelper.Run(
                maxParallelism,
                inputName,
                work: i =>
                {
                    PerformConversion(inputName, outputFileName, i + 1).GetAwaiter().GetResult();
                });

            //clean before next test
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Thread.Sleep(1000);
        }
    }
}
