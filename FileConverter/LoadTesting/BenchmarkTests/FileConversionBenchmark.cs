using BenchmarkDotNet.Attributes;
using FileConverter.Converter;
namespace FileConverter.LoadTesting
{
    public abstract class FileConversionBaseBenchmark
    {
        protected static int _iterationCounter = 0;

        protected FileConversionMonitorOptions Model { get; } = new FileConversionMonitorOptions
        {
            BasePath = FileConverterConfigConstants.BasePath,
            BasePathForResult = FileConverterConfigConstants.BasePathForResult,
            Watermark = FileConverterConfigConstants.Watermark,
        };

        protected abstract Dictionary<string, string> FilesToConvert { get; }


        [ParamsSource(nameof(FileKeys))]
        public string FileKey { get; set; }
        public IEnumerable<string> FileKeys => FilesToConvert.Keys;
    }

    public abstract class FileParallelConversionBenchmark : FileConversionBaseBenchmark
    {
        [Params(400)]
        public int Iterations { get; set; }

        [Benchmark]
        public async Task RunParallelFileConversion()
        {
            string outputFile = FilesToConvert[FileKey];

            await Parallel.ForEachAsync(Enumerable.Range(0, Iterations),
                new ParallelOptions { MaxDegreeOfParallelism = 200 },
                async (i, _) =>
            {
                //var sw = Stopwatch.StartNew();
                //long before = GC.GetAllocatedBytesForCurrentThread();

                await PerformConversion(FileKey, outputFile, i + 1);

                //long after = GC.GetAllocatedBytesForCurrentThread();
                //long allocated = after - before;

                //var current = Interlocked.Increment(ref _iterationCounter);
                //Console.WriteLine($"Iteration {current}: {allocated / 1024 / 1024.0:F2} MB allocated");

                //sw.Stop();
                //var elapsed = sw.ElapsedMilliseconds;
                //var current = Interlocked.Increment(ref _iterationCounter);

                //Console.WriteLine($"--------- Measured Iteration {current}: {elapsed} ms");
            });
        }

        private async Task PerformConversion(string inputFile, string outputFile, int i)
        {
            var outputPath = string.Format($"{Model.BasePathForResult}{outputFile}", i);
            var inputPath = string.Format($"{Model.BasePath}{inputFile}", i);

            await Converter.Converter.ToPdf(inputPath,
                outputPath,
                Model.Watermark.Text,
                Model.Watermark.ImagePath,
                !string.IsNullOrEmpty(Model.Watermark?.ImagePath));
        }
    }

    public class FileConversionMonitorOptions
    {
        public string BasePath { get; set; }
        public string BasePathForResult { get; set; }
        public ConversionOptions Watermark { get; set; }
    }
}
