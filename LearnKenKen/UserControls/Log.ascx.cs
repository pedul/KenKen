using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LearnKenKen.UIModels;

namespace LearnKenKen.UserControls
{
    public partial class Log : System.Web.UI.UserControl
    {        
        internal LogViewModel LogData
        {
            set
            {
                spanCellId.InnerHtml = value.CellId;
                spanWhy.InnerHtml = value.Why;

                lstDetails.DataSource =  value.Details.Select(item => new { Detail = item });
                lstDetails.DataBind();
            }
        }
    }
}