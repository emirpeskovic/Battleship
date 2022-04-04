using BattleshipServer.Logic.Ships;
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

        public Tile[,] Tiles { get; private set; }

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            Tiles = new Tile[Rows, Columns];
        }
        
        public bool CanPlaceShipOnTiles(Ship ship, int startX, int startY)
        {
            // Check if out of bounds
            if (startX > Rows || startY > Columns)
            {
                return false;
            }
            
            // Check each tile that shapes the ship, if out of bounds
            foreach (var tile in ship.Tiles)
            {
                if (!tile.ShipOccupiesTile) continue;

                var realX = startX + tile.X;
                var realY = startY + tile.Y;

                if (realX < 0 || realX >= Rows || realY < 0 || realY >= Columns)
                {
                    return false;
                }

                // Check if tile is already occupied
                if (Tiles[realX, realY].ShipOccupiesTile)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
