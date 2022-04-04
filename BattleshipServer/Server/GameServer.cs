using BattleshipServer.Server.Network;
using System.Net;
using System.Net.Sockets;

namespace BattleshipServer.Server
{
    public class GameServer
    {
        private Acceptor acceptor;

        public GameServer(IPAddress address, int port)
        {
            acceptor = new Acceptor(address, port);
        }

        public void Run()
        {
            acceptor.OnClientAccepted += ClientConnected;

            acceptor.StartListen();
        }

        private void ClientConnected(Socket obj)
        {
            
        }
    }
}
