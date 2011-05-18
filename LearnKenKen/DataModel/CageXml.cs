using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LearnKenKen.DataModel
{
    [Serializable]
    [XmlType("Cage")]
    public class CageXml
    {
        public CageXml() { }

        [XmlAttribute("TargetValue")]
        public int TargetValue { get; set; }

        [XmlAttribute("Operation")]
        public string Operation { get; set; }

        [XmlArray("Cells")]
        public List<CellXml> Cells { get; set; }
    }
}