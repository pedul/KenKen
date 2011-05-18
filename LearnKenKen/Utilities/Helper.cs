using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Xml.Serialization;

using LearnKenKen.DomainModels.Math;
using LearnKenKen.DomainModels;

namespace LearnKenKen.Utilities
{
    static class Helper
    {        
        internal static IEnumerable<IList<int>> Powerset(IEnumerable<int> numbers)
        {
            int[] currentGroupList = numbers.ToArray();
            int count = currentGroupList.Length;
            
            Dictionary<long, int> powerToIndex = new Dictionary<long, int>();
            
            long mask = 1L;            
            for (int i = 0; i < count; i++)
            {
                powerToIndex[mask] = currentGroupList[i];
                mask <<= 1;
            }

            Dictionary<long, int> result = new Dictionary<long, int>();
            yield return result.Values.ToArray();

            long max = 1L << count;
            for (long i = 1L; i < max; i++)
            {
                long key = i & -i;
                
                if (result.ContainsKey(key))
                    result.Remove(key);
                else
                    result[key] = powerToIndex[key];
            
                yield return result.Values.ToArray();
            }
        }

        internal static IEnumerable<IList<int>> Powerset(IEnumerable<int> numbers, int length, int sum)
        {
            foreach (var set in Powerset(numbers))
            {
                if (set.Count() == length && set.Sum() == sum)
                    yield return set;
            }
        }

        internal static List<int> PrimeFactors(int number)
        {
            List<int> primeFactors = new List<int>();

            int divisor = 2;

            while (divisor * divisor <= number)
            {
                if (number % divisor == 0)
                {
                    number /= divisor;
                    primeFactors.Add(divisor);
                }
                else
                {
                    divisor++;
                }
            }

            primeFactors.Add(number);

            return primeFactors;
        }

        internal static List<Permutation> GetPermutation(List<int> list)
        {
            List<Permutation> result = new List<Permutation>();

            if (list.Count == 1)
            {
                result.Add(new Permutation(list));
                return result;
            }

            foreach (var recursivePermute in GetPermutation(list.Skip(1).ToList()))
            {
                for (int i = 0; i < recursivePermute.Count; i++)
                {
                    Permutation newPermute = new Permutation(recursivePermute);                    
                    newPermute.Insert(i, list[0]);
                    result.Add(newPermute);
                }

                Permutation finalPermute = new Permutation(recursivePermute);
                finalPermute.Add(list[0]);
                result.Add(finalPermute);
            }

            return result;
        }

        internal static List<Permutation> GetUniquePermutation(List<int> list)
        {
            List<Permutation> allPermutes = GetPermutation(list);
            List<Permutation> uniquePermutes = new List<Permutation>();
            
            allPermutes.ForEach(permute =>
            {
                if (uniquePermutes.TrueForAll(existingPermute => !existingPermute.Equals(permute)))
                    uniquePermutes.Add(permute);
            });

            return uniquePermutes;
        }

        internal static string ToCommaSeparatedString<T>(IEnumerable<T> list)
        {
            if (list == null)
                return null;

            return string.Join(", ", list.ToArray());
        }

        internal static string GetNewlineSeparatedString<T>(IEnumerable<T> list)
        {
            return string.Join("<br/>", list.ToArray());
        }        

        internal static string GetSetDelimitedString<T>(IEnumerable<T> list)
        {
            return " { " + string.Join(", ", list.ToArray()) + " } ";
        }

        internal static string GetSetDelimitedString<T>(IEnumerable<T> list, IEnumerable<T> strikeOut)
        {
            string setDelimited = GetSetDelimitedString(list);    
            
            foreach(var item in strikeOut)
            {
                string spanElement = "<span style='color:#EE0000'>" + item.ToString() + "</span>";
                setDelimited = setDelimited.Replace(item.ToString(), spanElement);
            }

            return setDelimited;
        }       

        internal static string GetSetDelimitedLineSepaPossibilities(IEnumerable<Possibility> possibilities, IEnumerable<int> unusableNumbers)
        {
            return possibilities == null ? string.Empty :
                    Helper.GetNewlineSeparatedString(possibilities.Select<Possibility, string>
                        (x => Helper.GetSetDelimitedString(x, unusableNumbers)));
        }

