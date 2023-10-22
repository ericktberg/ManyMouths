using System.Runtime.InteropServices;

namespace PriceCheck.DB.ORM
{
    public static class GuidInterop
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);

        public static Guid CreateGuid()
        {
            UuidCreateSequential(out Guid guid);
            return guid;
        }
    }
}