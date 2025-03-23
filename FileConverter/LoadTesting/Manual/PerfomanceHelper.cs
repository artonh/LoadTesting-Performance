using System.Diagnostics;

namespace FileConverter.LoadTesting.Manual
{
    public static class PerfomanceHelper
    {
        public static void Run(int maxParallelism, string fileName, Action<int> work)
        {
            int gc0Before = GC.CollectionCount(0);
            int gc1Before = GC.CollectionCount(1);
            int gc2Before = GC.CollectionCount(2);
            long completedBefore = ThreadPool.CompletedWorkItemCount;
            long lockContentionsBefore = Monitor.LockContentionCount;

            var sw = Stopwatch.StartNew();

            long totalBefore = GC.GetTotalAllocatedBytes();

            Parallel.ForEach(
                Enumerable.Range(0, maxParallelism),
                new ParallelOptions { MaxDegreeOfParallelism = maxParallelism },
                i =>
                {
                    var swIter = Stopwatch.StartNew();

                    work(i);

                    swIter.Stop();

                    Console.WriteLine($"Iteration {i}: {Format.ElapsedTime(swIter.Elapsed.TotalSeconds)}");
                });

            sw.Stop();
            long totalAfter = GC.GetTotalAllocatedBytes();

            int gc0After = GC.CollectionCount(0);
            int gc1After = GC.CollectionCount(1);
            int gc2After = GC.CollectionCount(2);
            long completedAfter = ThreadPool.CompletedWorkItemCount;
            long lockContentionsAfter = Monitor.LockContentionCount;
            var completedWorkItems = completedAfter - completedBefore;
            var completedWorkItemsPerSecond = completedWorkItems / sw.Elapsed.TotalSeconds;
            var lockContentions = lockContentionsAfter - lockContentionsBefore;
            var lockContentionsPerSecond = lockContentions / sw.Elapsed.TotalSeconds;

            long totalAllocated = totalAfter - totalBefore;
            double avgPerIterBytes = totalAllocated / (double)maxParallelism;

            var memoryPerSecondBytes = totalAllocated / sw.Elapsed.TotalSeconds;
            var memoryPerSecondMB = memoryPerSecondBytes / (1024.0 * 1024);
            var memoryPerSecondGB = memoryPerSecondMB / 1024.0;


            double workItemsPerIteration = completedWorkItems / (double)maxParallelism;

            int gcTotal = (gc0After - gc0Before) + (gc1After - gc1Before) + (gc2After - gc2Before);
            double gcPerIter = gcTotal / (double)maxParallelism;

            double elapsedTimePerIteration = sw.Elapsed.TotalSeconds / maxParallelism;

            Console.WriteLine($"\nInput File: {fileName}, iteration: {maxParallelism}");
            Console.WriteLine("\n--------------------- Summary --------------------");
            Console.WriteLine("{0,-30} {1,15}", "Metric", "Value");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("{0,-30} {1,15}", "Total Mem. Allocated:", Format.Bytes(totalAllocated));
            Console.WriteLine("{0,-30} {1,15:F2}", "Memory per Second (MB/s):", memoryPerSecondMB);
            Console.WriteLine("{0,-30} {1,15:F2}", "Memory per Second (GB/s):", memoryPerSecondGB);

            Console.WriteLine("{0,-30} {1,15}", "Avg per Iteration:", Format.Bytes((long)avgPerIterBytes));
            Console.WriteLine("{0,-30} {1,15}", "GC Gen0:", gc0After - gc0Before);
            Console.WriteLine("{0,-30} {1,15}", "GC Gen1:", gc1After - gc1Before);
            Console.WriteLine("{0,-30} {1,15}", "GC Gen2:", gc2After - gc2Before);
            Console.WriteLine("{0,-30} {1,15:F2}", "GC Events per Iteration:", gcPerIter);
            Console.WriteLine("{0,-30} {1,15:F2}", "Total Completed Work Items:", completedWorkItems);
            Console.WriteLine("{0,-30} {1,15:F2}", "Total Completed Work Items/s:", completedWorkItemsPerSecond);
            Console.WriteLine("{0,-30} {1,15:F2}", "Total Lock Contentions:", lockContentions);
            Console.WriteLine("{0,-30} {1,15:F2}", "Total Lock Contentions/s:", lockContentionsPerSecond);
            Console.WriteLine("{0,-30} {1,15:F2}", "Work Items/ Iteration:", workItemsPerIteration);
            Console.WriteLine("{0,-30} {1,15}", "Total Elapsed Time:", Format.ElapsedTime(sw.Elapsed.TotalSeconds));
            Console.WriteLine("{0,-30} {1,15}", "Elapsed Time/iteration:", Format.ElapsedTime(elapsedTimePerIteration));
            Console.WriteLine(new string('-', 50));
        }
    }
}
