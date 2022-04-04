using BattleshipServer.Server.Network;
using System.Net;
using System.Net.Sockets;

namespace BattleshipServer.Server
{
    public class GameServer
    {
        private readonly Acceptor acceptor;
        private readonly List<GameClient> clients;

        private bool running = false;

        public GameServer(IPAddress address, int port)
        {
            acceptor = new Acceptor(address, port);
            clients = new();
        }

        public void Run()
        {
            acceptor.OnClientAccepted += ClientConnected;
            acceptor.StartListen();

            running = true;

            Console.WriteLine("[GameServer] Running...");
            Console.WriteLine("[GameServer] Type 'exit' to stop the server.");

            while (running)
            {
                string input = Console.ReadLine()!;

                if (input == "exit")
                {
                    running = false;
                }
            }
        }

        private void ClientConnected(Socket obj)
        {
            GameClient client = new(obj);

            client.OnPacket += ProcessPacket;
            client.OnDisconnect += ClientDisconnected;

            clients.Add(client);
        }

        private void ClientDisconnected(Client client)
        {
            // TODO: Announce disconnect to host
            clients.Remove((GameClient)client);

        }

        private void ProcessPacket(Packet packet)
        {
            switch (packet.OpCode)
            {
                default:
                    Console.WriteLine("[GameServer] Unhandled packet: " + packet.OpCode);
                    break;
            }
        }

        private void Announce(Packet packet) => clients.ForEach(client => client.SendPacket(packet));
    }
}
