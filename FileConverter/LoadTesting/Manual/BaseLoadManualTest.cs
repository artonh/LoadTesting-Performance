namespace FileConverter.LoadTesting.Manual
{
    public abstract class BaseLoadManualTest
    {
        protected async Task PerformConversion(string inputFile, string outputFile, int i)
        {
            var inputPath = string.Format(Path.Combine(FileConverterConfigConstants.BasePath, inputFile), i);
            var outputPath = string.Format(Path.Combine(FileConverterConfigConstants.BasePathForResult, outputFile), i);

            await Converter.Converter.ToPdf(inputPath,
                outputPath,
                FileConverterConfigConstants.Watermark.Text,
                FileConverterConfigConstants.Watermark.ImagePath,
                !string.IsNullOrEmpty(FileConverterConfigConstants.Watermark?.ImagePath));
        }
    }
}