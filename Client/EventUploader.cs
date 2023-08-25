using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class EventUploader : IUploader
    {
        private FileSystemWatcher inputFileWatcher;
        private readonly IFileSender fileSender;

        public EventUploader(IFileSender fileSender, string path)
        {
            CreateFileSystemWatcher(path);
            this.fileSender = fileSender;
        }

        // Kreira i parametrizuje obj inputFileWatcher koji osluskuje file system na prosledjenoj lokaciji,
        // ukoliko je na prosl. lokaciju dodata nova datoteka metoda FileCreated se poziva
        private void CreateFileSystemWatcher(string path)
        {
            inputFileWatcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = false,
                InternalBufferSize = 32768, // 32 KB
                Filter = "*.*", // default
                NotifyFilter = NotifyFilters.FileName
            };
            inputFileWatcher.Created += FileCreated;
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;
            try
            {
                fileSender.SendFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Start()
        {
            fileSender.SendFiles();
            inputFileWatcher.EnableRaisingEvents = true;
        }
    }
}
