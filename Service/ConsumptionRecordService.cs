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
using System.Xml.Serialization;

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
            DeviationCalculator calculator = new DeviationCalculator();
            DeviationReporter reporter = new DeviationReporter(calculator);

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, MissingFieldFound = null };

            // Uzima sve .csv fajlove iz prosledjenog direktorijuma
            string[] files = FileDirUtil.GetAllFiles(csvFilesPath);
            List<Load> loads = new List<Load>();
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

                        List<Load> loadObjects = csv.GetRecords<Load>().ToList();
                        

                        var importedFile = new ImportedFile() { Id = cnt, FileName = fileName };
                        importedFiles.Add(importedFile);
                        
                        if (loadObjects.Count != 24)
                        {
                            var audit = new Audit() { Id = new Random().Next(), TimeStamp = DateTime.Now, MessageType = MsgType.Err, Message = "Neodgovarajuci broj sati." };
                            audits.Add(audit);
                        }
                        else
                        {
                            foreach (var lo in loadObjects)
                            {
                                if (loads.Any(x => x.TimeStamp == lo.TimeStamp))
                                {
                                    var index = loads.FindIndex(x => x.TimeStamp == lo.TimeStamp);
                                    if (loads[index].ForecastValue == 0)
                                    {
                                        loads[index].ForecastValue = lo.ForecastValue;
                                    }
                                    if (loads[index].MeasuredValue == 0)
                                    {
                                        loads[index].MeasuredValue = lo.MeasuredValue;
                                    }
                                }
                                else
                                {
                                    lo.AbsolutePercentageDeviation = double.NaN;
                                    lo.SquaredDeviation = double.NaN;
                                    lo.ImportedFileId = cnt;
                                    loads.Add(lo);
                                }
                            }

                        }
                        DataBase.XmlDataBaseUpdate.UpdateDBAudit(audits);
                        DataBase.XmlDataBaseUpdate.UpdateDBImportedFile(importedFiles);

                        cnt++;
                        // update DB?
                    }
                }
            }
            
            if (ConfigurationManager.AppSettings["deviation"] == "APD")
            {
                calculator.CalculateDeviation(loads, "APD");
            }
            else if (ConfigurationManager.AppSettings["deviation"] == "SD")
            {
                calculator.CalculateDeviation(loads, "SD");
            }


            // update DB
            DataBase.XmlDataBaseUpdate.UpdateDBLoad(loads);

            

        }

        public void GetDeviations()
        {
            string path = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesaPopravni\DataBase\TBL_Load.xml";

            // Deserijalizuje Xml fajl i napravi listu objekata
            List<Load> objects = new XmlSerialize().ConvertXmlToObjects<Load>(path);

            string fileName = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\Service\bin\Debug\Calculations.csv";

            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes(objects.ToString());

                    fs.Write(title, 0, title.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public FileManipulationResults GetFiles(FileManipulationOptions options)
        {
            Console.WriteLine("Dobavljanje datoteke poceci sa: \"{options.FileName}\"");
            return new GetFilesHandler(GetFilesQuery(options)).GetFiles();
        }

        private IQuery GetFilesQuery(FileManipulationOptions options)
        {
            return new FileSystemGetFilesQuery(options, path);
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
