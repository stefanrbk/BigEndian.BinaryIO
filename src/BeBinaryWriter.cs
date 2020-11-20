using System;
using System.Buffers.Binary;
using System.Security;
using System.Text;

namespace System.IO
{
    public class BeBinaryWriter : BinaryWriter, IDisposable
    {
        private readonly Encoding _encoding;
        private readonly Encoder _encoder;
        private byte[]? _largeByteBuffer;
        private int _maxChars;
        private readonly byte[] _buffer = new byte[16];

        protected BeBinaryWriter()
            : this(Stream.Null) { }
        public BeBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
            : base(output, encoding, leaveOpen)
        {
            _encoding = encoding;
            _encoder = _encoding.GetEncoder();
        }
        public BeBinaryWriter(Stream output)
            : this(output, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)) { }

        public BeBinaryWriter(Stream output, Encoding encoding)
            : this(output, encoding, leaveOpen: false) { }

        public override void Write(double value)
        {
            BinaryPrimitives.WriteDoubleBigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 8);
        }
        public override void Write(decimal value)
        {
            var span = _buffer.AsSpan();
            var num = Decimal.GetBits(value);
            BinaryPrimitives.WriteInt32BigEndian(span.Slice(0, 4), num[3]);
            BinaryPrimitives.WriteInt32BigEndian(span.Slice(4, 4), num[2]);
            BinaryPrimitives.WriteInt32BigEndian(span.Slice(8, 4), num[1]);
            BinaryPrimitives.WriteInt32BigEndian(span.Slice(12, 4), num[0]);
            OutStream.Write(_buffer, 0, 16);
        }
        public override void Write(short value)
        {
            BinaryPrimitives.WriteInt16BigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 2);
        }
        [CLSCompliant(false)]
        public override void Write(ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 2);
        }
        public override void Write(int value)
        {
            BinaryPrimitives.WriteInt32BigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 4);
        }
        [CLSCompliant(false)]
        public override void Write(uint value)
        {
            BinaryPrimitives.WriteUInt32BigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 4);
        }
        public override void Write(long value)
        {
            BinaryPrimitives.WriteInt64BigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 8);
        }
        [CLSCompliant(false)]
        public override void Write(ulong value)
        {
            BinaryPrimitives.WriteUInt64BigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 8);
        }
        public override void Write(float value)
        {
            BinaryPrimitives.WriteSingleBigEndian(_buffer, value);
            OutStream.Write(_buffer, 0, 4);
        }
        public void WriteShortString(string value)
        {
            var byteCount = _encoding.GetByteCount(value);
            if (byteCount is > Int16.MaxValue or < 0)
                throw Error.WriteInvalidStringLength(byteCount);
            Write((short)byteCount);
            if (_largeByteBuffer is null)
            {
                _largeByteBuffer = new byte[256];
                _maxChars = _largeByteBuffer.Length / _encoding.GetMaxByteCount(1);
            }
            if (byteCount <= _largeByteBuffer.Length)
            {
                _ = _encoding.GetBytes(value, _largeByteBuffer);
                OutStream.Write(_largeByteBuffer, 0, byteCount);
                return;
            }
            var num = value.Length;
            var num2 = 0;
            ReadOnlySpan<char> span = value;
            if (_encoding.GetType() == typeof(UTF8Encoding))
            {
                while (num > 0)
                {
                    _encoder.Convert(span[num2..], _largeByteBuffer, num <= _maxChars, out var charsUsed, out var bytesUsed, out _);
                    OutStream.Write(_largeByteBuffer, 0, bytesUsed);
                    num2 += charsUsed;
                    num -= charsUsed;
                }
            }
            else
                WriteShortStringWhenEncodingIsNotUtf8(value, byteCount);
        }

        private unsafe void WriteShortStringWhenEncodingIsNotUtf8(string value, int len)
        {
            var num = value.Length;
            var num2 = 0;
            while (num > 0)
            {
                var num3 = (num > _maxChars) ? _maxChars : num;
                if (num2 < 0 || num3 < 0 || num2 > checked(value.Length - num3))
                    throw new ArgumentOutOfRangeException(nameof(value));
                int bytes2;
                fixed(char* ptr = value)
                {
                    var ptr2 = ptr;
                    fixed (byte* bytes = &_largeByteBuffer![0])
                        bytes2 = _encoder.GetBytes((char*)checked(unchecked((nuint)ptr2) + unchecked((nuint)checked(unchecked((nint)num2) * (nint)2))), num3, bytes, _largeByteBuffer.Length, num3 == num);
                }
                OutStream.Write(_largeByteBuffer, 0, bytes2);
                num2 += num3;
                num -= num3;
            }
        }
    }
}
