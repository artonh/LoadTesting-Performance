using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter
{
    internal class FileConverterToolModel
    {
        public string BasePath{ get; set; }
        public string BasePathForResult { get; set; }
        public string WatermarkImagePath { get; set; }
        public string WatermarkText { get; set; }
        public string DriveLetter { get; set; }
    }
}
