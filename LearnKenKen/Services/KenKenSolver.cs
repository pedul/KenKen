using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LearnKenKen.DomainModels;
using LearnKenKen.Utilities;
using LearnKenKen.DomainModels.Math;

namespace LearnKenKen.Services
{
    internal class KenKenSolver
    {        
        Grid _kenKenGrid;
        Logger _log;

        internal bool Solve(Grid grid, out Logger log, bool stepByStep = false)
        {            
            _log = log = new Logger();
            _kenKenGrid = grid;

            if (_kenKenGrid.Solved)
                return true;

            Step stepAction = null;

            while ((stepAction = FindCell()).FoundCell != null)
            {
                SolveCell(stepAction);

                if (stepByStep)
                    return _kenKenGrid.Solved;
            }

            return _kenKenGrid.Solved;
        }

        Step FindCell()
        {
            Step stepAction = FindEasiestUnsolvedCell();

            if (stepAction.FoundCell != null)
                return stepAction;
            else
                stepAction = AnalyzeGridAndFindCell();

            return stepAction;
        }        

        Step FindEasiestUnsolvedCell()
        {
            Step stepAction = new Step();

            stepAction = FindSingleCellWithoutOperation();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("SingleCellCage", stepAction);
                return stepAction;
            }

            stepAction = FindCellHavingOnlyOnePossibleSolution();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("OnlyOneValidValueInPossibleValuesForCell", stepAction);                
                return stepAction;
            }

