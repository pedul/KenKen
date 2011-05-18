using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnKenKen.DomainModels
{
    enum Dimension
    {
        Row,
        Column
    }

    class Cell
    {
        private HashSet<int> _unusableNumbers;

        public int Value { get; set; }        

        public Cage Cage { get; internal set; }

        public int Row { get; set; }
        
        public int Column { get; set; }

        public string Id { get; internal set; }

        public int Index { get; set; }

        public bool Solved { get { return (Value > 0); } }

        public bool IsValueFeasible(int value)
        {
            return PossibleValues.Contains(value);
        }

        public HashSet<int> UnusableNumbers
        {
            get { return _unusableNumbers; }
        }

        public List<int> PossibleValues
        {
            get { return Cage.UniqueNumbersInPossibilities.Except(this.UnusableNumbers).ToList(); }
        }
        
        public bool PossibilitiesFound
        {
            get { return this.Cage.Possibilities.Count > 0; }
        }

        public bool HasInvalidPossibilities 
        {
            get
            {
                return Cage.Possibilities.Any(possibility => possibility.ContainsOnly(_unusableNumbers));
            }
        }

        public List<Possibility> InvalidPossibilities
        {
            get
            {
                return Cage.Possibilities.FindAll(possibility => possibility.ContainsOnly(_unusableNumbers));
            }
        }

        public Cell(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            _unusableNumbers = new HashSet<int>();
        }
        
        internal void AddUnusableNumberRange(IEnumerable<int> numbers)
        {
            foreach (var no in numbers)
                _unusableNumbers.Add(no);
        }
        
        public Cell DeepCopy()
        {
            Cell newCell = new Cell(this.Row, this.Column);
            newCell._unusableNumbers = new HashSet<int>(this._unusableNumbers);
            newCell.Value = this.Value;
            newCell.Id = this.Id;

            return newCell;
        }        
    }
}