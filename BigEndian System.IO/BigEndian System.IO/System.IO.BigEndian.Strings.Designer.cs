﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.IO.BigEndian {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class System_IO_BigEndian_Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System_IO_BigEndian_Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("System.IO.BigEndian.System.IO.BigEndian.Strings", typeof(System_IO_BigEndian_Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of bytes requested does not fit into BinaryReader&apos;s internal buffer..
        /// </summary>
        internal static string ArgumentOutOfRange_BinaryReaderFillBuffer {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_BinaryReaderFillBuffer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BinaryWriter encountered an invalid string length of {0} characters..
        /// </summary>
        internal static string IO_BinaryWriterInvalidStringLen {
            get {
                return ResourceManager.GetString("IO_BinaryWriterInvalidStringLen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read beyond the end of the stream..
        /// </summary>
        internal static string IO_EOF_ReadBeyondEOF {
            get {
                return ResourceManager.GetString("IO_EOF_ReadBeyondEOF", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BinaryReader encountered an invalid string length of {0} characters..
        /// </summary>
        internal static string IO_InvalidStringLen_Len {
            get {
                return ResourceManager.GetString("IO_InvalidStringLen_Len", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot access a closed file..
        /// </summary>
        internal static string ObjectDisposed_FileClosed {
            get {
                return ResourceManager.GetString("ObjectDisposed_FileClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot access a closed Stream..
        /// </summary>
        internal static string ObjectDisposed_StreamClosed {
            get {
                return ResourceManager.GetString("ObjectDisposed_StreamClosed", resourceCulture);
            }
        }
    }
}