            stepAction = FindCellWhereOnlyOneNumberIsValid();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("AllButOneValueDiscarded", stepAction);
                return stepAction;
            }

            stepAction = FindCellHavingInvalidCellPossibilities();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("PossibilityInvalidForCell", stepAction);                
                return stepAction;
            }

            stepAction = FindCellHavingInvalidCagePossibilities();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("InvalidNumberInCagePossibilities", stepAction);
                return stepAction;
            }

            stepAction = FindFirstUnsolvedCellInDimension();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("FirstUnsolvedCell", stepAction);
                return stepAction;
            }

            return stepAction;
        }
        
        Step FindSingleCellWithoutOperation()
        {
            Step stepAction = new Step();

            stepAction.FoundCell =  _kenKenGrid.UnsolvedCells.FirstOrDefault(cell => cell.Cage.CellCount == 1);
            
            if (stepAction.FoundCell != null)
            {
                stepAction.CellValue = stepAction.FoundCell.Cage.TargetValue;   
            }

            return stepAction;
        }

        Step FindCellHavingOnlyOnePossibleSolution()
        {
            Step stepAction = new Step();

            stepAction.FoundCell =  _kenKenGrid.UnsolvedCells.FirstOrDefault(cell => cell.PossibleValues.Count == 1);

            if (stepAction.FoundCell != null)
            {
                stepAction.CellValue = stepAction.FoundCell.PossibleValues.First();
            }

            return stepAction;
        }

        Step FindCellWhereOnlyOneNumberIsValid()
        {
            Step stepAction = new Step();

            stepAction.FoundCell =  _kenKenGrid.UnsolvedCells.FirstOrDefault(cell => cell.UnusableNumbers.Count == _kenKenGrid.Dimension - 1);

            if (stepAction.FoundCell != null)
            {
                stepAction.CellValue = Enumerable.Range(1, _kenKenGrid.Dimension).Except(stepAction.FoundCell.UnusableNumbers).First();
            }

            return stepAction;
        }

        Step FindCellHavingInvalidCellPossibilities()
        {
            Step stepAction = new Step();

            stepAction.FoundCell = _kenKenGrid.UnsolvedCells.FirstOrDefault(cell => cell.HasInvalidPossibilities);

            if (stepAction.FoundCell != null)
            {
                stepAction.InvalidPossibilities = stepAction.FoundCell.InvalidPossibilities;
            }
                                    
            return stepAction;
        }

        Step FindCellHavingInvalidCagePossibilities()
        {
            Step stepAction = new Step();

            stepAction.FoundCell = _kenKenGrid.UnsolvedCells.FirstOrDefault(cell => cell.Cage.HasInvalidPossibilities);

            if (stepAction.FoundCell != null)
            {
                stepAction.InvalidPossibilities = stepAction.FoundCell.Cage.InvalidPossibilities;
            }

            return stepAction;
        }              

        Step FindFirstUnsolvedCellInDimension()
        {
            Step stepAction = new Step();

            int maxSolvedCount = -1;
            int rowIndex = -1;
            int columnIndex = -1;

            for (int i = 0; i < _kenKenGrid.Dimension; i++)
            {
                int rowSolvedCount = 0;
                int columnSolvedCount = 0;

                for (int j = 0; j < _kenKenGrid.Dimension; j++)
                {
                    if (_kenKenGrid[i, j].Solved || _kenKenGrid[i, j].PossibilitiesFound) rowSolvedCount++;
                    if (_kenKenGrid[j, i].Solved || _kenKenGrid[j, i].PossibilitiesFound) columnSolvedCount++;
                }

                if ((rowSolvedCount >= columnSolvedCount || columnSolvedCount == _kenKenGrid.Dimension) && rowSolvedCount > maxSolvedCount && rowSolvedCount != _kenKenGrid.Dimension)
                {
                    rowIndex = i;
                    columnIndex = -1;
                    maxSolvedCount = rowSolvedCount;
                }
                else if (columnSolvedCount > maxSolvedCount && columnSolvedCount != _kenKenGrid.Dimension)
                {
                    columnIndex = i;
                    rowIndex = -1;
                    maxSolvedCount = columnSolvedCount;
                }
            }

            if (!(rowIndex == -1 && columnIndex == -1))
            {
                if (rowIndex > columnIndex)
                {
                    stepAction.FoundCell = _kenKenGrid.Row(rowIndex).First(cell => !cell.Solved && !cell.PossibilitiesFound);
                    stepAction.Dimension = Dimension.Row;
                    stepAction.DimensionIndex = rowIndex;
                }
                else
                {
                    stepAction.FoundCell = _kenKenGrid.Column(columnIndex).First(cell => !cell.Solved && !cell.PossibilitiesFound);
                    stepAction.Dimension = Dimension.Column;
                    stepAction.DimensionIndex = columnIndex;
                }
            }

            return stepAction;
        }

        Step AnalyzeGridAndFindCell()
        {
            Step stepAction = new Step();
            
            stepAction = FindOnlyCellInDimensionThatCanContainNumber();

            if (stepAction.FoundCell != null)
            {
                _log.LogStep("OnlyCellInDimensionThatCanContainValue", stepAction);
                return stepAction;
            }
            
            stepAction = FindOnlyCageInDimensionThatCanContainNumber();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("CellsInMultidimentionalCageMustContainValue", stepAction);                
                return stepAction;
            }

            stepAction = FindMulticageWithInvalidPossibilities();
            
            if (stepAction.FoundCell != null)
            {
                _log.LogStep("InvalidPossibilitiesForMultidimensionalCage", stepAction);                
                return stepAction;
            }

            stepAction = FindMutidimensionalCageCellWithOnlyOneValidValueFromPossibilities();

            if (stepAction.FoundCell != null)
            {
                _log.LogStep("OnlyOneValueForMultidimensionalCageCell", stepAction);                
                return stepAction;
            }
            
            stepAction = FindUnusableNumbersForMultiDimensionalCageNeighbours();

            if (stepAction.FoundCell != null)
            {
                _log.LogStep("UnusableNumbersForMultidimensionalCageNeighbours", stepAction);
                return stepAction;
            }

            return stepAction;
        }

        Step FindOnlyCellInDimensionThatCanContainNumber()
        {
            Step stepAction = new Step();

            List<Cell> cells = null;

            foreach (int index in Enumerable.Range(0, _kenKenGrid.Dimension))
            {
                foreach (int value in Enumerable.Range(1, _kenKenGrid.Dimension))
                {
                    cells = _kenKenGrid.Row(index).FindAll(cell => cell.PossibleValues.Contains(value) && !cell.Solved);
                    
                    if (cells.Count == 1)
                    {
                        stepAction.FoundCell = cells[0];
                        stepAction.Dimension = Dimension.Row;
                        stepAction.CellValue = value;
                        stepAction.DimensionIndex = index;
                        
                        return stepAction;
                    }

                    cells = _kenKenGrid.Column(index).FindAll(cell => cell.PossibleValues.Contains(value) && !cell.Solved);
                    
                    if (cells.Count == 1)
                    {
                        stepAction.FoundCell = cells[0];
                        stepAction.Dimension = Dimension.Column;
                        stepAction.CellValue = value;
                        stepAction.DimensionIndex = index;

                        return stepAction;
                    }
                }
            }

            return stepAction;
        }

        Step FindOnlyCageInDimensionThatCanContainNumber()
        {
            Step stepAction = new Step();
            List<Cell> cells = null;

            foreach (int index in Enumerable.Range(0, _kenKenGrid.Dimension))
            {
                foreach (int value in Enumerable.Range(1, _kenKenGrid.Dimension))
                {
                    foreach (var val in Enum.GetValues(typeof(Dimension)))
                    {
                        Dimension dimension = (Dimension)val;

                        if (dimension == Dimension.Row)
                        {
                            cells = _kenKenGrid.Row(index).FindAll(cell => cell.PossibleValues.Contains(value) && !cell.Solved);
                        }
                        else if (dimension == Dimension.Column)
                        {
                            cells = _kenKenGrid.Column(index).FindAll(cell => cell.PossibleValues.Contains(value) && !cell.Solved);
                        }

                        if (cells.Count > 0)
                        {
                            Cell foundCell = cells[0];
                            
                            if (cells.Count > 1 && cells.TrueForAll(cell => cell.Cage == foundCell.Cage))
                            {                                
                                List<Possibility> invalidPossibilities = foundCell.Cage.Possibilities.FindAll(possibility => !possibility.Contains(value));

                                if (invalidPossibilities.Count > 0)
                                {
                                    stepAction.FoundCell = foundCell;
                                    stepAction.InvalidPossibilities = invalidPossibilities;
                                    stepAction.DimensionIndex = index;
                                    stepAction.Dimension = dimension;
                                    stepAction.CageValue = value;
                                    stepAction.FoundCageCells = cells;

                                    return stepAction;
                                }
                            }
                        }
                    }
                }
            }

            return stepAction;
        }
        
        Step FindMulticageWithInvalidPossibilities()
        {
            Step stepAction = new Step();

            List<Possibility> invalidPossibilities = null;
            
            foreach (var cage in _kenKenGrid.MultidimensionalCages)
            {
                invalidPossibilities = cage.Possibilities.FindAll(possibility =>             
                                            FindValidPermutationsForPossibility(cage, possibility).Count == 0);

                if (invalidPossibilities.Count > 0)
                {
                    stepAction.FoundCell = cage.Cells[0];
                    stepAction.InvalidPossibilities = invalidPossibilities;
                    return stepAction;
                }
            }

            return stepAction;
        }

        Step FindMutidimensionalCageCellWithOnlyOneValidValueFromPossibilities()
        {
            Step stepAction = new Step();

            foreach (var cage in _kenKenGrid.MultidimensionalCages)
            {
                List<Permutation> validPerms = FindValidPermutationsForCagePossibilities(cage);

                if (validPerms.Count > 0)
                {
                    foreach (var cell in cage.UnsolvedCells)
                    {
                        if (validPerms.SameValueAtIndex(cell.Index))
                        {
                            stepAction.FoundCell = cell;
                            stepAction.CagePossibilities = cell.Cage.Possibilities;
                            stepAction.CellValue = validPerms[0][cell.Index];
                            return stepAction;
                        }
                    }
                }
            }

            return stepAction;
        }

        Step FindUnusableNumbersForMultiDimensionalCageNeighbours()
        {
            Step stepAction = new Step();

            foreach (var cage in _kenKenGrid.MultidimensionalCages)
            {
                List<Permutation> validPerms = FindValidPermutationsForCagePossibilities(cage);

                foreach(var val in Enum.GetValues(typeof(Dimension)))
                {
                    Dimension dimension = (Dimension)val;

                    int minIndex = dimension == Dimension.Row ? cage.Cells.Min(cell => cell.Row) : cage.Cells.Min(cell => cell.Column);
                    int maxIndex = dimension == Dimension.Row ? cage.Cells.Max(cell => cell.Row) : cage.Cells.Max(cell => cell.Column);

                    for (int index = minIndex; index <= maxIndex; index++)
                    {
                        List<Cell> cageCellsInADimension = null;

                        if (dimension == Dimension.Row)
                        {
                            cageCellsInADimension = cage.Row(index);
                        }
                        else if (dimension == Dimension.Column)
                        {
                            cageCellsInADimension = cage.Column(index);
                        }

                        if (cageCellsInADimension.Count > 1)
                        {
                            List<Permutation> allSemiPermutations = new List<Permutation>();
                            
                            foreach (var perm in validPerms)
                            {
                                Permutation semiPerms = new Permutation();
                                cageCellsInADimension.ForEach(cell => semiPerms.Add(perm[cell.Index]));

                                allSemiPermutations.Add(semiPerms);
                            }

                            List<int> commonNumbers = allSemiPermutations.CommonNumbers();

                            if (commonNumbers.Count > 0)
                            {
                                List<Cell> cageNeighbours = null;

                                if (dimension == Dimension.Row)
                                {
                                    cageNeighbours = _kenKenGrid.Row(cageCellsInADimension[0].Row).FindAll(cell => !cage.Cells.Contains(cell));
                                }
                                else if (dimension == Dimension.Column)
                                {
                                    cageNeighbours = _kenKenGrid.Column(cageCellsInADimension[0].Column).FindAll(cell => !cage.Cells.Contains(cell));
                                }

                                List<Cell> affectedCells = cageNeighbours.FindAll(cell => !cell.Solved && 
                                                                                    commonNumbers.Any(no => cell.PossibleValues.Contains(no)));
                                if (affectedCells.Count > 0)
                                {
                                    stepAction.AffectedNeighboursCells = affectedCells;
                                    stepAction.Dimension = dimension;
                                    stepAction.FoundCell = affectedCells[0];
                                    stepAction.FoundCageCells = cageCellsInADimension;
                                    stepAction.DimensionIndex = index;
                                    stepAction.InvalidNeighbours = cageNeighbours;
                                    stepAction.InvalidNeighbourNumbers = commonNumbers;
                                    
                                    return stepAction;
                                }
                            }
                        }
                    }
                }                
            }

            return stepAction;
        }

        List<Permutation> FindValidPermutationsForCagePossibilities(Cage cage)
        {
            List<Permutation> validPerms = new List<Permutation>();

            cage.Possibilities.ForEach(possibility =>
                    validPerms.AddRange(FindValidPermutationsForPossibility(cage, possibility)));

            return validPerms;
        }

        List<Permutation> FindValidPermutationsForPossibility(Cage cage, Possibility possibility)
        {
            List<Permutation> validPerms = new List<Permutation>();

            List<Permutation> perms = Helper.GetUniquePermutation(possibility.ToList<int>());

            foreach (var permutation in perms)
            {
                Cage copyCage = cage.DeepCopy();

                bool isPermutationValid = true;
                for (int i = 0; i < copyCage.Cells.Count; i++)
                {
                    if (!copyCage.Cells[i].IsValueFeasible(permutation[i]))
                    {
                        isPermutationValid = false;
                        break;
                    }
                    else
                    {
                        MarkCellAsSolved(copyCage.Cells[i], permutation[i], copyCage.Cells);
                    }
                }

                if (isPermutationValid)
                    validPerms.Add(permutation);
            }

            return validPerms;
        }

        void SolveCell(Step stepAction)
        {
            if (stepAction.FoundCell == null || stepAction.FoundCell.Solved)
                return;

            _log.FoundCell = stepAction.FoundCell;
            
            IEnumerable<Possibility> possibilities = stepAction.FoundCell.Cage.FindPossibilites(_kenKenGrid.Dimension);

            if (stepAction.FoundCell.Cage.Possibilities != null && stepAction.FoundCell.Cage.Possibilities.Count == 0 &&
                                                                            stepAction.FoundCell.Cage.Operation != Operation.None)
            {
                Step localStepAction = new Step();

                localStepAction.FoundCell = stepAction.FoundCell;
                localStepAction.FoundPossibilities = possibilities.ToList();
                
                _log.LogStep("PossibleSolutions", localStepAction);
                
                if (stepAction.CellValue > 0)
                    possibilities = EliminatePossibilitiesWithoutCellValue(possibilities, stepAction.CellValue);

                possibilities = EliminateAlreadyUsedNumbersFromPossibilities(possibilities, stepAction.FoundCell.Cage);
            }

            MarkCageAsPartiallSolved(stepAction.FoundCell.Cage, possibilities);

            if (stepAction.CellValue > 0)
                MarkCellAsSolved(stepAction.FoundCell, stepAction.CellValue);

            if (stepAction.InvalidPossibilities != null)
                DeleteCagePossibilities(stepAction.FoundCell.Cage, stepAction.InvalidPossibilities);

            if (stepAction.InvalidNeighbours != null)
                AddUnusableNumbersToCells(stepAction.InvalidNeighbours, stepAction.InvalidNeighbourNumbers);
        }

        private IEnumerable<Possibility> EliminatePossibilitiesWithoutCellValue(IEnumerable<Possibility> possibilities, int value)
        {
            List<Possibility> invalidPoss = possibilities.Where(possibility =>
                                                                    !possibility.Contains(value)).ToList();            

            if (invalidPoss.Count > 0)
            {
                Step localStepAction = new Step();

                localStepAction.CellValue = value;
                localStepAction.InvalidPossibilities = invalidPoss;

                _log.LogStep("DiscardPossibilityWithoutCellValue", localStepAction);                
            }

            return possibilities.Where(possibility => possibility.Contains(value));            
        }

        List<Possibility> EliminateAlreadyUsedNumbersFromPossibilities(IEnumerable<Possibility> possibilities, Cage cage)
        {
            List<int> unusableNumbers = cage.CommonUnusableNumbersInCells;

            List<Possibility> filteredPossibilities = possibilities.Where(possibility => !possibility.ContainsAny(unusableNumbers)).ToList();            
            List<Possibility> invalidPossibilities = possibilities.Where(possibility => possibility.ContainsAny(unusableNumbers)).ToList();

            if (invalidPossibilities.Count > 0)
            {
                Step localStepAction = new Step();

                localStepAction.InvalidPossibilities = invalidPossibilities;

                _log.LogStep("DiscardPossibilities", localStepAction);                
            }

            filteredPossibilities = filteredPossibilities.FindAll(possibility =>
                {                    
                    Cell found = cage.Cells.FirstOrDefault(cell => possibility.ContainsOnly(cell.UnusableNumbers));
                    
                    if (found != null)
                    {
                        Step localStepAction = new Step();

                        localStepAction.FoundCell = found;
                        localStepAction.InvalidPossibilities = new List<Possibility>() { possibility };

                        _log.LogStep("DiscardPossibilityForCell", localStepAction);                                        
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                });

            return filteredPossibilities;
        }

        void MarkCageAsPartiallSolved(Cage cage, IEnumerable<Possibility> possibilities)
        {
            if (possibilities.Count() < 1)
                throw new ArgumentException("List of possibilities cannot be empty");

            cage.AddPossibilities(possibilities);

            UpdateUnusableNumbersForCageNeighbours(cage);
        }

        void UpdateUnusableNumbersForCageNeighbours(Cage cage)
        {
            if (cage.SpansSingleRow || cage.SpansSingleColumn)
            {
                if (cage.SpansSingleRow)
                    AddUnusableNumbersToCells(_kenKenGrid.Row(cage.Cells.First().Row).Where(cell => !cage.Cells.Contains(cell)),
                                                cage.CommonValuesInCellPossibilities);
                if (cage.SpansSingleColumn)
                    AddUnusableNumbersToCells(_kenKenGrid.Column(cage.Cells.First().Column).Where(cell => !cage.Cells.Contains(cell)),
                                                cage.CommonValuesInCellPossibilities);
            }
            else
            {
                // => Cage spans multiple rows or columns
                // => Deleting invalid numbers from multicage cell neighbours is handled as a different 
                //    step because it is too complex as a side-effect of another step
            }
        }

        void MarkCellAsSolved(Cell cell, int value, List<Cell> localCells = null)
        {
            cell.Value = value;

            List<Possibility> invalidPoss = cell.Cage.FindPossibilitiesNotContainingValue(value);            

            if (localCells == null)
            {
                if (invalidPoss.Count > 0)
                {
                    Step localStepAction = new Step();
                    
                    localStepAction.FoundCell = cell;
                    localStepAction.CellValue = value;
                    localStepAction.InvalidPossibilities = invalidPoss;
                    
                    _log.LogStep("DeletingInvalidPossibilityAfterCellSolved", localStepAction);                    
                }

                DeleteCagePossibilities(cell.Cage, invalidPoss);
                
                AddUnusableNumbersToCells(_kenKenGrid.Neighbours(cell), new List<int> { value });
            }
            else
            {
                cell.Cage.DeletePossibilities(invalidPoss);

                foreach (var affectedCell in localCells)
                {
                    if (affectedCell != cell && (affectedCell.Row == cell.Row || affectedCell.Column == cell.Column))
                        affectedCell.UnusableNumbers.Add(cell.Value);
                }
            }
        }

        void DeleteCagePossibilities(Cage cage, List<Possibility> invalidPossibilities)
        {
            cage.DeletePossibilities(invalidPossibilities);

            UpdateUnusableNumbersForCageNeighbours(cage);
        }

        void AddUnusableNumbersToCells(IEnumerable<Cell> cells, IEnumerable<int> numbers)
        {
            List<Cell> affectedCells = new List<Cell>();

            foreach (var cell in cells)
            {
                int originalCount = cell.UnusableNumbers.Count;
                cell.AddUnusableNumberRange(numbers);

                if (cell.UnusableNumbers.Count > originalCount && !cell.Solved)
                    affectedCells.Add(cell);
            }

            if (affectedCells.Count > 0)
            {
                Step localStepAction = new Step();
                
                localStepAction.InvalidNeighbourNumbers = numbers.ToList();
                localStepAction.AffectedNeighboursCells = affectedCells;

                _log.LogStep("InvalidNumbersForNeighbours", localStepAction);                
            }
        }
    }
}