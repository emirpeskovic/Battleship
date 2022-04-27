using BattleshipServer.Server.Network;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
            // TODO: Make server into a lobby server that can host multiple games
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

                clients.Add(client);
            }
        }

        private void ClientDisconnected(Client client)
        {
            // TODO: Announce disconnect to host
            Console.WriteLine($"[GameServer] Client disconnected with IP {client.Socket.RemoteEndPoint}");
            clients.Remove((GameClient)client);
        }

        private void ProcessPacket(Client client, Packet packet)
        {
            switch (packet.OpCode)
            {
                case OpCode.PLAYER_JOINED:
                    ((GameClient)client).GameMaster = clients.Count == 1;

                    Packet pk = new(OpCode.PLAYER_JOINED);
                    pk.Write<string>("Test name");
                    Announce(pk);
                    break;
                case OpCode.CREATE_GAME:
                    break;
                default:
                    Console.WriteLine("[ProcessPacket] Unhandled packet: " + packet.OpCode);
                    Console.WriteLine("[ProcessPacket] Unhandled packet data: " + packet.GetReadableBuffer());
                    break;
            }
        }

        private void Announce(Packet packet)
        {
            lock (clients)
            {
                foreach (GameClient client in clients)
                {
                    client.SendPacket(packet);
                }
            }
        }
    }
}
