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
    public class FileSystemGetFilesQuery : IQuery
    {
        private readonly FileManipulationOptions options;
        private readonly string path;

        public FileSystemGetFilesQuery(FileManipulationOptions options, string path)
        {
            this.options = options;
            this.path = path;
        }

        public FileManipulationResults GetResults()
        {
            var results = new FileManipulationResults();
            string[] files = Directory.GetFiles(path);
            foreach (string filePath in files)
            {
                AddMemoryStream(filePath, results);
            }
            return results;
        }


        // Ako ime fajla pocinje sa imenom datoteke iz opcija, cita se datoteka, izdvaja memory stream i dodaje se u rezultate
        private void AddMemoryStream(string filePath, FileManipulationResults results)
        {
            var fileName = Path.GetFileName(filePath);

            if (Path.GetFileName(fileName).StartsWith(options.FileName, StringComparison.CurrentCultureIgnoreCase))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    MemoryStream ms = new MemoryStream();
                    fs.CopyTo(ms);
                    results.MSs.Add(fileName, ms);
                }
            }
        }
    }
}
