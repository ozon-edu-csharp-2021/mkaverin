using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate
{

    public class OrderStatusException : Exception
    {
        public OrderStatusException(string message) : base(message)
        {

        }

        public OrderStatusException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
