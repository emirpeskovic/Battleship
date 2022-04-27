using BattleshipServer.Server.Network;
using System.Net.Sockets;

namespace BattleshipServer.Server
{
    public class GameClient : Client
    {
        public bool GameMaster { get; set; }
        
        public GameClient(Socket socket) : base(socket)
        {
            Receive();
        }

        public override void SendPacket(Packet packet)
        {
            // TODO: phase this out some day :^)
            lock (this)
            {
                SendAsync(packet);
            }
        }
    }
}
