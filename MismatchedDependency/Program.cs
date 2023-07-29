using Core;
using Transaction;

namespace MismatchedDependency
{
    public static class Program
    {
        private static int Main()
        {
            var x = new EntityTwo("From Console", "Direct.Construct");
            Trx.Multiply(x);
            Util.Multiply(x);
        
            var y = Trx.GenerateEntity("From Transaction");
            Trx.Multiply(y);
            Util.Multiply(y);
            //y.Boom(13);
            return 0;
        }
    }
}

