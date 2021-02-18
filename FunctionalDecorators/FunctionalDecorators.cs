using System;

namespace FunctionalDecorators
{
    public static class FunctionalDecorators
    {
        public static Func<T> Retry<T>(int retryCount, Func<T> methodDelegate)
        {
            return () =>
            {
                var attemptsLeft = retryCount;

                T entity = default;

                while(attemptsLeft > 0)
                {
                    try
                    {
                        entity = methodDelegate();
                        return entity;
                    }
                    catch
                    {
                        if(--attemptsLeft == 0)
                            throw;
                    }
                }

                return entity;
            };
        }

        public static Func<T> BoundryLog<T>(string methodName, Func<T> methodDelegate)
        {
            return () =>
            {
                Console.WriteLine($"Method {methodName} has started");
                var result = methodDelegate();
                Console.WriteLine($"Method {methodName} has finished");
                return result;
            };
        }
    }
}