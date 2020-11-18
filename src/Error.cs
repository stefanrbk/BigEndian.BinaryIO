using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

using Strings = System.IO.System_IO_BigEndian_Strings;

[assembly: CLSCompliant(true)]

namespace System.IO
{
    internal static class Error
    {
        internal static string Format(string format, object obj) =>
            String.Format(format, obj);
        internal static Exception StreamIsClosed =>
            new ObjectDisposedException(null, Strings.ObjectDisposed_StreamClosed);
        internal static Exception FileNotOpen =>
            new ObjectDisposedException(null, Strings.ObjectDisposed_FileClosed);
        internal static Exception EndOfFile =>
            new EndOfStreamException(Strings.IO_EOF_ReadBeyondEOF);
        internal static Exception OutOfRangeFillBuffer =>
            new ArgumentOutOfRangeException(null, Strings.ArgumentOutOfRange_BinaryReaderFillBuffer);
        internal static Exception InvalidStringLength(int len) =>
            new IOException(Format(Strings.IO_InvalidStringLen_Len, len));
        internal static Exception WriteInvalidStringLength(int len) =>
            new IOException(Format(Strings.IO_BinaryWriterInvalidStringLen, len));
    }
}
