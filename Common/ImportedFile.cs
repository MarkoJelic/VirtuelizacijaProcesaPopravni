using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    [XmlType("Object")]
    [DataContract]
    public class ImportedFile
    {
        [XmlElement("Id")]
        [DataMember]
        public int Id { get; set; }

        [XmlElement("FileName")]
        [DataMember]
        public string FileName { get; set; }
    }
}
