using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace LearnKenKen.DomainModels.Math
{
    internal class Permutation : IEnumerable<int>
    {
        List<int> _permutation;

        public int Count
        {
            get { return _permutation == null ? 0 : _permutation.Count; }
        }

        public int this[int index]
        {
            get { return _permutation[index]; }
            set { _permutation[index] = value; }
        }

        public Permutation() 
        {
            _permutation = new List<int>();
        }

        public Permutation(List<int> permutation)
        {
            _permutation = permutation;
        }

        public Permutation(Permutation permutation)
        {
            _permutation = new List<int>(permutation._permutation);
        }

        public override bool Equals(object obj)
        {           
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Permutation other = obj as Permutation;

            return _permutation.SequenceEqual(other._permutation);                        
        }

        public override int GetHashCode()
        {
            return _permutation.GetHashCode();            
        }        

        internal void Insert(int index, int item)
        {
            _permutation.Insert(index, item);
        }

        internal void Add(int item)
        {
            _permutation.Add(item);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _permutation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _permutation.GetEnumerator();
        }
    }

    static internal class PermutationHelper
    {
        internal static List<int> CommonNumbers(this List<Permutation> permutations)
        {
            List<int> commonNumbers = new List<int>(permutations[0]);
            foreach (var permutation in permutations)
                commonNumbers = commonNumbers.Intersect(permutation).ToList();

            return commonNumbers;
        }

        internal static bool SameValueAtIndex(this List<Permutation> permutations, int index)
        {
            int value = permutations[0][index];

            foreach (var array in permutations)
            {
                if (array[index] != value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}