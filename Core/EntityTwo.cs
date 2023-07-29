using System;
using System.Runtime.InteropServices;

namespace Core
{
    public class EntityTwo
    {
        public readonly double Version;
        public readonly EntityOne Member;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=Hardcode.EntityTwoBufferSize)]
        private readonly char[] _text;
        public string Text => string.Concat(_text);
        
        public EntityTwo(string text, string member, short multiplier = Hardcode.EntityTwoMultiplier)
        {
            Version = Hardcode.EntityTwoVersion;
            Member = new EntityOne(member, multiplier);
            var len = Math.Min(Hardcode.EntityTwoBufferSize-2, text.Length);
            _text = text[..len].ToCharArray();
        }

        public bool Boom(int len)
        {
            var raw = new int[2 * len];
            var handle = GCHandle.Alloc(raw, GCHandleType.Pinned);
            try
            {
                var pointer = handle.AddrOfPinnedObject();
                var buffer = new byte[len];

                // Unhandled exception. System.ArgumentOutOfRangeException: Specified argument was out of the range of valid values.
                // https://stackoverflow.com/questions/5486938/c-sharp-how-to-get-byte-from-intptr
                Marshal.Copy( pointer, buffer, 0, 3*len );
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }

            return true;
        }
    }
}