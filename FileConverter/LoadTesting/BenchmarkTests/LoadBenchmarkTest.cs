using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace FileConverter.LoadTesting
{
    public class LoadBenchmarkTest: ILoadTest
    {

        public void Perform(int maxParallelism)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmm"); //yyyyMMdd-HHmmss


            var artifactsPath1 = Path.Combine(FileConverterConfigConstants.BenchmarkArtifacts, $"Run-{timestamp}");

            var job = Job.Default
                         .WithRuntime(CoreRuntime.Core80)
                         .WithGcServer(true)
                         .WithGcConcurrent(true)
                         .WithLaunchCount(1) //How many times will BenchmarkDotNet start from scratch to run this benchmark
                         .WithWarmupCount(1) // times do we run the method before measuring
                         .WithIterationCount(2) //How many times do we measure the performance
                         .WithInvocationCount(16); //how many times your method is called within a single iteration.
            /*
          Job explanation:
            Not Measured: LaunchCount * Warmup * Invocation
            Measured:     LaunchCount * IterationCount * Invocation

            LaunchCount:1, WarmupCount:1, IterationCount2, InvocationCount:16, makes in total 48 calls
                1 × 1 warmup × 16 = 16 calls (not measured)
                1 × 2 measured × 16 = 32 calls (measured)
         */

            var job1 = Job.Default
                          .WithRuntime(CoreRuntime.Core80)
                          .WithGcServer(true)
                          .WithGcConcurrent(true)
                          .WithLaunchCount(1)
                          .WithWarmupCount(1)
                          .WithIterationCount(5)       // Small but enough for variation
                          .WithInvocationCount(1)      // One full 100-thread run per iteration
                          .WithUnrollFactor(1)         // Required when InvocationCount is not divisible by 16
                          .WithId($"Threads{maxParallelism}-{timestamp}");

            var benchmarkConfig = DefaultConfig.Instance
                                               .WithOptions(ConfigOptions.DisableOptimizationsValidator) // Skip JIT optimizations check
                                               .WithArtifactsPath(artifactsPath1)
                                               .AddJob(
                                                   job1)
                                               .AddDiagnoser(MemoryDiagnoser.Default)
                                               .AddDiagnoser(ThreadingDiagnoser.Default);


            RunBenchmarkSafely<JpegParallelConversionBenchmark>();
            //RunBenchmarkSafely<TxtParallelConversionBenchmark>();

            void RunBenchmarkSafely<T>() where T : class
            {
                try
                {
                    BenchmarkRunner.Run<T>(benchmarkConfig);
                }
                catch (OutOfMemoryException ex)
                {
                    Console.WriteLine($"Out of memory during {typeof(T).Name}: {ex.Message}");
                }
            }
        }
    }
}
