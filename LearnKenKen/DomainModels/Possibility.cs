using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using LearnKenKen.DomainModels.Math;

namespace LearnKenKen.DomainModels
{
    class Possibility : IEnumerable<int>
    {      
        List<int> _items;
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int MaxRepeatCount 
        {
            get
            {
                return _items.Max(no => _items.Count(element => element == no));
            }
        }

        Possibility() { }

        public Possibility(IEnumerable<int> values)
        {
            _items = new List<int>(values);
            _items.Sort();
        }
        
        public Possibility(Partition partion)
        {
            _items = new List<int>();

            foreach (var subset in partion)
                _items.Add(subset.Product);

            _items.Sort();
        }

        internal bool ContainsOnly(IEnumerable<int> numbers)
        {
            return _items.TrueForAll(no => numbers.Contains(no));
        }

        internal bool ContainsAny(IEnumerable<int> numbers)
        {
            return _items.Any(no => numbers.Contains(no));
        }

        internal Possibility DeepCopy()
        {
            Possibility newPossibility = new Possibility();
            newPossibility._items = new List<int>(this._items);

            return newPossibility;
        }
    }    
}