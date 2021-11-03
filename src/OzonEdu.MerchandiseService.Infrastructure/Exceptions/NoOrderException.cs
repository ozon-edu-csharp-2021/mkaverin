using System;

namespace OzonEdu.StockApi.Domain.Exceptions.StockItemAggregate
{
    public class NoOrderException : Exception
    {
        public NoOrderException(string message) : base(message)
        {
            
        }
        
        public NoOrderException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}