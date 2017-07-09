using System;
using System.Runtime.InteropServices;

namespace org.apache.metamodel.j2cs.types
{
    public static class CsObjectExtensions
    {
        public static int GetHashCode(this object o)
        {
            int hash_code = (int)DateTime.Now.Ticks;
            GCHandle handle = GCHandle.Alloc(o, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = GCHandle.ToIntPtr(handle);
                hash_code = unchecked((int)pointer);
            }
            finally
            {
                handle.Free();
            }
            return hash_code;
        } // GetHashCode()
    } // CsObjectExtensions class
} // org.apache.metamodel.j2cs.types namespace
