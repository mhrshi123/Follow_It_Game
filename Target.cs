using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowIt
{
    internal class Target
    {
        public int row;
        public int col;
        public Panel panel;

        public Target(int row, int col)
        {
            panel = new Panel()
            {
                BackColor = Color.Green,
                Size = new Size(25, 25)
            };
            this.row = row;
            this.col = col;
        }

        public void Add(TableLayoutPanel grid)
        {
            var cellPanel = grid.GetControlFromPosition(col, row) as Panel;
            panel.Location = new Point((cellPanel.Width - panel.Width) / 2, (cellPanel.Height - panel.Height) / 2);
            cellPanel.Controls.Add(panel);
        }
    }
}
