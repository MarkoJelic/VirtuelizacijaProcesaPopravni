using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        private static void GetDeviations()
        {
            // DOWNLOAD PUTANJA
            string downloadPath = ConfigurationManager.AppSettings["downloadPath"];
            // Proverava da li postoji putanja
            FileDirUtil.CheckCreatePath(downloadPath);

            ChannelFactory<IConsumptionRecord> channel = new ChannelFactory<IConsumptionRecord>("ConsumptionRecordService");
            IConsumptionRecord proxy = channel.CreateChannel();

            IDownloader downloader = GetDownloader(proxy, downloadPath);
            proxy.GetDeviations();
            downloader.Download("Calculations.csv");
            Console.WriteLine($"Ime datoteke Calculations, putnja {downloadPath}. Pritisnite bilo koji taster da se vratite u pocetni meni.");
            Console.ReadLine();
        }

        public static IDownloader GetDownloader(IConsumptionRecord proxy, string path)
        {
            return new StartDownloader(proxy, path);
        }


        public static void SendCsvFiles()
        {
            var uploadPath = ConfigurationManager.AppSettings["uploadPath"];
            // Proverava da li postoji putanja
            FileDirUtil.CheckCreatePath(uploadPath);

            ChannelFactory<IConsumptionRecord> channel = new ChannelFactory<IConsumptionRecord>("ConsumptionRecordService");
            // Kreiranje kanala tipa IConsumptionRecord
            IConsumptionRecord proxy = channel.CreateChannel();
            
            IUploader uploader = GetUploader(GetFileSender(uploadPath, proxy, GetFileInUseChecker()), uploadPath);
            // Salje fajlove servisu pomocu MS-a (Start poziva SendFiles koji poziva SendFile koji za svaki kopira iz fStream-a u memStream i tako salje preko proxy-ja)
            uploader.Start();

            // METODA ZA KREIRANJE OBJEKATA ************************** VAMO NASTAVITI
            proxy.CreateObjects(@"C:\Users\Marko\source\repos\VirtuelizacijaProcesaPopravni\Service\bin\Debug\");

            Console.WriteLine("Upload-er client je u upotrebi. Pritisnite bilo koji taster da se vratite u pocetni meni.");
            Console.ReadLine();
        }

        private static IFileSender GetFileSender(string uploadPath, IConsumptionRecord proxy, IFileInUseChecker fileInUseChecker)
        {
            return new FileSender(uploadPath, proxy, fileInUseChecker);
        }

        private static IFileInUseChecker GetFileInUseChecker()
        {
            return new FileInUseCommonChecker();
        }

        private static IUploader GetUploader(IFileSender fileSender, string uploadPath)
        {
            Console.WriteLine("Event Uploader se koristi trenutno.");
            return new EventUploader(fileSender, uploadPath);
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("'Send' to send CSV files");
            Console.WriteLine("'Get' to receive deviation calculations");
            Console.WriteLine("'Exit' to exit");
            Console.WriteLine("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "Send":
                    SendCsvFiles();
                    return true;
                case "Get":
                    GetDeviations();
                    return true;
                case "Exit":
                    return false;
                default:
                    return true;
            }
        }
    }
}
