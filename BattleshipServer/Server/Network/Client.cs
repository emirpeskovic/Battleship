using System.Net.Sockets;

namespace BattleshipServer.Server.Network
{
    public abstract class Client : IDisposable
    {
        protected readonly Socket socket;

        public event Action<Packet>? OnPacket;
        public event Action<Client>? OnDisconnect;

        protected bool disposed = false;

        public Client(Socket socket)
        {
            this.socket = socket;

            Receive();
        }

        protected void Receive()
        {
            if (disposed) return;

            var recvBuffer = new byte[1024]; // max buffer size
            var state = recvBuffer;

            socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, EndReceive, state);
        }

        private void EndReceive(IAsyncResult ar)
        {
            if (disposed) return;

            var bytesRead = socket.EndReceive(ar, out var errorCode);

            if (bytesRead == 0 || errorCode != SocketError.Success || ar.AsyncState == null)
            {
                Dispose();
            }
            else
            {
                var buffer = (byte[])ar.AsyncState;
                var packet = new Packet(buffer);

                OnPacket?.Invoke(packet);

                Receive();
            }
        }

        protected void SendSync(Packet packet)
        {
            if (disposed) return;

            int sent = socket.Send(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, out var error);

            if (sent == 0 || error != SocketError.Success)
            {
                Dispose();
            }
        }

        protected void SendAsync(Packet packet)
        {
            if (disposed) return;

            socket.BeginSend(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, out var error, EndSend, packet);

            if (error != SocketError.Success || error != SocketError.IOPending)
            {
                Dispose();
            }
        }

        public abstract void SendPacket(Packet packet);

        private void EndSend(IAsyncResult ar)
        {
            if (disposed) return;

            int sent = socket.EndSend(ar, out var error);

            if (sent == 0 || error != SocketError.Success)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                socket?.Shutdown(SocketShutdown.Both);
                socket?.Close();

                OnDisconnect?.Invoke(this);

                socket?.Dispose();
            }
        }
    }
}
