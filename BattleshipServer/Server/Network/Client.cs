using System.Net.Sockets;

namespace BattleshipServer.Server.Network
{
    public abstract class Client : IDisposable
    {
        public readonly Socket Socket;

        public event Action<Packet>? OnPacket;
        public event Action<Client>? OnDisconnect;

        protected bool disposed = false;

        public Client(Socket socket)
        {
            Socket = socket;
        }

        // Receive data from the client
        protected void Receive()
        {
            if (disposed) return;

            var recvBuffer = new byte[1024]; // max buffer size
            var state = recvBuffer;

            Socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, EndReceive, state);
        }

        // Parse data from client
        private void EndReceive(IAsyncResult iar)
        {
            if (disposed) return;

            var bytesRead = Socket.EndReceive(iar, out var errorCode);

            if (errorCode != SocketError.Success)
            {
                Dispose();
            }
            else if (bytesRead > 0)
            {
                var buffer = (byte[])iar.AsyncState!;
                var packet = new Packet(buffer);

                OnPacket?.Invoke(packet);

                Receive();
            }
        }

        // Send a packet synchronously
        protected void SendSync(Packet packet)
        {
            if (disposed) return;

            int sent = Socket.Send(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, out var error);

            if (error != SocketError.Success)
            {
                Dispose();
            }
        }

        // Send a packet asynchronously
        protected void SendAsync(Packet packet)
        {
            if (disposed) return;

            Socket.BeginSend(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, out var error, EndSend, packet);

            if (error != SocketError.Success || error != SocketError.IOPending)
            {
                Dispose();
            }
        }

        // Override on class inheriting from this one, that class can decide whether to
        // use SendAsync or SendSync
        public abstract void SendPacket(Packet packet);

        private void EndSend(IAsyncResult ar)
        {
            if (disposed) return;

            int sent = Socket.EndSend(ar, out var error);

            if (error != SocketError.Success)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                OnDisconnect?.Invoke(this);

                Socket?.Shutdown(SocketShutdown.Both);
                Socket?.Close();

                Socket?.Dispose();
            }
        }
    }
}
