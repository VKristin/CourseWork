using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builders
{
    public class Points
    {
        public double x; //координата x
        public double y; //координата y
        public char pointName; //название точки
        public int usage; //0 - неиспользована; 1 - использована

        public Points (double _x, double _y, char _pointName)
        {
            x = _x;
            y = _y;
            pointName = _pointName;
        }
    }
}
