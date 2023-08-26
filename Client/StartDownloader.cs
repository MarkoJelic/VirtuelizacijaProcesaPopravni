using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class StartDownloader : IDownloader
    {
        private readonly IConsumptionRecord proxy;
        private readonly string path;

        public StartDownloader(IConsumptionRecord proxy, string path)
        {
            this.proxy = proxy;
            this.path = path;
        }

        public void Download(string fileName)
        {
            var fileResults = proxy.GetFiles(new FileManipulationOptions(null, fileName));
            ResultTypes resultType = fileResults.ResultType;

            switch (resultType)
            {
                case ResultTypes.Failed:
                    Console.WriteLine($"Error: {fileResults.ResultMessage}");
                    break;
                case ResultTypes.Warning:
                    Console.WriteLine($"Warning: {fileResults.ResultMessage}");
                    break;
                default:
                    Console.WriteLine(fileResults.ResultMessage);
                    DownloadFiles(fileResults.MSs);
                    break;
            }
        }

        public void DownloadFiles(Dictionary<string, MemoryStream> MSs)
        {
            foreach (KeyValuePair<string, MemoryStream> stream in MSs)
            {
                DownloadFile(stream.Key, stream.Value);
            }
        }

        public void DownloadFile(string fileName, MemoryStream ms)
        {
            FileStream fs = new FileStream($"{ path }\\{fileName}", FileMode.Create, FileAccess.Write);
            ms.WriteTo(fs);
            fs.Close();
            fs.Dispose();
            ms.Dispose();
            Console.WriteLine($"Preuzeta datoteka {fileName}");
        }
    }
}
