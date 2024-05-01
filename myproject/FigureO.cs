using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public class FigureO : Figure
    {
        public FigureO(Point start)
        {
            state = 0;
            Point cell1 = new Point(start.X, start.Y);
            Point cell2 = new Point(start.X + 1, start.Y);
            Point cell3 = new Point(start.X + 1, start.Y + 1);
            Point cell4 = new Point(start.X, start.Y + 1);
            this.cells.Add(cell1);
            this.cells.Add(cell2);
            this.cells.Add(cell3);
            this.cells.Add(cell4);
            base.getColor();
        }
        public override void rotateFigure()
        {
            
        }
    }
}
