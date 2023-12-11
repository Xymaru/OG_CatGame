using System;
using UnityEngine;
using System.Runtime.InteropServices;


namespace PawsAndClaws.Utils
{
    public interface IReader<T>
    {
        T Read(byte[] buff, int offset, ref int size);
    }
    public class Reader<T> : IReader<T>
    {
        public static readonly IReader<T> P = Reader.P as IReader<T> ?? new Reader<T>();
        public T Read(byte[] buff, int offset, ref int size)
        {
            throw new NotImplementedException();
        }
    }
    public class Reader : IReader<char>, IReader<int>, IReader<byte>,
        IReader<long>, IReader<ulong>, IReader<uint>, IReader<float>,
        IReader<double>, IReader<bool>, IReader<ushort>
    {
        public static Reader P = new Reader();

        public byte Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(byte);
            return buff[offset];
        }

        char IReader<char>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(char);
            return BitConverter.ToChar(buff, offset);
        }

        int IReader<int>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(int);
            return BitConverter.ToInt32(buff, offset);
        }

        long IReader<long>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(long);
            return BitConverter.ToInt64(buff, offset);
        }

        ulong IReader<ulong>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(ulong);
            return BitConverter.ToUInt64(buff, offset);
        }

        uint IReader<uint>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(uint);
            return BitConverter.ToUInt32(buff, offset);
        }

        float IReader<float>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(float);
            return BitConverter.ToSingle(buff, offset);
        }

        ushort IReader<ushort>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(ushort);
            return BitConverter.ToUInt16(buff, offset);
        }

        bool IReader<bool>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(bool);
            return BitConverter.ToBoolean(buff, offset);
        }

        double IReader<double>.Read(byte[] buff, int offset, ref int size)
        {
            size = sizeof(double);
            return BitConverter.ToDouble(buff, offset);
        }
    }
    public class BlobStreamReader
    {
        public int Position => _position;
        public byte[] Data => _buffer;
        private int _position;
        private byte[] _buffer;
        public BlobStreamReader(byte[] buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public T Read<T>() 
        {
            int size = 0;
            T val = Reader<T>.P.Read(_buffer,  _position, ref size);
            _position += size;
            return val;
        }
        public void Skip(int offset)
        {
            _position += offset;
        }
        public void Skip<T>()
        {
            _position += Marshal.SizeOf(typeof(T));
        }

    }

    public class BlobStreamWriter
    {
        public int Position => _position;
        public byte[] Data => _buffer;
        public int Length => _bufferSize;

        private byte[] _buffer;
        private int _position;
        private int _bufferSize;
        public BlobStreamWriter(byte[] buffer)
        {
            Debug.Assert(buffer != null && buffer.Length != 0);
            _buffer = buffer;
            _position = 0;
            _bufferSize = buffer.Length;
        }
        public BlobStreamWriter(byte[] buffer, int size)
        {
            Debug.Assert(buffer != null && size != 0);
            _buffer = buffer;
            _position = 0;
            _bufferSize = size;
        }
        public void Write(byte value)
        {
            Debug.Assert((_bufferSize - _position) >= sizeof(byte), "Buffer is too small to contain the value");
            // Copy the bytes to the buffer.
            _buffer[_position] = value;
            // Update the position.
            _position += sizeof(byte);
        }
        public void Write(bool value)
        {
            Debug.Assert((_bufferSize - _position) >= sizeof(bool), "Buffer is too small to contain the value");
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }
        public void Write(int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }
        public void Write(uint value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }
        public void Write(ushort value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }

        public void Write(float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }

        public void Write(double value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }

        public void Write(long value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }

        public void Write(ulong value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }

        public void Write(char value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer);
        }

        public void Write(byte[] buffer)
        {
            Debug.Assert((_bufferSize - _position) >= buffer.Length, "Buffer is too small to contain the value");
            buffer.CopyTo(_buffer, _position);
            _position += buffer.Length;
        }

        public void Skip(int offset)
        {
            _position += offset;
        }

    }
}