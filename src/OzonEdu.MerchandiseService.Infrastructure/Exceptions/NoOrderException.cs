using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate
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