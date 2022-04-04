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
            // Max 2 clients per game
            if (clients.Count == 2)
            {
                obj.Close();
                return;
            }

            lock (clients)
            {
                GameClient client = new(obj);
                
                client.OnPacket += ProcessPacket;
                client.OnDisconnect += ClientDisconnected;

                if (clients.Count == 0)
                    client.GameMaster = true;

                clients.Add(client);
            }
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
                case OpCode.CREATE_GAME:
                    break;
                default:
                    Console.WriteLine("[GameServer] Unhandled packet: " + packet.OpCode);
                    break;
            }
        }

        private void Announce(Packet packet) => clients.ForEach(client => client.SendPacket(packet));
    }
}
