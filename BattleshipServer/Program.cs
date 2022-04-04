using BattleshipServer.Server;
using System.Net;

IPAddress address;
int port;

while (!IPAddress.TryParse(Console.ReadLine(), out address!))
{
    Console.WriteLine("Invalid IP address");
}

while (!int.TryParse(Console.ReadLine(), out port))
{
    Console.WriteLine("Invalid port");
}

GameServer server = new GameServer(address, port);
server.Run();