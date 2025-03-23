# 🚀 Load Testing Tool

This console application simulates concurrent file-to-PDF conversions, capturing performance under different thread loads. Built with a focus on real-world testing needs, it supports both manual load testing and BenchmarkDotNet integration (manual approach proved faster in real use).

---

## 🔧 Features

- 🧵 Simulates parallel file conversions to PDF  
- ⚙️ Fully configurable via `appsettings.json`  
- 🗂️ Automatically duplicates input files for parallel testing  
- 🧹 Cleans up test artifacts after each run  
- 🛡️ Handles and logs memory-related and general exceptions  

---

## 📁 Configuration

Customize your test settings by updating the `appsettings.json`:

```json
{
  "LoadTest": {
    "FileTests": [
      {
        "FileName": "docx",
        "MaxParallelism": 8
      }
    ]
  },
  "FileConverterConfig": {
    "BasePath": "ConvertFiles",
    "BasePathForResult": "ConvertedFiles",
    "BenchmarkArtifacts": "BenchmarkArtifacts"
  }
}
