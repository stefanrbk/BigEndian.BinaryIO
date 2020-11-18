using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Buffers.Binary;

namespace System.IO
{
    [ComVisible(true)]
    public class BeBinaryReader : BinaryReader, IDisposable
    {
        private readonly byte[] _buffer;
        private readonly Decoder _decoder;
        private readonly int _maxCharsSize;
        private byte[]? _charBytes;
        private char[]? _charBuffer;
        private bool _disposed;

        public BeBinaryReader(Stream input)
            : this(input, new UTF8Encoding()) { }
        public BeBinaryReader(Stream input, Encoding encoding)
            : this(input, encoding, leaveOpen: false) { }
        public BeBinaryReader(Stream input, Encoding encoding, bool leaveOpen)
            : base(input, encoding, leaveOpen)
        {
            _decoder = encoding.GetDecoder();
            _maxCharsSize = encoding.GetMaxCharCount(128);
            var num = encoding.GetMaxByteCount(1);
            if (num < 16)
                num = 16;
            _buffer = new byte[num];
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw Error.FileNotOpen;
        }

        private ReadOnlySpan<byte> InternalRead(int numBytes)
        {
            var _stream = BaseStream;
            ThrowIfDisposed();
            var num = 0;
            do
            {
                var num2 = _stream.Read(_buffer, num, numBytes - num);
                if (num2 == 0)
                    throw Error.EndOfFile;
                num += num2;
            } while (num < numBytes);
            return _buffer;
        }

        public override short ReadInt16() =>
            BinaryPrimitives.ReadInt16BigEndian(InternalRead(2));

        [CLSCompliant(false)]
        public override ushort ReadUInt16() =>
            BinaryPrimitives.ReadUInt16BigEndian(InternalRead(2));

        public override int ReadInt32() =>
            BinaryPrimitives.ReadInt32BigEndian(InternalRead(4));

        [CLSCompliant(false)]
        public override uint ReadUInt32() =>
            BinaryPrimitives.ReadUInt32BigEndian(InternalRead(4));

        public override long ReadInt64() =>
            BinaryPrimitives.ReadInt64BigEndian(InternalRead(8));

        [CLSCompliant(false)]
        public override ulong ReadUInt64() =>
            BinaryPrimitives.ReadUInt64BigEndian(InternalRead(8));
        [SecuritySafeCritical]
        public override float ReadSingle() =>
            BinaryPrimitives.ReadSingleBigEndian(InternalRead(4));

        [SecuritySafeCritical]
        public override double ReadDouble() =>
            BinaryPrimitives.ReadDoubleBigEndian(InternalRead(8));

        public override decimal ReadDecimal()
        {
            var span = InternalRead(16);
            var lo = BinaryPrimitives.ReadInt32BigEndian(span[12..]);
            var mid = BinaryPrimitives.ReadInt32BigEndian(span[8..]);
            var hi = BinaryPrimitives.ReadInt32BigEndian(span[4..]);
            var flags = BinaryPrimitives.ReadInt32BigEndian(span);
            return new decimal(new int[] { lo, mid, hi, flags });
        }
        public string ReadShortString()
        {
            ThrowIfDisposed();
            var num = 0;
            var num2 = ReadInt16();
            var _stream = BaseStream;
            if (num2 < 0)
                throw Error.InvalidStringLength(num2);
            if (num2 == 0)
                return String.Empty;
            if (_charBytes is null)
                _charBytes = new byte[128];
            if (_charBuffer is null)
                _charBuffer = new char[_maxCharsSize];
            StringBuilder? stringBuilder = null;
            do
            {
                var count = (num2 - num > 128) ? 128 : (num2 - num);
                var num3 = _stream.Read(_charBytes, 0, count);
                if (num3 == 0)
                    throw Error.EndOfFile;
                var chars = _decoder.GetChars(_charBytes, 0, num3, _charBuffer, 0);
                if (num == 0 && num3 == num2)
                    return new string(_charBuffer, 0, chars);
                if (stringBuilder is null)
                    stringBuilder = new StringBuilder(Math.Min(num2, (short)360));
                _ = stringBuilder.Append(_charBuffer, 0, chars);
                num += num3;
            } while (num < num2);
            return stringBuilder.ToString();
        }

        protected override void FillBuffer(int numBytes)
        {
            var _stream = BaseStream;

            if (numBytes < 0 || numBytes > _buffer.Length)
                throw Error.OutOfRangeFillBuffer;

            var num = 0;
            int num2;
            ThrowIfDisposed();
            if (numBytes == 1)
            {
                num2 = _stream.ReadByte();
                if (num2 == -1)
                    throw Error.EndOfFile;
                _buffer[0] = (byte)num2;
                return;
            }
            do
            {
                num2 = _stream.Read(_buffer, num, numBytes - num);
                if (num2 == 0)
                    throw Error.EndOfFile;
                num += num2;
            } while (num < numBytes);
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
