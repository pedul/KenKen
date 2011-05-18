using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LearnKenKen.DomainModels.Math
{
    class Partition : IEnumerable<MultiSet>
    {
        List<MultiSet> _items = new List<MultiSet>();

        public int SubsetCount
        {
            get { return _items.Count; }
        }

        internal void AddSubset(MultiSet subset)
        {
            _items.Add(subset);
            _items.RemoveAll(item => item.Length == 0);
        }

        public Partition(MultiSet multiSet)
        {
            _items.Add(multiSet);
        }

        public IEnumerator<MultiSet> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    internal static class PartitionHelper
    {
        static public IEnumerable<Partition> Filter(this IEnumerable<Partition> partitions, int length, int maxNumber)
        {
            return partitions.Where(partition =>
                                        partition.All(subset => subset.Product <= maxNumber)
                                            && partition.Count() == length
                                    );
        }
    }
}