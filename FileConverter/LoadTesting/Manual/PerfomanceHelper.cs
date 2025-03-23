using System.Diagnostics;

namespace FileConverter.LoadTesting.Manual
{
    public static class PerfomanceHelper
    {
        public static void Run(int maxParallelism, string fileName, Action<int> work)
        {
            long[] allocations = new long[maxParallelism];

            int gc0Before = GC.CollectionCount(0);
            int gc1Before = GC.CollectionCount(1);
            int gc2Before = GC.CollectionCount(2);
            long completedBefore = ThreadPool.CompletedWorkItemCount;
            long lockContentionsBefore = Monitor.LockContentionCount;

            var sw = Stopwatch.StartNew();

            Parallel.ForEach(
                Enumerable.Range(0, maxParallelism),
                new ParallelOptions { MaxDegreeOfParallelism = maxParallelism },
                i =>
                {
                    var swBody = Stopwatch.StartNew();
                    long before = GC.GetAllocatedBytesForCurrentThread();

                    work(i); // sync logic or async wrapped via .GetResult()

                    long after = GC.GetAllocatedBytesForCurrentThread();
                    sw.Stop();
                    allocations[i] = after - before;

                    var elapsedSec = sw.Elapsed.TotalSeconds;

                    Console.WriteLine($"Iteration {i}: {allocations[i] / 1024.0 / 1024.0:F2} MB allocated,  {elapsedSec:F2} s");
                });

            sw.Stop();

            int gc0After = GC.CollectionCount(0);
            int gc1After = GC.CollectionCount(1);
            int gc2After = GC.CollectionCount(2);
            long completedAfter = ThreadPool.CompletedWorkItemCount;
            long lockContentionsAfter = Monitor.LockContentionCount;

            long totalAllocated = allocations.Sum();
            double avgPerIter = totalAllocated / (double)maxParallelism / 1024.0 / 1024.0;

            Console.WriteLine($"\nInput File: {fileName}"); 
            Console.WriteLine("\n--------------------- Summary --------------------");
            Console.WriteLine("{0,-30} {1,15}", "Metric", "Value");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("{0,-30} {1,15:F2} MB", "Total Allocated:", totalAllocated / 1024.0 / 1024.0);
            Console.WriteLine("{0,-30} {1,15:F2} MB", "Avg per Iteration:", avgPerIter);
            Console.WriteLine("{0,-30} {1,15}", "GC Gen0:", gc0After - gc0Before);
            Console.WriteLine("{0,-30} {1,15}", "GC Gen1:", gc1After - gc1Before);
            Console.WriteLine("{0,-30} {1,15}", "GC Gen2:", gc2After - gc2Before);
            Console.WriteLine("{0,-30} {1,15}", "Completed Work Items:", completedAfter - completedBefore);
            Console.WriteLine("{0,-30} {1,15}", "Lock Contentions:", lockContentionsAfter - lockContentionsBefore);
            Console.WriteLine("{0,-30} {1,15:F2} s", "Elapsed Time:", sw.Elapsed.TotalSeconds);
            Console.WriteLine(new string('-', 50));
        }
    }
}
