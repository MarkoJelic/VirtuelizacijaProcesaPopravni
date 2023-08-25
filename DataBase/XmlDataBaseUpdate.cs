using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataBase
{
    public class XmlDataBaseUpdate
    {
        public XmlDataBaseUpdate()
        {
        }

        public static void UpdateDBLoad(List<Load> loads)
        {
            string savePath = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesaPopravni\DataBase\TBL_Load.xml";
            var xmlSavePath = new XElement("TBL_Load", from obj in loads
                                                       select new XElement("Object", new XElement("Id", obj.Id),
                                                                                     new XElement("TimeStamp", obj.TimeStamp),
                                                                                     new XElement("ForecastValue", obj.ForecastValue),
                                                                                     new XElement("MeasuredValue", obj.MeasuredValue),
                                                                                     new XElement("AbsolutePercentageDeviation", obj.AbsolutePercentageDeviation),
                                                                                     new XElement("SquaredDeviation", obj.SquaredDeviation),
                                                                                     new XElement("ImportedFileId", obj.ImportedFileId)));

            xmlSavePath.Save(savePath);
        }

        public static void UpdateDBAudit(List<Audit> audits)
        {
            string savePath = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesaPopravni\DataBase\TBL_Audit.xml";
            var xmlSavePath = new XElement("TBL_Audit", from obj in audits
                                                        select new XElement("Object", new XElement("Id", obj.Id),
                                                                                      new XElement("TimeStamp", obj.TimeStamp),
                                                                                      new XElement("MessageType", obj.MessageType),
                                                                                      new XElement("Message", obj.Message)));
            xmlSavePath.Save(savePath);
        }

        public static void UpdateDBImportedFile(List<ImportedFile> importedFiles)
        {
            string savePath = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\DataBase\TBL_Imported_File.xml";
            var xmlSavePath = new XElement("TBL_Imported_File", from obj in importedFiles
                                                        select new XElement("Object", new XElement("Id", obj.Id),
                                                                                      new XElement("FileName", obj.FileName)));
            xmlSavePath.Save(savePath);
        }
    }
}
