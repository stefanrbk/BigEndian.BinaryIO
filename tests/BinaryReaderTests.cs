using java.io;

using NUnit.Framework;

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BigEndianTests
{
    public partial class BeBinaryReaderTests
    {
        private static Stream CreateMemStream(byte[] array) =>
            new MemoryStream(array);
        private static ByteArrayOutputStream CreateOutStream(int size) =>
            new(size);
        [Test]
        public void BeBinaryReader_DisposeTests()
        {
            using var memory = CreateMemStream(new byte[128]);
            using var reader = new BeBinaryReader(memory);

            reader.Dispose();
            reader.Dispose();
            reader.Dispose();
        }
        [Test]
        public void BeBinaryReader_CloseTests()
        {
            using var memory = CreateMemStream(new byte[128]);
            using var reader = new BeBinaryReader(memory);

            reader.Close();
            reader.Close();
            reader.Close();
        }
        [Test]
        public void BeBinaryReader_DisposeTests_Negative()
        {
            using var memStream = CreateMemStream(new byte[128]);
            var BeBinaryReader = new BeBinaryReader(memStream);
            BeBinaryReader.Dispose();
            ValidateDisposedExceptions(BeBinaryReader);
        }
        [Test]
        public void BeBinaryReader_CloseTests_Negative()
        {
            using var memStream = CreateMemStream(new byte[128]);
            var BeBinaryReader = new BeBinaryReader(memStream);
            BeBinaryReader.Close();
            ValidateDisposedExceptions(BeBinaryReader);
        }

        private static void ValidateDisposedExceptions(BeBinaryReader BeBinaryReader)
        {
            var byteBuffer = new byte[10];
            var charBuffer = new char[10];

            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.PeekChar());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.Read());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.Read(byteBuffer, 0, 1));
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.Read(charBuffer, 0, 1));
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadBoolean());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadByte());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadBytes(1));
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadChar());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadChars(1));
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadDecimal());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadDouble());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadInt16());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadInt32());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadInt64());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadSByte());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadSingle());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadString());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadShortString());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadUInt16());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadUInt32());
            Assert.Throws<ObjectDisposedException>(() => BeBinaryReader.ReadUInt64());
        }

        [TestCase((short)-12263)]
        public void BeBinaryReader_ReadInt16(short expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeShort(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadInt16();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((ushort)53273)]
        public void BeBinaryReader_ReadUInt16(ushort expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeShort(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadUInt16();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((int)1_082_135_433)]
        public void BeBinaryReader_ReadInt32(int expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeInt(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadInt32();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((uint)1_082_135_433)]
        public void BeBinaryReader_ReadUInt32(uint expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeInt((int)expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadUInt32();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(0x3b235f263e56614b)]
        public void BeBinaryReader_ReadInt64(long expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeLong(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadInt64();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((ulong)0x3b235f263e56614b)]
        public void BeBinaryReader_ReadUInt64(ulong expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeLong((long)expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadUInt64();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((float)8.0119379474248503295485341968759340044135)]
        public void BeBinaryReader_ReadSingle(float expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeFloat(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadSingle();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((double)8.0119379474248503295485341968759340044135)]
        public void BeBinaryReader_ReadDouble(double expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeDouble(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadDouble();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((double)8.0119379474248503295485341968759340044135)]
        public void BeBinaryReader_ReadDecimal(double d)
        {
            var expected = new decimal(d);
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            var bits = Decimal.GetBits(expected).Reverse();
            foreach (var i in bits)
                dataOut.writeInt(i);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadDecimal();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("Hello")]
        public void BeBinaryReader_ReadString(string expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeUTF(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BeBinaryReader(memStream);
            var actual = reader.ReadShortString();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}