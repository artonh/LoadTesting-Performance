using FileConverter.Converter;
using FileConverter.Models;

namespace FileConverter.LoadTesting
{
    public static class FileConverterConfigConstants
    {
        public static string BasePath = string.Empty;
        public static string BasePathForResult = string.Empty;
        public static string BenchmarkArtifacts = string.Empty;
        public static ConversionOptions Watermark = new();

        public static void LoadFrom(FileConverterConfig config)
        {
            BasePath = config.BasePath;
            BasePathForResult = config.BasePathForResult;
            BenchmarkArtifacts = config.BenchmarkArtifacts;
            Watermark = config.Watermark;
        }
    }
}