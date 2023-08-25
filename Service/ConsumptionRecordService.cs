using Application;
using Common;
using CsvHelper;
using CsvHelper.Configuration;
using DataBase;
using FileSystemManipulation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ConsumptionRecordService : IConsumptionRecord
    {
        private string path;

        public ConsumptionRecordService()
        {
            this.path = ConfigurationManager.AppSettings["path"];
            FileDirUtil.CheckCreatePath(path);
        }

        public void CreateObjects(string csvFilesPath)
        {
            //XmlDataBaseUpdate dataBase = new XmlDataBaseUpdate();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, MissingFieldFound = null };

            // Uzima sve .csv fajlove iz prosledjenog direktorijuma
            string[] files = FileDirUtil.GetAllFiles(csvFilesPath);
            List<Load> results = new List<Load>();
            List<Audit> audits = new List<Audit>();
            List<ImportedFile> importedFiles = new List<ImportedFile>();
            int cnt = 1;
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                using (var reader = new StreamReader(fileName))
                {
                    using (var csv = new CsvReader(reader, configuration))
                    {
                        //List<Audit> audits = new List<Audit>();

                        List<Load> loadObjects = csv.GetRecords<Load>().ToList();

                        var importedFile = new ImportedFile() { Id = cnt, FileName = fileName };
                        importedFiles.Add(importedFile);
                        cnt++;
                        if (loadObjects.Count != 24)
                        {
                            var audit = new Audit() { Id = new Random().Next(), TimeStamp = DateTime.Now, MessageType = MsgType.Err, Message = "Neodgovarajuci broj sati." };
                            audits.Add(audit);
                        }
                        else
                        {
                            foreach (var lo in loadObjects)
                            {
                                if (results.Any(x => x.TimeStamp == lo.TimeStamp))
                                {
                                    var index = results.FindIndex(x => x.TimeStamp == lo.TimeStamp);
                                    if (results[index].ForecastValue == 0)
                                    {
                                        results[index].ForecastValue = lo.ForecastValue;
                                    }
                                    if (results[index].MeasuredValue == 0)
                                    {
                                        results[index].MeasuredValue = lo.MeasuredValue;
                                    }
                                }
                                else
                                {
                                    results.Add(lo);
                                }
                            }

                        }
                        DataBase.XmlDataBaseUpdate.UpdateDBAudit(audits);
                        DataBase.XmlDataBaseUpdate.UpdateDBImportedFile(importedFiles);

                        // update DB?
                    }
                }
            }
            // POZIV METODE ZA PRORACUN DEVIJACIJE
            // update DB
            DataBase.XmlDataBaseUpdate.UpdateDBLoad(results);
        }

        public FileManipulationResults SendFile(FileManipulationOptions options)
        {
            // Poziva metodu InsertFile koja dalje poziva komandu Execute itd.
            Console.WriteLine($"Primanje datoteke sa imenom: \"{options.FileName}\"");
            return new InsertFileHandler(GetInsertFileCommand(options)).InsertFile();
        }

        private ICommand GetInsertFileCommand(FileManipulationOptions options)
        {
            return new FileSystemInsertFileCommand(options, path);
        }
    }
}
