using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;

using LearnKenKen.DomainModels;
using LearnKenKen.DataModel;
using LearnKenKen.Utilities;

namespace LearnKenKen.Services
{
    static class KenKenGenerator
    {        
        internal static Grid LoadKenKen(int index)
        {
            GridXml grid = Helper.GetXmlConfigData<GridXml>("KenKen_" + (index + 1) + ".xml");

            var cages = new List<Cage>();
            int cageId = 1;

            grid.Cages.ForEach(cage =>
            {
                List<Cell> cells = new List<Cell>();
                cage.Cells.ForEach(cell => cells.Add(new Cell(cell.Row, cell.Column)));

                cages.Add(new Cage(cageId++, Helper.ParseOperation(cage.Operation), cage.TargetValue, cells));
            });

            return new Grid(cages);                        
        }
    }
}