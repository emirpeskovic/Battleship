using BattleshipServer.Server;
using System.Net;

IPAddress address;
int port;

Console.Write("Enter the IP address the server should listen to: ");
while (!IPAddress.TryParse(Console.ReadLine(), out address!))
{
    Console.WriteLine("Invalid IP address");
}

Console.Write("Enter the port the server should listen to: ");
while (!int.TryParse(Console.ReadLine(), out port))
{
    Console.WriteLine("Invalid port");
}

GameServer server = new(address, port);
server.Run();