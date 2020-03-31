using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TS.Data.Helper
{
    public class PDFHelper
    {
        /// <summary>
        /// wkhtmltopdf 调用浏览器内核实现，不支持多线程方式处理，所以通过信号量控制资源
        /// </summary>
        static AutoResetEvent myResetEvent = new AutoResetEvent(true);

        public void HtmlToPDF(string htmlSrc,string pdfSrc)
        {
            myResetEvent.WaitOne();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" -B 0 ");
            stringBuilder.Append(" -L 0 ");
            stringBuilder.Append(" -R 0 ");
            stringBuilder.Append(" -T 0 ");
            stringBuilder.Append(" --page-height 297 ");
            stringBuilder.Append(" --page-width 210 ");
            stringBuilder.Append(" --disable-smart-shrinking ");
            stringBuilder.Append(" " + $"{htmlSrc}" + " ");
            stringBuilder.Append(" " + $"{pdfSrc}" + " ");
            ProcessStartInfo processStartInfo = new ProcessStartInfo();

            processStartInfo.FileName = $@"{System.AppDomain.CurrentDomain.BaseDirectory}Tools\wkhtmltopdf.exe";
            processStartInfo.WorkingDirectory = Path.GetDirectoryName($@"{System.AppDomain.CurrentDomain.BaseDirectory}Tools\wkhtmltopdf.exe");
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.Arguments = stringBuilder.ToString();

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();

            process.Close();
            process.Dispose();

            myResetEvent.Set();
        }
    }
}
