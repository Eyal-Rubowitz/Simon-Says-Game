using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonGame
{
    public class SimonLink
    {
        public int _data;
        public SimonLink _next;

        public SimonLink(int gameStatus, SimonLink next)
        {
            _data = gameStatus;
            _next = next;
        }
    }
}
