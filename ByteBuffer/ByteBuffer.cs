using System;
using System.Collections.Generic;
using System.Text;

public class ByteBuffer : IDisposable
{
    public const int IntSize = sizeof(int);
    public const int ShortSize = sizeof(short);
    public const int FloatSize = sizeof(float);
    public const int LongSize = sizeof(long);

    private List<byte> _buffer;
    private bool _bufferUpdated = false;

    private bool _disposedValue = false; // To detect redundant calls;
    private byte[] _readBuffer;
    private int _readPosition;

    public ByteBuffer()
    {
        _buffer = new List<byte>();
        _readPosition = 0;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public long GetReadPosition()
    {
        return _readPosition;
    }

    public byte[] ToArray()
    {
        return _buffer.ToArray();
    }

    public int GetCount()
    {
        return _buffer.Count;
    }

    public int GetLength()
    {
        return GetCount() - _readPosition;
    }

    public void Clear()
    {
        _buffer.Clear();
        _readPosition = 0;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Clear();
            }

            _disposedValue = true;
        }
    }


    #region WRITE_DATA

    public void WriteBytes(byte[] value)
    {
        _buffer.AddRange(value);
        _bufferUpdated = true;
    }

    public void WriteShort(short value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
        _bufferUpdated = true;
    }

    public void WriteInt(int value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
        _bufferUpdated = true;
    }

    public void WriteFloat(float value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
        _bufferUpdated = true;
    }

    public void WriteLong(long value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
        _bufferUpdated = true;
    }

    public void WriteString(string value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value.Length));
        _buffer.AddRange(Encoding.ASCII.GetBytes(value));
        _bufferUpdated = true;
    }

    #endregion

    #region READ_DATA

    public byte[] ReadBytes(int length, bool peek = true)
    {
        if (_buffer.Count > _readPosition)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffer.ToArray();
                _bufferUpdated = false;
            }

            byte[] value = _buffer.GetRange(_readPosition, length).ToArray();
            if (peek && _buffer.Count > _readPosition)
            {
                _readPosition += length;
            }

            return value;
        }
        else
        {
            throw new Exception("ByteBuffer is past limit");
        }
    }

    public short ReadShort(bool peek = true)
    {
        if (_buffer.Count > _readPosition)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffer.ToArray();
                _bufferUpdated = false;
            }

            short value = BitConverter.ToInt16(_readBuffer, _readPosition);
            if (peek && _buffer.Count > _readPosition)
            {
                _readPosition += ShortSize;
            }

            return value;
        }
        else
        {
            throw new Exception("ByteBuffer is past limit");
        }
    }

    public float ReadFloat(bool peek = true)
    {
        if (_buffer.Count > _readPosition)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffer.ToArray();
                _bufferUpdated = false;
            }

            float value = BitConverter.ToSingle(_readBuffer, _readPosition);
            if (peek && _buffer.Count > _readPosition)
            {
                _readPosition += FloatSize;
            }

            return value;
        }
        else
        {
            throw new Exception("ByteBuffer is past limit");
        }
    }

    public long ReadLong(bool peek = true)
    {
        if (_buffer.Count > _readPosition)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffer.ToArray();
                _bufferUpdated = false;
            }

            long value = BitConverter.ToInt64(_readBuffer, _readPosition);
            if (peek && _buffer.Count > _readPosition)
            {
                _readPosition += LongSize;
            }

            return value;
        }
        else
        {
            throw new Exception("ByteBuffer is past limit");
        }
    }

    public int ReadInteger(bool peek = true)
    {
        if (_buffer.Count > _readPosition)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffer.ToArray();
                _bufferUpdated = false;
            }

            int value = BitConverter.ToInt32(_readBuffer, _readPosition);
            if (peek && _buffer.Count > _readPosition)
            {
                _readPosition += IntSize;
            }

            return value;
        }
        else
        {
            throw new Exception("ByteBuffer is past limit");
        }
    }

    public string ReadString(bool peek = true)
    {
        int length = ReadInteger(true);
        if (_buffer.Count > _readPosition)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffer.ToArray();
                _bufferUpdated = false;
            }

            string value = Encoding.ASCII.GetString(_readBuffer, _readPosition, length);
            if (peek && _buffer.Count > _readPosition)
            {
                if (value.Length > 0)
                {
                    _readPosition += length;
                }
            }

            return value;
        }
        else
        {
            throw new Exception("ByteBuffer is past limit");
        }
    }

    #endregion
}