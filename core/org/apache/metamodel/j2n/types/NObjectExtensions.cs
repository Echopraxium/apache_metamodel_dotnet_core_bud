
using System.Runtime.CompilerServices;

namespace org.apache.metamodel.j2n.types
{
    public static class NObjectExtensions
    {
        public static int GetHashCode(this object o)
        {
            int hash_code;
            hash_code = RuntimeHelpers.GetHashCode(o);
            //int hash_code = (int)DateTime.Now.Ticks;
            //GCHandle handle = GCHandle.Alloc(o, GCHandleType.Pinned);
            //try
            //{
            //    IntPtr pointer = GCHandle.ToIntPtr(handle);
            //    hash_code = unchecked((int)pointer);
            //}
            //finally
            //{
            //    handle.Free();
            //}
            return hash_code;
        } // GetHashCode()
    } // NObjectExtensions class
} // org.apache.metamodel.j2n.types namespace
