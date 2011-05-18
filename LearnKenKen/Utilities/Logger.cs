using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LearnKenKen.DomainModels;
using LearnKenKen.DataModel;

namespace LearnKenKen.Utilities
{
    enum LogCase
    {
        Default,
        Singular,
        Plural
    }    

    class Logger
    {
        Dictionary<string, ReasonXml> _reasons = null;
                
        public string Why { get; private set; }
        public List<string> Details { get; private set; }

        public Cell FoundCell 
        { 
            set
            {
                CellId = value.Id;
                CageId = value.Cage.Id;
                CellRow = value.Row;
                CellColumn = value.Column;
            }
        }

        public string CellId { get; set; }
        public int CageId { get; set; }
        public int CellRow { get; set; }
        public int CellColumn { get; set; }        

        public Logger()
        {
            Details = new List<string>();

            _reasons = Helper.GetXmlConfigData<List<ReasonXml>>("Reason.xml")
                                                        .ToDictionary(reason => reason.Id);
        }

        internal void LogStep(string reason, Step stepAction)
        {
            ReasonXml rs = _reasons[reason];

            LogCase logCase = LogCase.Default;

            if (stepAction.InvalidPossibilities != null)
                logCase = stepAction.InvalidPossibilities.Count > 1 ? LogCase.Plural : LogCase.Singular;            
            else if (stepAction.FoundPossibilities != null)
                logCase = stepAction.FoundPossibilities.Count > 1 ? LogCase.Plural : LogCase.Singular;
            else if (stepAction.InvalidNeighbourNumbers != null)
                logCase = stepAction.InvalidNeighbourNumbers.Count > 1 ? LogCase.Plural : LogCase.Singular;
            else if (stepAction.CagePossibilities != null)
                logCase = stepAction.CagePossibilities.Count > 1 ? LogCase.Plural : LogCase.Singular;

            string why = GetWhy(stepAction, rs, logCase);
            string detail = GetDetail(stepAction, rs, logCase);

            Update(why, detail);
        }

        string GetWhy(Step stepAction, ReasonXml reason, LogCase logCase)
        {                                
            string why = reason.Why;

            if (logCase == LogCase.Singular && reason.WhySingular != null)
                why = reason.WhySingular;
            else if (logCase == LogCase.Plural && reason.WhyPlural != null)
                why = reason.WhyPlural;

            return Helper.ReplaceReasonPlaceholders(stepAction, why);
        }

        string GetDetail(Step stepAction, ReasonXml reason, LogCase logCase)
        {
            string detail = reason.Detail;

            if (logCase == LogCase.Singular && reason.DetailSingular != null)
                detail = reason.DetailSingular;
            else if (logCase == LogCase.Plural && reason.DetailPlural != null)
                detail = reason.DetailPlural;

            return Helper.ReplaceReasonPlaceholders(stepAction, detail);
        }

        void Update(string why, string detail)
        {
            if (why != null)
                Why = why;

            if (detail != null)
                Details.Add(detail);
        }        
    }
}