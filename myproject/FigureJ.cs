using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public class FigureJ : Figure
    {
        public FigureJ(Point start)
        {
            state = 0;
            Point cell1 = new Point(start.X, start.Y);
            Point cell2 = new Point(start.X, start.Y + 1);
            Point cell3 = new Point(start.X + 1, start.Y + 1);
            Point cell4 = new Point(start.X + 2, start.Y + 1);
            this.cells.Add(cell1);
            this.cells.Add(cell2);
            this.cells.Add(cell3);
            this.cells.Add(cell4);
            base.getColor();
        }
        public override void rotateFigure()
        {
            Point p = new Point(this.cells[0].X, this.cells[0].Y);
            this.cells.Clear();
            switch (this.state)
            {
                case 0:
                    this.cells.Add(new Point(p.X + 2, p.Y));
                    this.cells.Add(new Point(p.X + 1, p.Y));
                    this.cells.Add(new Point(p.X + 1, p.Y + 1));
                    this.cells.Add(new Point(p.X + 1, p.Y + 2));
                    break;
                case 1:
                    this.cells.Add(new Point(p.X, p.Y + 2));
                    this.cells.Add(new Point(p.X, p.Y + 1));
                    this.cells.Add(new Point(p.X - 1, p.Y + 1));
                    this.cells.Add(new Point(p.X - 2, p.Y + 1));
                    break;
                case 2:
                    this.cells.Add(new Point(p.X - 2, p.Y));
                    this.cells.Add(new Point(p.X - 1, p.Y));
                    this.cells.Add(new Point(p.X - 1, p.Y - 1));
                    this.cells.Add(new Point(p.X - 1, p.Y - 2));
                    break;
                case 3:
                    this.cells.Add(new Point(p.X, p.Y - 2));
                    this.cells.Add(new Point(p.X, p.Y - 1));
                    this.cells.Add(new Point(p.X + 1, p.Y - 1));
                    this.cells.Add(new Point(p.X + 2, p.Y - 1));
                    break;

            }
            this.state = (this.state + 1) % 4;
        }
    }
}
