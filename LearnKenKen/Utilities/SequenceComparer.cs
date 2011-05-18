using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnKenKen.Utilities
{
    internal class SequenceComparer<T> : IEqualityComparer<T>
        where T : IEnumerable<int>
    {
        public bool Equals(T x, T y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}