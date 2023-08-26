using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public class XmlSerialize
    {
        public List<T> ConvertXmlToObjects<T>(string fileName)
        {
            List<T> lista;
            using (StreamReader reader = new StreamReader(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("TBL_Load"));
                lista = (List<T>)serializer.Deserialize(reader);
            }
            return lista;
        }
    }
}
