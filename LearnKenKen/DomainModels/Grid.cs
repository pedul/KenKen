using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnKenKen.DomainModels
{
    class Grid
    {
        private int _dimension;
        private List<Cell> _cells;

        public int Dimension
        {
            get { return _dimension; }
        }

        public List<Cage> Cages { get; private set; }

        public List<Cage> MultidimensionalCages 
        {
            get { return Cages.FindAll(cage => !cage.SpansSingleColumn && !cage.SpansSingleRow); } 
        }

        public Cell[,] CellMatrix { get; private set; }                

        public List<Cell> Cells { get { return _cells; } }

        public List<Cell> UnsolvedCells { get { return _cells.FindAll(cell => !cell.Solved); } }

        public Cell this[int row, int column]
        {
            get
            {
                return CellMatrix[row, column];
            }
        }

        public bool Solved
        {
            get { return Cages.TrueForAll(cage => cage.Cells.TrueForAll(cell => cell.Solved)); }
        }

        public Grid(IEnumerable<Cage> cages)
        {
            Cages = new List<Cage>(cages);
            
            _dimension = Cages.Max(cage =>
                            cage.Cells.Max(cell =>
                                cell.Row)) + 1;

            CellMatrix = new Cell[_dimension, _dimension];
            _cells = new List<Cell>();

            Cages.ForEach(cage =>
            {
                cage.Cells.ForEach(cell =>
                    {
                        CellMatrix[cell.Row, cell.Column] = cell;
                        _cells.Add(cell);
                    });
            });
        }

        internal List<Cell> Row(int row)
        {
            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < _dimension; i++)
                cells.Add(CellMatrix[row, i]);
            
            return cells;
        }

        internal List<Cell> Column(int column)
        {
            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < _dimension; i++)
                cells.Add(CellMatrix[i, column]);

            return cells;
        }

        internal List<Cell> Neighbours(Cell cell)
        {
            return _cells.FindAll(xCell => (xCell.Row == cell.Row || xCell.Column == cell.Column) && xCell != cell);
        }                                     
    }
}