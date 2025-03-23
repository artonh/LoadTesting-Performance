using FileConverter.Converter;

namespace FileConverter.Models
{
    public class LoadTestFileConfig
    {
        public string FileName { get; set; }
        public int MaxParallelism { get; set; }
    }

    public class LoadTestConfig
    {
        public List<LoadTestFileConfig> FileTests { get; set; } = new();
    }
}
