using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public class Field
    {
        public List<List<Cell>> cells;
        public int sizeX = 10, sizeY = 20;
        public Field()
        {
            cells = new List<List<Cell>>();
            for (int i = 0; i < sizeY; ++i)
            {
                cells.Add(new List<Cell>());
                for (int j = 0; j < sizeX; ++j)
                {
                    cells[i].Add(new Cell());
                }
            }
        }
    }
}
