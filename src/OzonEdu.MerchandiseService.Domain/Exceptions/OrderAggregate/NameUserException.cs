using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate
{

    public class NameUserException : Exception
    {
        public NameUserException(string message) : base(message)
        {

        }

        public NameUserException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
