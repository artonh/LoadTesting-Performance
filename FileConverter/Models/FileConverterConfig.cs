using FileConverter.Converter;

namespace FileConverter.Models
{

    public class FileConverterConfig
    {
        public string BasePath { get; set; }
        public string BasePathForResult { get; set; }
        public string BenchmarkArtifacts { get; set; }
        public ConversionOptions Watermark { get; set; }
    }
}
