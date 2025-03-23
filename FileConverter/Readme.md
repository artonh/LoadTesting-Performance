It simulates concurrent conversions and captures performance under different thread loads.

Features
Simulates parallel file conversions to PDF

Supports custom configuration via appsettings.json

Automatically copies input files for simulation

Cleans up test artifacts after each run

Handles and reports memory-related and general exceptions

Configuration
Update appsettings.json with your test settings:

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


Usage
Place the initial source files (e.g., docx.docx) in the ConvertFiles folder.

Run the console application.

Monitor console output for performance logs and results.

Notes
The application will generate multiple copies of the source file to simulate parallel conversion.

Output PDF files and temporary files are cleaned up after each test.

If a source file is missing, the test will exit early with a message.

Requirements
.NET 8.0 SDK

Syncfusion File Converter libraries