using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using LearnKenKen.Utilities;

namespace LearnKenKen.DomainModels.Math
{
    class MultiSet : IEnumerable<int>
    {
        List<int> _items = null;

        public int Length
        {
            get { return _items.Count; }
        }

        public int this[int index]
        {
            get { return _items[index]; }
        }

        public static MultiSet operator -(MultiSet first, int number)
        {
            List<int> items = new List<int>(first._items);
            items.Remove(number);

            return new MultiSet(items);
        }

        public static MultiSet operator -(MultiSet first, IEnumerable<int> elements)
        {
            List<int> items = new List<int>(first._items);
            foreach (var no in elements)
                items.Remove(no);

            return new MultiSet(items);
        }

        public static MultiSet operator +(MultiSet first, int second)
        {
            List<int> items = new List<int>(first._items);
            items.Add(second);

            return new MultiSet(items);
        }

        public int Product
        {
            get
            {
                return this.Aggregate((product, x) => product *= x);
            }
        }

        public MultiSet()
        {
            _items = new List<int>();
        }

        public MultiSet(IEnumerable<int> items)
            : this()
        {
            _items.AddRange(items);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Find all possible partitions of the multiset
        /// 
        /// function: Multiset Partition S
        ///             pick any element s from S (in our algo, we always pick first element)
        ///             X = s Union PS  [for each PS in powerset of {S/s}]
        ///             Y = X | P       [for each P in Multiset Partition of {S/X}]
        ///
        /// Rules : Multiset Partition [x] = {x} where x is belongs to N
        /// Rules : Multiset Partition [empty] = {empty} set
        /// 
        /// e.g. Consider {3, 5, 7} multiset        
        ///     initially s = 3
        ///     powerset of S/s = { {empty}, {5}, {7}, {5,7} }
        ///     s U PS gives us {3} {3,5}  {3,7} and {3,5,7}
        ///         take {3, 5, 7}
        ///             multisetpartition of S/{s U PS} is multisetpartition of empty = {empty}
        ///             hence we have {3,5,7}
        ///         take {3, 7}
        ///             multisetpartition of S/{s U PS} is multisetpartition of 5 = {5} (one element only)
        ///             hence we have {3, 7} | {5}
        ///         take {3, 5}
        ///             multisetpartition of S/{s U PS} is multisetpartition of 7 = {7} (one element only)
        ///             hence we have {3, 5} | {7}
        ///         take {3}
        ///             multisetpartition of S/{s U PS} is multisetpartition of {5, 7} = {{5} | {7}, {5, 7} }
        ///             hence we have {3} | {5} | {7}
        ///             hence we have {3} | {5, 7}
        /// 
        /// </summary>
        /// <returns>
        ///     List of partitions for the multiset
        /// </returns>
        internal IEnumerable<Partition> Partition()
        {
            if (Length == 0)
                return new List<Partition>() { new Partition(new MultiSet()) };

            if (Length == 1)
                return new List<Partition>() { new Partition(this) };

            int s = _items[0];

            // Powerset of S/s
            IEnumerable<IList<int>> powerset = Helper.Powerset(this - s);

            List<Partition> partitions = new List<Partition>();
            foreach (var ps in powerset)
            {
                // Find Partitions of S/{s U ps}
                MultiSet recursiveSet = this - ps - s;
                IEnumerable<Partition> recursivePartitions = recursiveSet.Partition();

                // Add subset {s U ps} to each partition of S/{s U ps}
                foreach (var partition in recursivePartitions)
                {
                    MultiSet ms = new MultiSet(ps) + s;
                    partition.AddSubset(ms);
                }

                // Add the new partitions to set of partitions
                foreach (var partition in recursivePartitions)
                    partitions.Add(partition);
            }

            return partitions;
        }        
    }
}