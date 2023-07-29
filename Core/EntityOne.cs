#define NEW

using System;
using System.Runtime.InteropServices;
#if NEW
using EntityOneType = System.Int64;
#else
using EntityOneType = System.Int16;
#endif

namespace Core
{
    internal static class Hardcode
    {
#if NEW
        internal const int EntityOneBufferSize = 9;
        internal const float EntityTwoVersion = 1.2f;
        internal const short EntityTwoMultiplier = 3;
#else
        internal const int EntityOneBufferSize = 10;
        internal const float EntityTwoVersion = 1.1f;
        internal const short EntityTwoMultiplier = 2;
#endif
        internal const int EntityTwoBufferSize = 32;
    }
    
    public class EntityOne
    {
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=Hardcode.EntityOneBufferSize)]
        private readonly char[] _text;

        public string Text => string.Concat(_text);
        
        public EntityOneType Content { get; private set; }
        private readonly EntityOneType _multiplier;
        
        internal EntityOne(string text, EntityOneType multiplier)
        {
            Content = 1;
            _multiplier = multiplier;
            
            var len = Math.Min(Hardcode.EntityOneBufferSize-2, text.Length);
            _text = text[..len].ToCharArray();
        }

        public void Multiply()
        {
            var oldValue = Content;
            Content *= _multiplier;
            Console.WriteLine($"multiply {Text} from {oldValue} to {Content}");
        }
    }
}