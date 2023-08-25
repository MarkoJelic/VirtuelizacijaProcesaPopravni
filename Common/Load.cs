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
    public class Load
    {
        [XmlElement("Id")]
        [DataMember]
        public int Id { get; set; }

        [XmlElement("TimeStamp")]
        [DataMember]
        public DateTime TimeStamp { get; set; }

        [XmlElement("ForecastValue")]
        [DataMember]
        public double ForecastValue { get; set; }

        [XmlElement("MeasuredValue")]
        [DataMember]
        public double MeasuredValue { get; set; }

        [XmlElement("AbsolutePercentageDeviation")]
        [DataMember]
        public double AbsolutePercentageDeviation { get; set; }

        [XmlElement("SquaredDeviation")]
        [DataMember]
        public double SquaredDeviation { get; set; }

        [XmlElement("ImportedFileId")]
        [DataMember]
        public int ImportedFileId { get; set; }
    }
}
