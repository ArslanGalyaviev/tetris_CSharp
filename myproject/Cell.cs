using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myproject
{
    public class Cell
    {
        public bool blocked;
        public Figure figure;
        public Cell() 
        {
            blocked = false;            
        }
        public void switchBlock(Figure f)
        {
            blocked = true;
            figure = f;
        }

    }
}
