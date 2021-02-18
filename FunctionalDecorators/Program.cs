using System;

namespace FunctionalDecorators
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int> method = MethodToWrapp;
            method = FunctionalDecorators.Retry(3, method);
            method = FunctionalDecorators.BoundryLog(nameof(MethodToWrapp), method);

            method();
        }

        static int MethodToWrapp()
        {
            for(var i = 0; i < 3; i++)
            {
                Console.WriteLine("Doing some work");
            }

            return 1;
        }
    }
}
