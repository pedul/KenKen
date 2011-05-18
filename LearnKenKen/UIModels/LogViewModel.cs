using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LearnKenKen.Utilities;

namespace LearnKenKen.UIModels
{
    class LogViewModel
    {
        public LogViewModel(Logger log)
        {
            this.CellId = log.CellId;
            this.Why = log.Why;
            this.Details = log.Details;
        }

        public string CellId { get; set; }
        public string Why { get; set; }
        public List<string> Details { get; set; }
    }
}