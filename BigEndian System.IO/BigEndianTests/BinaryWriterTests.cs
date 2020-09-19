using NUnit.Framework;

using System;
using System.IO;

using BinaryWriter = System.IO.BigEndian.BinaryWriter;
using BinaryReader = System.IO.BigEndian.BinaryReader;

namespace BigEndianTests
{
    public class BinaryWriterTests
    {
        [Test]
        public void BinaryWriter_WriteBoolTest()
        {
            using var mstr = CreateMemStream();
            using var bw = new BinaryWriter(mstr);
            using var br = new BinaryReader(mstr);

            bw.Write(false);
            bw.Write(false);
            bw.Write(true);
            bw.Write(false);
            bw.Write(true);
            bw.Write(5);
            bw.Write(0);

            bw.Flush();
            mstr.Position = 0;

            Assert.That(br.ReadBoolean(), Is.EqualTo(false));
            Assert.That(br.ReadBoolean(), Is.EqualTo(false));
            Assert.That(br.ReadBoolean(), Is.EqualTo(true));
            Assert.That(br.ReadBoolean(), Is.EqualTo(false));
            Assert.That(br.ReadBoolean(), Is.EqualTo(true));
            Assert.That(br.ReadInt32(), Is.EqualTo(5));
            Assert.That(br.ReadInt32(), Is.EqualTo(0));
        }
        [TestCaseSource(nameof(FloatValues))]
        public void BinaryWriter_WriteSingleTest(float expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadSingle());
        [TestCaseSource(nameof(DecimalValues))]
        public void BinaryWriter_WriteDecimalTest(decimal expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadDecimal());
        [TestCaseSource(nameof(DoubleValues))]
        public void BinaryWriter_WriteDoubleTest(double expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadDouble());
        [TestCaseSource(nameof(ShortValues))]
        public void BinaryWriter_WriteInt16Test(short expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadInt16());
        [TestCaseSource(nameof(IntValues))]
        public void BinaryWriter_WriteInt32Test(int expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadInt32());
        [TestCaseSource(nameof(LongValues))]
        public void BinaryWriter_WriteInt64Test(long expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadInt64());
        [TestCaseSource(nameof(UShortValues))]
        public void BinaryWriter_WriteUInt16Test(ushort expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadUInt16());
        [TestCaseSource(nameof(UIntValues))]
        public void BinaryWriter_WriteUInt32Test(uint expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadUInt32());
        [TestCaseSource(nameof(ULongValues))]
        public void BinaryWriter_WriteUInt64Test(ulong expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadUInt64());
        [TestCaseSource(nameof(StringValues))]
        public void BinaryWriter_WriteStringTest(string expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadString());
        [TestCaseSource(nameof(StringValues))]
        public void BinaryWriter_WriteShortStringTest(string expected) =>
            WriteTest(expected, (bw, s) => bw.WriteShortString(s), br => br.ReadShortString());


        private static Stream CreateMemStream() =>
            new MemoryStream();

        private static void WriteTest<T>(T expected, Action<BinaryWriter, T> write, Func<BinaryReader, T> read)
        {
            using var memStream = CreateMemStream();
            using var writer = new BinaryWriter(memStream);
            using var reader = new BinaryReader(memStream);

            write(writer, expected);

            writer.Flush();
            memStream.Position = 0;

            var actual = read(reader);
            Assert.That(actual, Is.EqualTo(expected));

            Assert.Throws<EndOfStreamException>(() => read(reader));
        }

        public readonly static float[] FloatValues = new float[]
        {
            float.MinValue,
            float.MaxValue,
            float.Epsilon,
            float.PositiveInfinity,
            float.NegativeInfinity,
            new float(),
            0,
            -1e20f,
            -3.5e-20f,
            1.4e-10f,
            10000.2f,
            2.3e30f
        };
        public readonly static decimal[] DecimalValues = new decimal[]
        {
            decimal.One,
            decimal.Zero,
            decimal.MinusOne,
            decimal.MinValue,
            decimal.MaxValue,
            new decimal(-1000.5),
            new decimal(-10.0e-40),
            new decimal(3.4e-40898),
            new decimal(3.4e-28),
            new decimal(3.4e+28),
            new decimal(0.45),
            new decimal(5.55),
            new decimal(3.4899e23)
        };
        public readonly static double[] DoubleValues = new double[]
        {
            double.NegativeInfinity,
            double.PositiveInfinity,
            double.Epsilon,
            double.MinValue,
            double.MaxValue,
            -3e59,
            -1000.5,
            -1e-40,
            3.4e-37,
            0.45,
            5.55,
            3.4899e233
        };
        public readonly static short[] ShortValues = new short[]
            {
                short.MinValue,
                short.MaxValue,
                0,
                -10000,
                10000,
                -50,
                50
            };
        public readonly static int[] IntValues = new int[]
            {
                int.MinValue,
                int.MaxValue,
                0,
                -10000,
                10000,
                -50,
                50
            };
        public readonly static long[] LongValues = new long[]
            {
                long.MinValue,
                long.MaxValue,
                0,
                -10000,
                10000,
                -50,
                50
            };
        public readonly static ushort[] UShortValues = new ushort[]
            {
                ushort.MinValue,
                ushort.MaxValue,
                0,
                100,
                1000,
                10000,
                ushort.MaxValue - 100
            };
        public readonly static uint[] UIntValues = new uint[]
            {
                uint.MinValue,
                uint.MaxValue,
                0,
                100,
                1000,
                10000,
                uint.MaxValue - 100
            };
        public readonly static ulong[] ULongValues = new ulong[]
            {
                ulong.MinValue,
                ulong.MaxValue,
                0,
                100,
                1000,
                10000,
                ulong.MaxValue - 100
            };
        public readonly static string[] StringValues = new string[]
        {
            "ABC",
            "\t\t\n\n\n\0\r\r\v\v\t\0\rHello",
            "This is a normal string",
            "12345667789!@#$%^&&())_+_)@#",
            "ABSDAFJPIRUETROPEWTGRUOGHJDOLJHLDHWEROTYIETYWsdifhsiudyoweurscnkjhdfusiyugjlskdjfoiwueriye",
            "     ",
            "\0\0\0\t\t\tHey\"\"",
            string.Empty,
            "🚗🚀🚁",
            "あいうえお"
        };
    }
}
