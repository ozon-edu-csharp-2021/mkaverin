using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions.MerchandiseRequestAggregate
{

    public class MerchandiseRequestStatusException : Exception
    {
        public MerchandiseRequestStatusException(string message) : base(message)
        {

        }

        public MerchandiseRequestStatusException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
