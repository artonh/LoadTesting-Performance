using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;

namespace FileConverter.LoadTesting
{
    [MemoryDiagnoser] // Measures memory usage
    [GcServer(true)] // Forces server GC mode
    [ThreadingDiagnoser] // Captures thread contention & locks
    [SimpleJob(RuntimeMoniker.Net80)] // Specify .NET version
    [EtwProfiler] // Captures CPU & memory profiling
    //[EventPipeProfiler(EventPipeProfile.CpuSampling)] // Collects CPU usage samples
    //[InliningDiagnoser(logFailuresOnly: false, allowedNamespaces: new[] { "FileConverter" })]
    public class TxtParallelConversionBenchmark : FileParallelConversionBenchmark
    {
        protected override Dictionary<string, string> FilesToConvert => new Dictionary<string, string>
        {
            { "txt{0}.txt", "txt_syncfusion{0}.pdf" }
        };
    }
}