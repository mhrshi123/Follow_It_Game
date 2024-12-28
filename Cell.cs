using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowIt
{
    internal class Cell
    {
        public int row;
        public int col;
        public Panel? panel;
        public List<Cell> cellsToMove = new List<Cell>();
        public bool visited;

        public Cell( int row, int col, Panel panel)
        {
            this.row = row;
            this.col = col;
            this.panel = panel;
        }

        public Cell(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}
