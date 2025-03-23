using System.Buffers;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Presentation;
using Syncfusion.PresentationRenderer;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System.Runtime;

namespace FileConverter.Converter
{
    public class ConversionOptions
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }
    }

    public static class Converter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="watermarkImagePath">image path that is supposed to be in Watermark</param>
        public static async Task ToPdf(string inputPath, string outputPath, string watermarkText, string watermarkImagePath = "", bool addWaterMark = false)
        {
             //DO THE JOB
        }
    }
}