        internal static string ToString(List<Possibility> possibilities)
        {
            if (possibilities == null)
                return null;

            return Helper.ToCommaSeparatedString(possibilities.Select<Possibility, string>(possibility =>
                                              Helper.GetSetDelimitedString(possibility)));
        }

        internal static string ToString(List<Cell> cells)
        {
            if (cells == null)
                return null;

            return Helper.ToCommaSeparatedString(cells.Select<Cell, string>(cell => cell.Id));
        }

        internal static string ToString(int? value)
        {
            if (value == null)
                return null;

            return value.ToString();
        }

        internal static Operation ParseOperation(string operation)
        {
            return (Operation)Enum.Parse(typeof(Operation), operation);
        }

        internal static string GetString(this Operation operation)
        {
            switch (operation)
            {
                case Operation.None:
                    return "";
                case Operation.Add:
                    return "+";
                case Operation.Multiply:
                    return "x";
                case Operation.Divide:
                    return "÷";
                case Operation.Subtract:
                    return "-";
                default:
                    return "";
            }
        }

        internal static string ReplaceReasonPlaceholders(Step stepAction, string data)
        {
            if (data == null)
                return null;

            string cageId = stepAction.FoundCell == null ? null : stepAction.FoundCell.Cage.Id.ToString();
            string cellId = stepAction.FoundCell == null ? null : stepAction.FoundCell.Id;
            string cellValue = Helper.ToString(stepAction.CellValue);
            string cageValue = Helper.ToString(stepAction.CageValue);
            string cagePossibilities = Helper.ToString(stepAction.CagePossibilities);
            string foundPossibilities = Helper.ToString(stepAction.FoundPossibilities);
            string invalidPossibilities = Helper.ToString(stepAction.InvalidPossibilities);
            string unusableNumbers = Helper.ToCommaSeparatedString(stepAction.InvalidNeighbourNumbers);
            string foundCageCells = Helper.ToString(stepAction.FoundCageCells);
            string affectedNeighbourCells = Helper.ToString(stepAction.AffectedNeighboursCells);
            string dimension = stepAction.Dimension == Dimension.Row ? "row" : "column";
            string dimensionNumber = Helper.ToString(stepAction.DimensionIndex + 1);

            return data
                    .Replace("@CellId", "<span class='LogData'>" + cellId + "</span>")
                    .Replace("@CageId", "<span class='LogData'>" + cageId + "</span>")
                    .Replace("@CellValue", "<span class='LogData'>" + cellValue + "</span>")
                    .Replace("@CageValue", "<span class='LogData'>" + cageValue + "</span>")
                    .Replace("@CagePossibilities", "<span class='LogData'>" + cagePossibilities + "</span>")
                    .Replace("@FoundPossibilities", "<span class='LogData'>" + foundPossibilities + "</span>")
                    .Replace("@UnusableNumbers", "<span class='LogData'>" + unusableNumbers + "</span>")
                    .Replace("@FoundCageCells", "<span class='LogData'>" + foundCageCells + "</span>")
                    .Replace("@AffectedNeighbourCells", "<span class='LogData'>" + affectedNeighbourCells + "</span>")
                    .Replace("@InvalidPossibilities", "<span class='LogData'>" + invalidPossibilities + "</span>")
                    .Replace("@DimensionNumber", "<span class='LogData'>" + dimensionNumber + "</span>")
                    .Replace("@Dimension", "<span class='LogData'>" + dimension + "</span>");
        }

        internal static Control FindControlRecursive(this Control control, string id)
        {
            Control returnControl = control.FindControl(id);

            if (returnControl == null)
            {
                foreach (Control child in control.Controls)
                {
                    returnControl = FindControlRecursive(child, id);

                    if (returnControl != null && returnControl.ID == id)
                        return returnControl;
                }
            }

            return returnControl;
        }

        internal static T FindControlRecursive<T>(this Control control)
            where T : Control
        {
            T returnControl = null;

            foreach (Control child in control.Controls)
            {
                if (child is T)
                    returnControl = child as T;
                else
                    child.FindControlRecursive<T>();
            }

            return returnControl;
        }

        internal static T GetXmlConfigData<T>(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
            StreamReader sr = new StreamReader(path);

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(sr);
            }
            finally
            {
                sr.Close();
            }
        }
    }
}