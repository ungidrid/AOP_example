using System;

namespace Core.Exceptions
{
    public class InvalidInterceptorException: Exception
    {
        public InvalidInterceptorException(string message) :base(message)
        {
        }
    }
}