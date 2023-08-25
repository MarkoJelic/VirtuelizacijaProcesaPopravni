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
    public class Audit
    {
        [XmlElement("Id")]
        [DataMember]
        public int Id { get; set; }

        [XmlElement("TimeStamp")]
        [DataMember]
        public DateTime TimeStamp { get; set; }

        [XmlElement("MessageType")]
        [DataMember]
        public MsgType MessageType { get; set; }

        [XmlElement("Message")]
        [DataMember]
        public string Message { get; set; }
    }
}
