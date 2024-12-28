using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowIt
{
    internal class Player
    {
        public int row;
        public int col;
        public Panel panel;

        public Player(int row, int col)
        {
            panel = new Panel()
            {
                BackColor = Color.Red,
                Size = new Size(50,50)
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
