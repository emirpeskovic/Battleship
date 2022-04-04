using System.Runtime.InteropServices;

namespace BattleshipServer.Server.Network
{
    public enum OpCode : short
    {
        PLAYER_JOINED,
        CREATE_GAME,
        SET_RULES,

        TILE_CLICK,
    }
    
    public class Packet : IDisposable
    {
        private byte[] Data { get; set; }
        public byte[] Buffer { get => GetBuffer(); }

        // Network operation code so we can figure out what the packet is supposed to do
        public OpCode OpCode { get => (OpCode)BitConverter.ToUInt16(Data, 0); }

        // Get actual buffer size, so we don't send a packet that's 1024 in size every single time
        // Return the buffer in it's actual size, and append the size of the buffer to the beginning of the buffer
        private byte[] GetBuffer()
        {
            if (writer != null)
            {
                byte[] realBuffer = new byte[writer.BaseStream.Position + 2];
                byte[] bufferSize = BitConverter.GetBytes((short)writer.BaseStream.Position);
                Array.Copy(bufferSize, realBuffer, 2);
                Array.Copy(Data, 0, realBuffer, 2, realBuffer.Length);
                return realBuffer;
            }
            else
            {
                return Data;
            }
        }

        private BinaryWriter? writer;
        private BinaryReader? reader;

        public bool Outgoing { get; set; }

        public Packet(OpCode opCode)
        {
            Data = new byte[1024];
            writer = new BinaryWriter(new MemoryStream(Data));

            Write((short)opCode);

            Outgoing = true;
        }

        public Packet(byte[] data)
        {
            Data = data;
            reader = new BinaryReader(new MemoryStream(Data));

            Outgoing = false;
        }

        // Generic write function
        // Make sure the data is actually convertible to bytes by forcing interface recognition
        public void Write<T>(T value) where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, ISpanFormattable, IFormattable
        {
            if (Outgoing)
            {
                byte[] bytes = BitConverter.GetBytes(value as dynamic);
                writer!.Write(bytes);
            }
        }

        // Generic read function
        // Make sure the data is actually convertible to bytes by forcing interface recognition
        public T? Read<T>() where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, ISpanFormattable, IFormattable
        {
            if (!Outgoing)
            {
                try
                {
                    byte[] bytes = reader!.ReadBytes(Marshal.SizeOf<T>());
                    var ptr = Marshal.AllocHGlobal(bytes.Length);
                    Marshal.Copy(bytes, 0, ptr, bytes.Length);

                    // TODO: This is a hack, fix it
                    return (T)Marshal.PtrToStructure(ptr, typeof(T));
                }
                catch (Exception)
                {
                    throw new Exception("Could not read data from packet");
                }
            }
            return default;
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }
    }
}
