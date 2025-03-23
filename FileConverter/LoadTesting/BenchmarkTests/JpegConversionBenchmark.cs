using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;

namespace FileConverter.LoadTesting
{
    //[MemoryDiagnoser] // Measures memory usage
    //[GcServer(true)] // Forces server GC mode
    //[ThreadingDiagnoser] // Captures thread contention & locks

    //[SimpleJob(RuntimeMoniker.Net80, 
    //    launchCount: 1, //How many times will BenchmarkDotNet start from scratch to run this benchmark
    //    warmupCount: 1,  // times do we run the method before measuring
    //    iterationCount: 2 , //How many times do we measure the performance
    //    invocationCount: 3  //how many times your method is called within a single iteration.
    //     )]

    /*
    c1: 1,1,50,100  = 5,100 calls  (Time: 90ms × 5100 or ~7.6m)

     C2:
        10 launches × (20 warmups + 50 measurements) × 100 invocations
       = 70,000 method calls (50,000 of which are measured)  ... 24h
     */
    //[EtwProfiler] // (Creates another job) Captures CPU & memory profiling
    //[EventPipeProfiler(EventPipeProfile.CpuSampling)] //(Creates another job) Collects CPU usage samples

    //[InliningDiagnoser(logFailuresOnly: false, allowedNamespaces: new[] { "FileConverter" })]
    //[DisassemblyDiagnoser(maxDepth: 1, printSource: true)]
    public class JpegParallelConversionBenchmark : FileParallelConversionBenchmark
    {
        protected override Dictionary<string, string> FilesToConvert => new Dictionary<string, string>
        {
            { "jpeg{0}.jpeg", "jpeg_syncfusion{0}.pdf" }
        };
    }
    
    [MemoryDiagnoser] // Measures memory usage
    [GcServer(true)] // Forces server GC mode
    //[ThreadingDiagnoser] // Captures thread contention & locks
    [SimpleJob(RuntimeMoniker.Net80)] // Specify .NET version
    [EtwProfiler] // Captures CPU & memory profiling
    //[EventPipeProfiler(EventPipeProfile.CpuSampling)] // Collects CPU usage samples
    //[InliningDiagnoser(logFailuresOnly: false, allowedNamespaces: new[] { "FileConverter" })]
    //[DisassemblyDiagnoser(maxDepth: 1, printSource: true)]
    public class JpegConversionBenchmark : FileParallelConversionBenchmark
    {
        protected override Dictionary<string, string> FilesToConvert => new Dictionary<string, string>
        {
            { "jpeg.jpeg", "jpeg_syncfusion.pdf" }
        };
    }
}