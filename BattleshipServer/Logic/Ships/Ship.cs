namespace BattleshipServer.Logic.Ships
{
    // Different types of ships
    public enum ShipType
    {
        L,
        I,
        T,
        Z,
        K,
    }

    public enum ShipRotation
    {
        UP,
        LEFT,
        DOWN,
        RIGHT
    }
    
    public class Ship : IDisposable
    {
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        
        public ShipType ShipType { get; private set; }
        public ShipRotation ShipRotation { get; private set; }
        public Tile[,] Tiles { get; private set; }

        public Ship(ShipType shipType, ShipRotation shipRotation, Tile[,] tiles, int startX, int startY)
        {
            ShipType = shipType;
            ShipRotation = shipRotation;
            Tiles = tiles;
            StartX = startX;
            StartY = startY;
        }

        public void Dispose()
        {
            // ???
        }
    }
}
