using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builders //класс балок
{
    public class Beam
    {
        public double x1; //верхняя или левая часть балки
        public double y1; 
        public double x2; //нижняя или правая
        public double y2;
        public int location; // 1 - вертикально; 2 - горизонтально
        public int number; //порядковый номер

        public Beam (double _x1, double _y1, double _x2, double _y2, int _location, int _number)
        {
            x1 = _x1;
            y1 = _y1;
            x2 = _x2;
            y2 = _y2;
            location = _location;
            number = _number;
        }
        public Beam() { }
    }
}
