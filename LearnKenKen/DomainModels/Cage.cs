using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LearnKenKen.Utilities;
using LearnKenKen.DomainModels.Math;

namespace LearnKenKen.DomainModels
{
    public enum Operation
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide
    }

    class Cage
    {
        List<Cell> _cells;
        List<Possibility> _possibilities;

        public Operation Operation { get; private set; }
        
        public int TargetValue { get; private set; }

        public int Id { get; private set; }

        public List<Cell> Cells
        {
            get { return _cells; }
        }

        public List<Cell> UnsolvedCells
        {
            get { return _cells.FindAll(cell => !cell.Solved); }
        }
        
        public List<Possibility> Possibilities
        {
            get { return _possibilities; }
        }

        public List<int> CommonValuesInCellPossibilities
        {
            get
            {
                List<int> commonNuumbers = new List<int>(_possibilities.First());
                foreach (var possibility in _possibilities)
                    commonNuumbers = commonNuumbers.Intersect(possibility).ToList();

                return commonNuumbers;
            }
        }

        public int CellCount
        {
            get { return _cells.Count; }
        }

        public int RepeatCount
        {
            get
            {
                int repeatCount = 1;

                if (!(this.SpansSingleColumn || this.SpansSingleRow))
                {
                    int uniqueRowIndexes = this.Cells.Select<Cell, int>(cell => cell.Row).Distinct().Count();
                    int uniqueColIndexes = this.Cells.Select<Cell, int>(cell => cell.Column).Distinct().Count();

                    repeatCount = uniqueRowIndexes <= uniqueColIndexes ? uniqueRowIndexes : uniqueColIndexes;
                }

                return repeatCount;
            }
        }

        public bool SpansSingleRow
        {
            get { return _cells.TrueForAll(x => x.Row == _cells[0].Row);  }
        }

        public bool SpansSingleColumn
        {
            get { return _cells.TrueForAll(x => x.Column == _cells[0].Column);  }
        }

        public List<int> CommonUnusableNumbersInCells
        {
            get
            {
                IEnumerable<int> _eliminatedNumbers = new List<int>(_cells[0].UnusableNumbers);

                _cells.ForEach(cell =>
                    _eliminatedNumbers = _eliminatedNumbers.Intersect(cell.UnusableNumbers));

                return _eliminatedNumbers.ToList<int>();
            }
        }

        public List<int> UniqueNumbersInPossibilities
        {
            get
            {
                var numbersInPoss = new List<int>();
                foreach (var poss in _possibilities)
                    numbersInPoss.AddRange(poss);

                return numbersInPoss.Distinct().ToList<int>();
            }
        }

        public bool HasInvalidPossibilities
        {
            get
            {
                return _possibilities.Any(possibility => possibility.ContainsAny(CommonUnusableNumbersInCells));
            }
        }

        public List<Possibility> InvalidPossibilities
        {
            get
            {
                return _possibilities.FindAll(possibility => possibility.ContainsAny(CommonUnusableNumbersInCells));
            }
        }

        public Cage(int id, Operation operation, int targetValue, IEnumerable<Cell> cells)
        {
            Operation = operation;
            TargetValue = targetValue;
            Id = id;

            _possibilities = new List<Possibility>();

            _cells = new List<Cell>();
            _cells.AddRange(cells);

            char c = 'a';
            int counter = 0;
            _cells.ForEach(cell =>
            {
                cell.Index = counter ++;
                cell.Id = this.Id + " " + c++;
                cell.Cage = this;
            });            
        }

        private Cage(int id, Operation operation, int targetValue, List<Cell> cells, List<Possibility> possibilities)
        {
            Id = id;
            Operation = operation;
            TargetValue = targetValue;            

            _cells = cells;
            cells.ForEach(cell => cell.Cage = this);

            _possibilities = possibilities;
        }

        internal Cage DeepCopy()
        {
            List<Cell> newCells = new List<Cell>();
            foreach (var cell in this.Cells)
            {
                Cell newCell = cell.DeepCopy();
                newCells.Add(newCell);
            }

            List<Possibility> newPossibilities = new List<Possibility>();
            this.Possibilities.ForEach(possibility =>
            {
                Possibility newPossibility = possibility.DeepCopy();
                newPossibilities.Add(newPossibility);
            });

            return new Cage(this.Id, this.Operation, this.TargetValue, newCells, newPossibilities);
        }    

        internal void DeletePossibilities(IEnumerable<Possibility> invalidPossibilities)
        {
            foreach (var poss in invalidPossibilities)
                _possibilities.Remove(poss);
        }

        internal List<Possibility> FindPossibilitiesNotContainingValue(int value)
        {
            return _possibilities.FindAll(possibility =>
                                            !possibility.Contains(value));
        }

        internal void AddPossibilities(IEnumerable<Possibility> possibilities)
        {
            _possibilities.Clear();
            _possibilities.AddRange(possibilities);
        }                   

        internal List<Possibility> FindPossibilites(int dimension)
        {
            List<Possibility> possibilities = null;

            if (_possibilities != null && _possibilities.Count > 0)
            {
                possibilities = new List<Possibility>(_possibilities);
            }
            else
            {
                switch (Operation)
                {
                    case Operation.Multiply:
                        possibilities = FindPossibilitiesForMultiplyCage(dimension);
                        break;
                    case Operation.Divide:
                        possibilities = FindPossibilitiesForDivideCage(dimension);
                        break;
                    case Operation.Add:
                        possibilities = FindPossibilitiesForAddCage(dimension);
                        break;
                    case Operation.Subtract:
                        possibilities = FindPossibilitiesForSubtractCage(dimension);
                        break;
                    case Operation.None:
                        possibilities = FindPossibilitiesForSingleCellCage(dimension);
                        break;
                }
            }

            return possibilities;
        }

        List<Possibility> FindPossibilitiesForSingleCellCage(int dimension)
        {
            return new List<Possibility>() { new Possibility(new List<int>() { TargetValue } ) };
        }

        List<Possibility> FindPossibilitiesForSubtractCage(int dimension)
        {
            List<Possibility> possibilities = new List<Possibility>();

            for (int i = 1; i <= dimension; i++)
                for (int j = 1; j <= dimension; j++)
                    if (i - j == TargetValue)
                        possibilities.Add(new Possibility(new List<int>() { i, j }));

            return possibilities;
        }

        List<Possibility> FindPossibilitiesForDivideCage(int dimension)
        {
            List<Possibility> possibilities = new List<Possibility>();

            for (int i = 1; i <= dimension; i++)
                for (int j = 1; j <= dimension; j++)
                    if (i / j == TargetValue && i % j == 0)
                        possibilities.Add(new Possibility(new List<int>() { i, j }));

            return possibilities;
        }

        List<Possibility> FindPossibilitiesForAddCage(int dimension)
        {
            List<Possibility> possibilities = new List<Possibility>();

            var inputSet = new List<int>();
            for (int i = 0; i < RepeatCount; i++)
                inputSet.AddRange(Enumerable.Range(1, dimension));

            foreach (var set in Helper.Powerset(inputSet, CellCount, TargetValue))
            {
                Possibility newPossibility = new Possibility(set);
                
                if (!possibilities.Contains(newPossibility, new SequenceComparer<Possibility>()))
                    possibilities.Add(newPossibility);
            }

            return possibilities;
        }

        List<Possibility> FindPossibilitiesForMultiplyCage(int dimension)
        {
            List<Possibility> possibilities = new List<Possibility>();

            List<int> primeFactors = Helper.PrimeFactors(TargetValue);
            primeFactors.Add(1);
            
            MultiSet multisetOfPrimes = new MultiSet(primeFactors);
            
            IEnumerable<Partition> partitions = multisetOfPrimes.Partition().Filter(CellCount, dimension);
            
            foreach (var partion in partitions)
            {
                Possibility newPossibility = new Possibility(partion);

                if (newPossibility.MaxRepeatCount <= this.RepeatCount  
                            && !possibilities.Contains(newPossibility, new SequenceComparer<Possibility>()))
                    possibilities.Add(newPossibility);                    
            }

            return possibilities;
        }

        public List<Cell> Row(int index)
        {
            return _cells.FindAll(cell => cell.Row == index);
        }

        public List<Cell> Column(int index)
        {
            return _cells.FindAll(cell => cell.Column == index);
        }
    }
}