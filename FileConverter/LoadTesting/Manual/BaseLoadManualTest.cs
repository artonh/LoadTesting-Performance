using FileConverter.Converter;
using FileConverter.Models;

namespace FileConverter.LoadTesting.Manual
{
    public abstract class BaseLoadManualTest
    {
        protected async Task PerformConversion(string inputFile, string outputFile, int i)
        {
            var outputPath = string.Format($"{FileConverterConfigConstants.BasePathForResult}{outputFile}", i);
            var inputPath = string.Format($"{FileConverterConfigConstants.BasePath}{inputFile}", i);

            await Converter.Converter.ToPdf(inputPath,
                outputPath,
                FileConverterConfigConstants.Watermark.Text,
                FileConverterConfigConstants.Watermark.ImagePath,
                !string.IsNullOrEmpty(FileConverterConfigConstants.Watermark?.ImagePath));
        }
    }
}