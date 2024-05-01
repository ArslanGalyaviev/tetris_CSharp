using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public class Figure
    {
        Random random;
        public List<Point> cells = new List<Point>();
        public SolidBrush color;
        public int state, r, g, b;
        public void getColor()
        {
            random = new Random(DateTime.Now.Millisecond);
            r = random.Next(255);
            g = random.Next(255);
            b = random.Next(255);
            color = new SolidBrush(Color.FromArgb(r, g, b));
        }
        public void moveDown()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Point p = cells[i];
                p.Y++;
                cells[i] = p;
            }
        }
        public void moveLeft()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Point p = cells[i];
                p.X--;
                cells[i] = p;
            }
        }
        public void moveRight()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Point p = cells[i];
                p.X++;
                cells[i] = p;
            }
        }
        public virtual void rotateFigure()
        {

        }
    }

}
