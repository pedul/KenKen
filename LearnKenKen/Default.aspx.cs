/**
 * Preeti Edul
 * http://preetiedul.wordpress.com/2011/05/17/kenken/ 
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LearnKenKen.DomainModels;
using LearnKenKen.UserControls;
using LearnKenKen.UIModels;
using LearnKenKen.Services;
using LearnKenKen.Utilities;
using LearnKenKen.DataModel;

namespace LearnKenKen
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadKenKenPuzzleList();
            }
        }

        void LoadKenKenPuzzleList()
        {            
            List<GridXml> grids = Helper.GetXmlConfigData<List<GridXml>>("KenKenGridList.xml");

            List<KenListItemViewModel> listKens = new List<KenListItemViewModel>();                        
            grids.ForEach(grid => listKens.Add(new KenListItemViewModel(grid.DisplayName, grid.Index)));

            lstKen.Items = listKens;
            lstKen.SelectKenKen = 3;
        }

        protected void lstKen_KenKenSelected(object sender, KenListEventArgs e)
        {
            DisplayKenKen(e.SelectedIndex);
        }

        void DisplayKenKen(int index)
        {
            spanSolved.Visible = false;
            btnSolve.Visible = true;
            btnSolve.Text = "Take a Step !";

            var grid = KenKenGenerator.LoadKenKen(index);

            SaveSession(grid, false, 1);
            kenGrid.Grid = grid;
        }

        protected void btnSolveStepByStep_Click(object sender, EventArgs e)
        {
            Logger log = null;
            Grid grid = null;
            int steps = 0;
            bool prevSolved = false, solved = false;

            LoadSession(out grid, out prevSolved, out steps);
            kenGrid.Grid = grid;

            KenKenSolver kenSolver = new KenKenSolver();
            solved = kenSolver.Solve(grid, out log, true);

            if (prevSolved)
            {
                NoMoreSteps(true, steps);
            }
            else if (log.CellId == null)
            {
                NoMoreSteps(false, steps);
            }
            else
            {
                UpdateLog(log, true);
                btnSolve.Text = "OK ! Take Step " + ++steps;
            }

            SaveSession(grid, solved, steps);
        }

        protected void btnJump_Click(object sender, EventArgs e)
        {
            SolveKenKen(false);
        }

        void SolveKenKen(bool stepByStep)
        {
            Logger log = null;
            Grid grid = null;
            int steps = 0;
            bool prevSolved = false, solved = false;

            LoadSession(out grid, out prevSolved, out steps);            

            KenKenSolver kenSolver = new KenKenSolver();
            solved = kenSolver.Solve(grid, out log, stepByStep);
            
            kenGrid.Grid = grid;
            
            if (prevSolved)
            {
                NoMoreSteps(true, steps);
            }
            else if (log.CellId == null)
            {
                NoMoreSteps(false, steps);
            }
            else
            {
                UpdateLog(log, false);
                btnSolve.Text = "OK ! Take Step " + ++steps;
            }

            SaveSession(grid, solved, steps);
        }

        void NoMoreSteps(bool puzzleSolved, int steps)
        {
            btnSolve.Visible = false;
            currentLog.Visible = false;

            spanSolved.Visible = true;

            if (puzzleSolved)
                spanSolved.InnerHtml = "Done ! KenKen Puzzle solved in " + steps + " steps :)";                
            else
                spanSolved.InnerHtml = "Stuck ! Failed to solve KenKen Puzzle :(";
        }

        void LoadSession(out Grid grid, out bool prevSolved, out int steps)
        {
            grid = (Grid)Session["grid"];
            prevSolved = (bool)Session["solved"];
            steps = (int)Session["steps"];
        }

        void SaveSession(Grid grid, bool solved, int steps)
        {
            Session["grid"] = grid;
            Session["solved"] = solved;
            Session["steps"] = steps;
        }

        void UpdateLog(Logger log, bool highlightCell)
        {
            currentLog.LogData = new LogViewModel(log);

            if (highlightCell)
                kenGrid.HighlightCell(log.CellRow, log.CellColumn);
        } 
    }
}