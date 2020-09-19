using java.io;

using NUnit.Framework;

using System;
using System.IO;
using System.Linq;
using System.Text;

using BinaryReader = System.IO.BigEndian.BinaryReader;

namespace BigEndianTests
{
    public partial class BinaryReaderTests
    {
        private static Stream CreateMemStream(byte[] array) =>
            new MemoryStream(array);
        private static ByteArrayOutputStream CreateOutStream(int size) =>
            new ByteArrayOutputStream(size);
        [Test]
        public void BinaryReader_DisposeTests()
        {
            using var memory = CreateMemStream(new byte[128]);
            using var reader = new BinaryReader(memory);

            reader.Dispose();
            reader.Dispose();
            reader.Dispose();
        }
        [Test]
        public void BinaryReader_CloseTests()
        {
            using var memory = CreateMemStream(new byte[128]);
            using var reader = new BinaryReader(memory);

            reader.Close();
            reader.Close();
            reader.Close();
        }
        [Test]
        public void BinaryReader_DisposeTests_Negative()
        {
            using var memStream = CreateMemStream(new byte[128]);
            var binaryReader = new BinaryReader(memStream);
            binaryReader.Dispose();
            ValidateDisposedExceptions(binaryReader);
        }
        [Test]
        public void BinaryReader_CloseTests_Negative()
        {
            using var memStream = CreateMemStream(new byte[128]);
            var binaryReader = new BinaryReader(memStream);
            binaryReader.Close();
            ValidateDisposedExceptions(binaryReader);
        }

        private void ValidateDisposedExceptions(BinaryReader binaryReader)
        {
            byte[] byteBuffer = new byte[10];
            char[] charBuffer = new char[10];

            Assert.Throws<ObjectDisposedException>(() => binaryReader.PeekChar());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.Read());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.Read(byteBuffer, 0, 1));
            Assert.Throws<ObjectDisposedException>(() => binaryReader.Read(charBuffer, 0, 1));
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadBoolean());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadByte());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadBytes(1));
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadChar());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadChars(1));
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadDecimal());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadDouble());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadInt16());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadInt32());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadInt64());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadSByte());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadSingle());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadString());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadShortString());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadUInt16());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadUInt32());
            Assert.Throws<ObjectDisposedException>(() => binaryReader.ReadUInt64());
        }

        [TestCase((short)-12263)]
        public void BinaryReader_ReadInt16(short expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeShort(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadInt16();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((ushort)53273)]
        public void BinaryReader_ReadUInt16(ushort expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeShort(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadUInt16();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((int)1_082_135_433)]
        public void BinaryReader_ReadInt32(int expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeInt(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadInt32();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((uint)1_082_135_433)]
        public void BinaryReader_ReadUInt32(uint expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeInt((int)expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadUInt32();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(0x3b235f263e56614b)]
        public void BinaryReader_ReadInt64(long expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeLong(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadInt64();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((ulong)0x3b235f263e56614b)]
        public void BinaryReader_ReadUInt64(ulong expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeLong((long)expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadUInt64();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((float)8.0119379474248503295485341968759340044135)]
        public void BinaryReader_ReadSingle(float expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeFloat(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadSingle();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((double)8.0119379474248503295485341968759340044135)]
        public void BinaryReader_ReadDouble(double expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeDouble(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadDouble();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase((double)8.0119379474248503295485341968759340044135)]
        public void BinaryReader_ReadDecimal(double d)
        {
            var expected = new decimal(d);
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            var bits = decimal.GetBits(expected).Reverse();
            foreach (var i in bits)
                dataOut.writeInt(i);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadDecimal();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("Hello")]
        public void BinaryReader_ReadString(string expected)
        {
            using var outStream = CreateOutStream(16);
            using var dataOut = new DataOutputStream(outStream);
            dataOut.writeUTF(expected);
            dataOut.flush();

            using var memStream = CreateMemStream(outStream.toByteArray());
            using var reader = new BinaryReader(memStream);
            var actual = reader.ReadShortString();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}