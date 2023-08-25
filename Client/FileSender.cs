using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class FileSender : IFileSender
    {
        private readonly string path;
        private readonly IConsumptionRecord proxy;
        private readonly IFileInUseChecker fileInUseChecker;
        private ConcurrentBag<string> importedFiles = new ConcurrentBag<string>();

        public FileSender(string path, IConsumptionRecord proxy, IFileInUseChecker fileInUseChecker)
        {
            this.path = path;
            this.proxy = proxy;
            this.fileInUseChecker = fileInUseChecker;
        }

        public void SendFile(string filePath)
        {
            // Proverava da li je fajl vec upload-ovan
            if (importedFiles.Contains(filePath))
            {
                Console.WriteLine($"Fajl {Path.GetFileName(filePath)} je vec upload-ovan.");
                DeleteFile(filePath);
                return;
            }
            
            // Uzima ime fajla iz putanje
            var fileName = Path.GetFileName(filePath);
            // Pravi novi objekat klase, prosledjuje MS i fileName
            FileManipulationOptions options = new FileManipulationOptions(GetMemoryStream(filePath), fileName);
            // Salje fajl servisu
            var result = proxy.SendFile(options);
            options.Dispose();

            if (result.ResultType == ResultTypes.Failed)
            {
                Console.WriteLine($"Upload-ovanje fajla {fileName} neuspesno. Error message: {result.ResultMessage}");
            }
            else
            {
                if (result.ResultType == ResultTypes.Warning)
                {
                    Console.WriteLine($"Upload-ovanje fajla {fileName} importovano sa upozorenjem: {result.ResultMessage}");
                }
                else
                {
                    Console.WriteLine($"Upload-ovanje fajla {fileName} importovano uspesno.");
                }
                importedFiles.Add(filePath);
            }
            DeleteFile(filePath);
        }

        private MemoryStream GetMemoryStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            if (fileInUseChecker.IsFileInUse(filePath))
            {
                Console.WriteLine($"Nemoguce obraditi fajl {Path.GetFileName(filePath)}. Koristi se u nekom drugom procesu ili je vec izbrisan.");
                return ms;
            }

            // Otvori se novi FileStream i u njega se ucita odredjeni fajl, zatim se on kopira u MemoryStream i potom se zatvori
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(ms);
                fileStream.Close();
            }
            return ms;
        }

        private void DeleteFile(string filePath)
        {
            if (fileInUseChecker.IsFileInUse(filePath))
            {
                Console.WriteLine($"Fajl {Path.GetFileName(filePath)} se ne moze obrisati. Koristi se u nekom drugom procesu ili je vec izbrisan.");
                return;
            }
            File.Delete(filePath);
        }

        public void SendFiles()
        {
            string[] files = FileDirUtil.GetAllFiles(path);
            foreach (string filePath in files)
            {
                SendFile(filePath);
            }
        }
    }
}
