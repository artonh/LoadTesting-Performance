using FileConverter;
using FileConverter.LoadTesting;
using FileConverter.LoadTesting.Manual;
using FileConverter.Models;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

var settings = config.GetSection("LoadTest").Get<LoadTestConfig>()!;
var fileConverterConfig = config
                          .GetSection("FileConverterConfig")
                          .Get<FileConverterConfig>()!;

FileConverterConfigConstants.LoadFrom(fileConverterConfig);

EnsureRequiredDirectories();

var mainOutPutLogFile = OuPutConsoleLogToFile(settings.FileTests);

Console.WriteLine("Load Testing for Syncfusion File Converter to PDF...!");
Console.WriteLine($"Logical CPU threads available: {Environment.ProcessorCount}");

foreach (var test in settings.FileTests)
{
    var fileName = test.FileName;
    var maxParallelism = test.MaxParallelism;

    Console.WriteLine($"\nRunning Load Test for: {fileName} with {maxParallelism} threads");

    try
    {
        if (!TryValidateSourceFile(fileName, out var sourcePath))
            return;

        CopyInputFilesIfNeeded(fileName, maxParallelism, sourcePath);

        ILoadTest loadTest = new LoadManualTest($"{fileName}{{0}}.{fileName}", $"{fileName}_syncfusion{{0}}.pdf");
        // ILoadTest loadTest = new LoadBenchmarkTest();

        loadTest.Perform(maxParallelism);

        CleanupTestFiles(fileName, deleteConvertedFiles: true);
    }
    catch (OutOfMemoryException ex)
    {
        Console.WriteLine($"OutOfMemoryException during test for: {fileName}");
        Console.WriteLine($"\n\nDetails: {ex.Message} {ex.InnerException}\n\n");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error during test for: {fileName}");
        Console.WriteLine($"\n\nDetails: {ex.Message} {ex.InnerException}\n\n");
    }
    finally
    {
        CleanupTestFiles(fileName, deleteConvertedFiles: true);
    }
} 

Console.WriteLine("\nAll conversions complete. Press ENTER to exit.");
Console.ReadLine();


#region helpers
string OuPutConsoleLogToFile(List<LoadTestFileConfig> fileTests)
{
    if (fileTests.Count == 0)
        return "";

    var logFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_LoadTest.log";
    var logFilePath = Path.Combine(FileConverterConfigConstants.LogArtifacts, logFileName);

    BaseOuPutConsoleLogToFile(logFilePath);

    var name = string.Join(", ", fileTests.Select(x => $"{x.MaxParallelism} {SanitizeFileName(x.FileName)}"));
    Console.WriteLine($"Will be converted: {name}");
    return logFilePath;
}

string SanitizeFileName(string name)
{
    foreach (char c in Path.GetInvalidFileNameChars())
    {
        name = name.Replace(c, '_');
    }
    return name;
}

void BaseOuPutConsoleLogToFile(string logFilePath)
{
    var logWriter = new StreamWriter(logFilePath, append: false)
    {
        AutoFlush = true
    };
    var originalConsoleOut = Console.Out;
    var multiWriter = new MultiTextWriter(originalConsoleOut, logWriter);
    Console.SetOut(multiWriter);
}

void EnsureRequiredDirectories()
{
    Directory.CreateDirectory(FileConverterConfigConstants.BasePathForResult);
    Directory.CreateDirectory(FileConverterConfigConstants.LogArtifacts);

    if (!Directory.Exists(FileConverterConfigConstants.BasePath))
    {
        Console.WriteLine("ConvertFiles directory does not exist!");
        Console.ReadLine();
        Environment.Exit(1);
    }
}

bool TryValidateSourceFile(string baseName, out string sourcePath)
{
    var firstFile = $"{baseName}.{baseName}";
    sourcePath = Path.Combine(FileConverterConfigConstants.BasePath, firstFile);

    if (!File.Exists(sourcePath))
    {
        Console.WriteLine($"Source file not found: {sourcePath}");
        Console.ReadLine();
        return false;
    }

    return true;
}

void CopyInputFilesIfNeeded(string baseName, int count, string source)
{
    var lastFile = Path.Combine(FileConverterConfigConstants.BasePath, $"{baseName}{count}.{baseName}");
    if (File.Exists(lastFile)) return;

    Console.WriteLine("Copying input files to simulate real files...");
    Parallel.For(1, count + 1, i =>
    {
        var target = Path.Combine(FileConverterConfigConstants.BasePath, $"{baseName}{i}.{baseName}");
        File.Copy(source, target, overwrite: true);
        Console.Write(".");
    });

    Console.WriteLine($"\n{count} files copied.");
}

void CleanupTestFiles(string baseName, bool deleteConvertedFiles)
{
    foreach (var file in Directory.GetFiles(FileConverterConfigConstants.BasePath, $"{baseName}*.{baseName}"))
    {
        var name = Path.GetFileNameWithoutExtension(file);
        if (!name.Equals(baseName, StringComparison.OrdinalIgnoreCase))
            File.Delete(file);
    }

    if (deleteConvertedFiles)
    {
        foreach (var file in Directory.GetFiles(FileConverterConfigConstants.BasePathForResult, $"{baseName}_syncfusion*.pdf"))
        {
            File.Delete(file);
        }
    }

    Console.WriteLine("Cleanup complete. All files are removed.");
}
#endregion
