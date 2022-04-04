using System.Net;
using System.Net.Sockets;

namespace BattleshipServer.Server.Network
{
    public class Acceptor : IDisposable
    {
        private Socket socket;

        public bool ShouldAccept { get; set; }

        public event Action<Socket>? OnClientAccepted;
        public event Action<Exception>? OnException;


        public Acceptor(IPAddress address, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(address, port));
        }

        public void StartListen()
        {
            socket.Listen();

            ShouldAccept = true;
            
            BeginAccept();
        }

        private void BeginAccept()
        {
            if (ShouldAccept)
                socket.BeginAccept(EndAccept, null);
        }

        private void EndAccept(IAsyncResult ar)
        {
            try
            {
                Socket client = socket.EndAccept(ar);

                Console.WriteLine("Client connected on: " + client.RemoteEndPoint);

                OnClientAccepted?.Invoke(client);

                BeginAccept();
            }
            catch (Exception e)
            {
                OnException?.Invoke(e);
            }
        }

        public void Dispose()
        {
            socket.Dispose();
        }
    }
}
