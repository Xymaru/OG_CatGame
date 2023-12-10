using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PawsAndClaws.Utils
{
    public class BlobStreamWriter
    {
        public int Position => _position;
        
        private byte[] _buffer;
        private int _position;
        private int _bufferSize;

        public BlobStreamWriter(ref byte[] buffer, int size)
        {
            Debug.Assert(buffer != null && size != 0);
            _buffer = buffer;
            _position = 0;
            _bufferSize = size;
        }

        
        public void Write<T>(T value) where T : struct
        {
            // Get the GetBytes method info for the type of T.
            var getBytesMethod = typeof(BitConverter).GetMethod("GetBytes", new[] { typeof(T) });
            if (getBytesMethod == null)
            {
                throw new ArgumentException("Type not supported by BitConverter");
            }

            // Invoke the method to get the byte array.
            byte[] bytes = (byte[])getBytesMethod.Invoke(null, new object[] { value });

            Debug.Assert((_bufferSize - _position) >= bytes.Length, "Buffer is too small to contain the value");

            // Copy the bytes to the buffer.
            bytes.CopyTo(_buffer, _position);

            // Update the position.
            _position += bytes.Length;
        }

        public void Write(byte value)
        {
            Debug.Assert((_bufferSize - _position) >= sizeof(byte), "Buffer is too small to contain the value");
            // Copy the bytes to the buffer.
            _buffer[_position] = value;
            // Update the position.
            _position += sizeof(byte);
        }
        public void Write(byte[] buffer, int size)
        {
            Debug.Assert((_bufferSize - _position) >= size, "Buffer is too small to contain the value");
            
            // Copy the bytes to the buffer.
            Buffer.BlockCopy(buffer, 0, _buffer, _position, size);

            // Update the position.
            _position += size;
        }

        public void Skip(int offset)
        {
            _position += offset;
        }
        
    }
}