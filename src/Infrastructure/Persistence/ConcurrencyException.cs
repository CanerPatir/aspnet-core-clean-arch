using System;

namespace Infrastructure.Persistence
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException()
        {
        }
        
        public ConcurrencyException(string message) : base(message)
        {
            
        }
    }
}