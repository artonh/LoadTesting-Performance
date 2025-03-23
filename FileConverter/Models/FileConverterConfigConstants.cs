using FileConverter.Converter;
using FileConverter.Models;

namespace FileConverter.LoadTesting
{
    public static class FileConverterConfigConstants
    {
        public static string BasePath = string.Empty;
        public static string BasePathForResult = string.Empty;
        public static string LogArtifacts = string.Empty;
        public static ConversionOptions Watermark = new();

        public static void LoadFrom(FileConverterConfig config)
        {
            string baseDir = AppContext.BaseDirectory;

            BasePath = ResolvePath(baseDir, config.BasePath);
            BasePathForResult = ResolvePath(baseDir, config.BasePathForResult);
            LogArtifacts = ResolvePath(baseDir, config.LogArtifacts);

            Watermark = config.Watermark ?? new ConversionOptions();
            if (!string.IsNullOrWhiteSpace(Watermark.ImagePath))
            {
                Watermark.ImagePath = ResolvePath(baseDir, Watermark.ImagePath);
            }
        }

        private static string ResolvePath(string baseDir, string configuredPath)
        {
            return Path.IsPathRooted(configuredPath)
                ? configuredPath
                : Path.GetFullPath(Path.Combine(baseDir, configuredPath));
        }
    }
}