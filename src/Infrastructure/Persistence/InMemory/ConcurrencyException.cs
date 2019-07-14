using System;

namespace Infrastructure.Persistence.InMemory
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