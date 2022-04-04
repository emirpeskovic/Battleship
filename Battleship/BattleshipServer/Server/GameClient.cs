using BattleshipServer.Server.Network;
using System.Net.Sockets;

namespace BattleshipServer.Server
{
    public class GameClient : Client
    {
        public GameClient(Socket socket) : base(socket)
        {
        }

        public override void SendPacket(Packet packet)
        {
            lock (this)
            {
                SendAsync(packet);
            }
        }
    }
}
