using System;

namespace OzonEdu.StockApi.Domain.Exceptions.StockItemAggregate
{
    public class NoMerchandiseRequestException : Exception
    {
        public NoMerchandiseRequestException(string message) : base(message)
        {
            
        }
        
        public NoMerchandiseRequestException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}