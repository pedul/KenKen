using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LearnKenKen.DataModel
{
    [Serializable]
    [XmlType("Grid")]
    public class GridXml
    {
        public GridXml() { }

        [XmlAttribute("Name")]
        public string DisplayName { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlArray("Cages")]
        public List<CageXml> Cages { get; set; }
    }
}
