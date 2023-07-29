using System;

namespace Core
{
    public static class Util
    {
        public static void Multiply(EntityTwo entity)
        {
            Console.Write($"{nameof(Util)}.{entity.Version}: {entity.Text} from {entity.Member.Content}: ");
            entity.Member.Multiply();
        }
    }
}