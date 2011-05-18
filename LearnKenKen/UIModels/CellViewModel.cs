using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LearnKenKen.DomainModels;
using LearnKenKen.Utilities;

namespace LearnKenKen.UIModels
{
    class CellViewModel
    {
        public string Value { get; set; }
        public string TargetValue { get; set; }
        public string Operator { get; set; }
        public string CellId { get; set; }
        public string UnusableNumbers { get; set; }
        public string Possibilities { get; set; }              

        public bool BorderBottom { get; set; }
        public bool BorderTop { get; set; }
        public bool BorderLeft { get; set; }
        public bool BorderRight { get; set; }
        
        public CellViewModel(Cell cell)
        {
            CellId = cell.Id;
            Possibilities = Helper.GetSetDelimitedLineSepaPossibilities(cell.Cage.Possibilities, cell.UnusableNumbers);
            UnusableNumbers = Helper.GetNewlineSeparatedString(cell.UnusableNumbers);
            Operator = cell.Cage.Operation.GetString();
            TargetValue = cell.Cage.TargetValue.ToString();
            Value = cell.Value == 0 ? string.Empty : cell.Value.ToString();

            BorderTop = cell.Cage.Cells.FirstOrDefault(neighbour => neighbour.Column == cell.Column && neighbour.Row == cell.Row - 1) == null;
            BorderBottom = cell.Cage.Cells.FirstOrDefault(neighbour => neighbour.Column == cell.Column && neighbour.Row == cell.Row + 1) == null;
            BorderLeft = cell.Cage.Cells.FirstOrDefault(neighbour => neighbour.Row == cell.Row && neighbour.Column == cell.Column - 1) == null;
            BorderRight = cell.Cage.Cells.FirstOrDefault(neighbour => neighbour.Row == cell.Row && neighbour.Column == cell.Column + 1) == null;
        }       
    }
}