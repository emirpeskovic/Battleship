using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer.Logic.Ships
{
    public class LShip : Ship
    {
        public LShip(ShipRotation shipRotation, Tile[,] tiles, int startX, int startY) : base(ShipType.L, shipRotation, tiles, startX, startY)
        {
        }

        // startX and startY have no business being here, it was just easier to put them here
        // TODO: Don't do that ^
        public static LShip ShipFromRotation(ShipRotation shipRotation, int startX, int startY)
        {
            switch (shipRotation)
            {
                default:
                case ShipRotation.UP:
                    var shipUp = new LShip(shipRotation, new Tile[,] {
                        { new Tile(0, 0, true),     new Tile(0, 1),         new Tile(0, 2) },
                        { new Tile(1, 0, true),     new Tile(1, 1),         new Tile(1, 2) },
                        { new Tile(2, 0, true),     new Tile(2, 1, true),   new Tile(2, 2) }
                    }, startX, startY);
                    return shipUp;
                case ShipRotation.LEFT:
                    var shipLeft = new LShip(shipRotation, new Tile[,] {
                        { new Tile(0, 0, true),     new Tile(1, 0, true),   new Tile(2, 0, true) },
                        { new Tile(0, 1, true),     new Tile(1, 1),         new Tile(2, 1) },
                        { new Tile(0, 2),           new Tile(1, 2),         new Tile(2, 2) }
                    }, startX, startY);
                    return shipLeft;
                case ShipRotation.RIGHT:
                    var shipRight = new LShip(shipRotation, new Tile[,] {
                        { new Tile(0, 0, true),     new Tile(0, 1, true),   new Tile(0, 2, true) },
                        { new Tile(1, 0),           new Tile(1, 1),         new Tile(1, 2, true) },
                        { new Tile(2, 0),           new Tile(2, 1),         new Tile(2, 2) }
                    }, startX, startY);
                    return shipRight;
                case ShipRotation.DOWN:
                    var shipDown = new LShip(shipRotation, new Tile[,] {
                        { new Tile(0, 0, true),     new Tile(1, 0, true),   new Tile(2, 0) },
                        { new Tile(0, 1, true),     new Tile(1, 1),         new Tile(2, 1) },
                        { new Tile(0, 2, true),     new Tile(1, 2),         new Tile(2, 2) }
                    }, startX, startY);
                    return shipDown;
            }
        }
    }
}
