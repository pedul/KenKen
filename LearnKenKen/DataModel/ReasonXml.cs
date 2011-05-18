using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using LearnKenKen.DomainModels;
using LearnKenKen.Utilities;

namespace LearnKenKen.DataModel
{
    [Serializable]
    [XmlType("Reason")]
    public class ReasonXml
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlElement("Why")]
        public string Why { get; set; }

        [XmlElement("WhySingular")]
        public string WhySingular { get; set; }

        [XmlElement("WhyPlural")]
        public string WhyPlural { get; set; }

        [XmlElement("Detail")]
        public string Detail { get; set; }

        [XmlElement("DetailSingular")]
        public string DetailSingular { get; set; }

        [XmlElement("DetailPlural")]
        public string DetailPlural { get; set; }        
    }
}