using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace org.apache.metamodel.j2n.data.numbers
{
    // https://msdn.microsoft.com/en-us/library/cc953fe1.aspx
    // bool, char, unsigned char, signed char, __int8	         1 byte
    // __int16, short, unsigned short, wchar_t, __wchar_t	     2 bytes
    // float, __int32, int, unsigned int, long, unsigned long	 4 bytes
    // double, __int64, long double, long long	                 8 bytes
    //[StructLayout(LayoutKind.Explicit)]
    public struct NNumberValue
    {
        /// <summary>The union's value as a short variable</summary>
        /*[FieldOffset(0)]*/ public bool Bool;

        /// <summary>The union's value as a short variable</summary>
        /*[FieldOffset(1)]*/
        public short Short;

        /// <summary>The union's value as a int variable</summary>
        /*[FieldOffset(3)]*/
        public int Int;

        /// <summary>The union's value as a long</summary>
        /*[FieldOffset(7)]*/
        public long Long;

        /// <summary>The union's value as an unsigned long</summary>
        /*[FieldOffset(11)]*/
        public ulong ULong;

        /// <summary>The union's value as a double precision floating point variable</summary>
        /*[FieldOffset(15)]*/
        public double Double;

        /// <summary>The union's value as a BigInteger</summary>
        /*[FieldOffset(23)]*/
        public NInteger BigInt;
    } // NNumberValue struct
}
