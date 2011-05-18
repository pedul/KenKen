using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using LearnKenKen.DomainModels;
using LearnKenKen.UIModels;
using LearnKenKen.Utilities;

namespace LearnKenKen.UserControls
{
    public partial class KenGrid : System.Web.UI.UserControl
    {
        internal Grid Grid { set { DrawKenGrid(value); } }

        private void DrawKenGrid(Grid kenGrid)
        {
            container.Controls.Clear();

            Table tbl = new Table();
            container.Controls.Add(tbl);

            for (int i = 0; i < kenGrid.Dimension; i++)
            {
                TableRow row = new TableRow();
                tbl.Rows.Add(row);

                for (int j = 0; j < kenGrid.Dimension; j++)
                {
                    CellViewModel cellView = new CellViewModel(kenGrid.CellMatrix[i, j]);                    

                    TableCell cell = new TableCell();
                    DefineBorder(cell, cellView);
                    row.Cells.Add(cell);

                    KenCell kenCell = (KenCell)LoadControl("KenCell.ascx");
                    kenCell.ID = "kencell-" + i.ToString() + "-" + j.ToString();
                    kenCell.Cell = cellView;

                    cell.Controls.Add(kenCell);                                        
                }
            }
        }

        private void DefineBorder(TableCell cell, CellViewModel cellView)
        {
            string color = "#8B8378";

            if (cellView.BorderTop)
                cell.Style.Add("border-top-color", color);

            if (cellView.BorderBottom)
                cell.Style.Add("border-bottom-color", color);

            if (cellView.BorderLeft)
                cell.Style.Add("border-left-color", color);

            if (cellView.BorderRight)
                cell.Style.Add("border-right-color", color);                    
        }

        internal void HighlightCell(int row, int column)
        {
            KenCell cell = (KenCell)this.FindControlRecursive("kencell-" + row.ToString() + "-" + column.ToString());            
            cell.Highlight();            
        }        
    }
}