using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builders
{
    public class Action
    {
        public int name; //имя балки
        public int action; //номер действия
        public string firstCoord; //первая координата
        public string secondCoord; //вторая координата
        public int brigade; //номер бригады
        public Action(int _name, int _action, string _firstCoord, string _secondCoord, int _brigate)
        {
            name = _name;
            action= _action;
            firstCoord = _firstCoord;
            secondCoord = _secondCoord;
            brigade = _brigate;
        }
        public Action() { }
    }
}
