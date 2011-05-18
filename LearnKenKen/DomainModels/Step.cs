using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnKenKen.DomainModels
{
    public class Step
    {
        internal Cell FoundCell { get; set; }
        internal Dimension Dimension { get; set; }
        internal int DimensionIndex { get; set; }
        internal int CellValue { get; set; }
        internal List<Possibility> FoundPossibilities { get; set; }
        internal List<Possibility> CagePossibilities { get; set; }
        internal List<Possibility> InvalidPossibilities { get; set; }
        internal List<int> InvalidNeighbourNumbers { get; set; }
        internal List<Cell> InvalidNeighbours { get; set; }
        internal List<Cell> AffectedNeighboursCells { get; set; }
        internal int CageValue { get; set; }
        internal List<Cell> FoundCageCells { get; set; }        
    }
}