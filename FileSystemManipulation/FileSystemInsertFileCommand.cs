using Application;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemManipulation
{
    public class FileSystemInsertFileCommand : ICommand
    {
        private readonly FileManipulationOptions options;
        private readonly string path;

        public FileSystemInsertFileCommand(FileManipulationOptions options, string path)
        {
            this.options = options;
            this.path = path;
        }

        // Poziva se iz InsertFileHandler-a i uzima podatke iz MS i pise ih u fs pomocu InsertFile metode
        public void Execute()
        {
            if (options.MS == null || options.MS.Length == 0)
            {
                throw new IncorrectDataException("Memory stream ne sadrzi podatke!");
            }
            string filePath = $"{path}\\{options.FileName}";
            InsertFile(filePath, options.MS);
            options.Dispose();
        }

        void InsertFile(string filePath, MemoryStream ms)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(fs);
                fs.Close();
                fs.Dispose();
                ms.Dispose();
            }
        }
    }
}
