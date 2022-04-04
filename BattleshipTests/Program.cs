using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

// Create TcpClient and connect to server
TcpClient client = new();
await client.ConnectAsync(IPEndPoint.Parse("127.0.0.1:3744"));
var stream = client.GetStream();

// New thread to read byte data from the server
new Thread(async() =>
{
    while (true)
    {
        // Read byte data from the server
        var buffer = new byte[1024];
        var bytesRead = await stream.ReadAsync(buffer);
        if (bytesRead == 0)
            continue;

        // Convert byte array to readable hex values with stringbuilder
        var sb = new StringBuilder();
        for (var i = 0; i < bytesRead; i++)
            sb.Append(buffer[i].ToString("X2"));

        Console.WriteLine("[Receive] " + sb.ToString());
    }
}).Start();

// Write loop in main thread
while (true)
{
    Console.Write("[Send] Choose opcode (short): ");
    short opCode;
    while (!short.TryParse(Console.ReadLine(), out opCode))
    {
        Console.WriteLine("Invalid opcode");
    }

    Console.Write("[Send] Packet Data: ");
    var data = Console.ReadLine()!;

    // Convert string to byte array
    var bytes = Encoding.ASCII.GetBytes(data);

    // Copy opcode and bytes to new byte array
    var packet = new byte[bytes.Length + sizeof(short)];
    Array.Copy(BitConverter.GetBytes(opCode), 0, packet, 0, sizeof(short));
    Array.Copy(bytes, 0, packet, sizeof(short), bytes.Length);

    // Write packet to stream
    await stream.WriteAsync(packet, 0, packet.Length);
}