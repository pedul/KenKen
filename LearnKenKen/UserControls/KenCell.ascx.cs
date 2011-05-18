using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using LearnKenKen.UIModels;

namespace LearnKenKen.UserControls
{
    public partial class KenCell : System.Web.UI.UserControl
    {
        internal CellViewModel Cell
        {
            set
            {
                SetValue(value.Value);
                spanUnusableNumbers.InnerHtml = value.UnusableNumbers;
                spanPossibilities.InnerHtml = GetPossibilities(value.Possibilities);                
                spanCellId.InnerHtml = value.CellId;
                spanOperation.InnerHtml = value.CellId.Contains('a') ? value.Operator : string.Empty;                                
                spanTargetValue.InnerHtml =  value.CellId.Contains('a') ? value.TargetValue : string.Empty;
            }
        }

        private void SetValue(string value)
        {
            spanValue.InnerHtml = value;

            if (value.Length > 0)
            {
                spanValue.Visible = true;
                spanPossibilities.Visible = false;
                spanUnusableNumbers.Visible = false;
                this.MarkSolved();                    
            }
        }     

        private string GetPossibilities(string possibilities)
        {
            int index = -1;
            int counter = 0;

            while (counter < 4 && ((index = possibilities.IndexOf("<br/>", index + 1)) >= 0))
                counter++;

            if (counter == 4)
                return possibilities.Substring(0, index - 1) + " ...";                                

            return possibilities;             
        }       

        internal void Highlight()
        {
            this.AddStyle("background-color", "#FFFF00");
        }

        internal void MarkSolved()
        {
            this.AddStyle("background-color", "#F6F9ED");
        }

        public void AddStyle(string key, string value)
        {
            cellContainer.Style.Add(key, value);
        }
    }   
}