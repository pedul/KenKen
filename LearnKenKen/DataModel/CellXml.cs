using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LearnKenKen.DataModel
{
    [Serializable]
    [XmlType("Cell")]
    public class CellXml
    {
        [XmlAttribute("Row")]
        public int Row { get; set; }

        [XmlAttribute("Column")]
        public int Column { get; set; }

        public CellXml() { }
    }
}