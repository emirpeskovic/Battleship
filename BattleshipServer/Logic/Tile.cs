namespace BattleshipServer.Logic
{
    public class Tile
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public bool ShipOccupiesTile { get; private set; }

        public Tile(int x, int y, bool shipOccupiesTile = false)
        {
            X = x;
            Y = y;
            
            ShipOccupiesTile = shipOccupiesTile;
        }
    }
}
