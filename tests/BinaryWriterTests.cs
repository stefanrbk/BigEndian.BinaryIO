using NUnit.Framework;

using System;
using System.IO;

namespace BigEndianTests
{
    public class BeBinaryWriterTests
    {
        [Test]
        public void BeBinaryWriter_WriteBoolTest()
        {
            using var mstr = CreateMemStream();
            using var bw = new BeBinaryWriter(mstr);
            using var br = new BeBinaryReader(mstr);

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
        public void BeBinaryWriter_WriteSingleTest(float expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadSingle());
        [TestCaseSource(nameof(DecimalValues))]
        public void BeBinaryWriter_WriteDecimalTest(decimal expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadDecimal());
        [TestCaseSource(nameof(DoubleValues))]
        public void BeBinaryWriter_WriteDoubleTest(double expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadDouble());
        [TestCaseSource(nameof(ShortValues))]
        public void BeBinaryWriter_WriteInt16Test(short expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadInt16());
        [TestCaseSource(nameof(IntValues))]
        public void BeBinaryWriter_WriteInt32Test(int expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadInt32());
        [TestCaseSource(nameof(LongValues))]
        public void BeBinaryWriter_WriteInt64Test(long expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadInt64());
        [TestCaseSource(nameof(UShortValues))]
        public void BeBinaryWriter_WriteUInt16Test(ushort expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadUInt16());
        [TestCaseSource(nameof(UIntValues))]
        public void BeBinaryWriter_WriteUInt32Test(uint expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadUInt32());
        [TestCaseSource(nameof(ULongValues))]
        public void BeBinaryWriter_WriteUInt64Test(ulong expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadUInt64());
        [TestCaseSource(nameof(StringValues))]
        public void BeBinaryWriter_WriteStringTest(string expected) =>
            WriteTest(expected, (bw, s) => bw.Write(s), br => br.ReadString());
        [TestCaseSource(nameof(StringValues))]
        public void BeBinaryWriter_WriteShortStringTest(string expected) =>
            WriteTest(expected, (bw, s) => bw.WriteShortString(s), br => br.ReadShortString());


        private static Stream CreateMemStream() =>
            new MemoryStream();

        private static void WriteTest<T>(T expected, Action<BeBinaryWriter, T> write, Func<BeBinaryReader, T> read)
        {
            using var memStream = CreateMemStream();
            using var writer = new BeBinaryWriter(memStream);
            using var reader = new BeBinaryReader(memStream);

            write(writer, expected);

            writer.Flush();
            memStream.Position = 0;

            var actual = read(reader);
            Assert.That(actual, Is.EqualTo(expected));

            Assert.Throws<EndOfStreamException>(() => read(reader));
        }

        public readonly static float[] FloatValues = new float[]
        {
            Single.MinValue,
            Single.MaxValue,
            Single.Epsilon,
            Single.PositiveInfinity,
            Single.NegativeInfinity,
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
            Decimal.One,
            Decimal.Zero,
            Decimal.MinusOne,
            Decimal.MinValue,
            Decimal.MaxValue,
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
            Double.NegativeInfinity,
            Double.PositiveInfinity,
            Double.Epsilon,
            Double.MinValue,
            Double.MaxValue,
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
                Int16.MinValue,
                Int16.MaxValue,
                0,
                -10000,
                10000,
                -50,
                50
            };
        public readonly static int[] IntValues = new int[]
            {
                Int32.MinValue,
                Int32.MaxValue,
                0,
                -10000,
                10000,
                -50,
                50
            };
        public readonly static long[] LongValues = new long[]
            {
                Int64.MinValue,
                Int64.MaxValue,
                0,
                -10000,
                10000,
                -50,
                50
            };
        public readonly static ushort[] UShortValues = new ushort[]
            {
                UInt16.MinValue,
                UInt16.MaxValue,
                0,
                100,
                1000,
                10000,
                UInt16.MaxValue - 100
            };
        public readonly static uint[] UIntValues = new uint[]
            {
                UInt32.MinValue,
                UInt32.MaxValue,
                0,
                100,
                1000,
                10000,
                UInt32.MaxValue - 100
            };
        public readonly static ulong[] ULongValues = new ulong[]
            {
                UInt64.MinValue,
                UInt64.MaxValue,
                0,
                100,
                1000,
                10000,
                UInt64.MaxValue - 100
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
            String.Empty,
            "🚗🚀🚁",
            "あいうえお"
        };
    }
}
