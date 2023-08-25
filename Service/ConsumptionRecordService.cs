using Application;
using Common;
using CsvHelper;
using CsvHelper.Configuration;
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

        public ConsumptionRecordService(string path)
        {
            this.path = ConfigurationManager.AppSettings["path"];
        }

        public void CreateObjects(string csvFilesPath)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { /*HasHeaderRecord = false, OBRISATI */ MissingFieldFound = null };
            
            // Uzima sve .csv fajlove iz prosledjenog direktorijuma
            string[] files = FileDirUtil.GetAllFiles(csvFilesPath);
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                using (var reader = new StreamReader(fileName))
                {
                    using (var csv = new CsvReader(reader, configuration))
                    {
                        List<Audit> audits = new List<Audit>();
                        
                        try
                        {
                            List<Load> loadObjects = csv.GetRecords<Load>().ToList();
                            List<Load> results = new List<Load>();

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
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
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
