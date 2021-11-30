using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate
{
    public class MerchTypeException : Exception
    {
        public MerchTypeException(string message) : base(message)
        {

        }

        public MerchTypeException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
