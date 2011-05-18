using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnKenKen.UIModels
{
    class KenListItemViewModel
    {
        public KenListItemViewModel(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public string Name { get; set; }
        public int Index { get; set; }
    }
}