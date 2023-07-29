using System;
using Core;

namespace Transaction
{
    public static class Trx
    {
        private static void _PrintFragile(EntityTwo entity)
        {
            Console.Write($"{nameof(Trx)}.{entity.Version}: {entity.Text} from {entity.Member.Content}: ");
        }
        public static void Multiply(EntityTwo entity)
        {
            try                           // can't catch directly;
            {                             // throws at the function entry point
                _PrintFragile(entity);  
            }                             // instead of at the offending line 
            catch (MissingFieldException) // public long Content;
            {
                Console.Write($"{nameof(Trx)}.{entity.Version}: {entity.Text} (MissingFieldException): ");
            }
            catch (MissingMethodException) // public long Content { get; private set; }
            {
                Console.Write($"{nameof(Trx)}.{entity.Version}: {entity.Text} (MissingMethodException): ");
            }
            entity.Member.Multiply();
        }
        
        public static EntityTwo GenerateEntity(string txt) => new(txt, "Trx.Construct");
    }
}