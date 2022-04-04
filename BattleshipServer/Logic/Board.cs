using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer.Logic
{
    // Each player is assigned one board
    // Boards store all tiles and ships
    public class Board
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
    }
}
