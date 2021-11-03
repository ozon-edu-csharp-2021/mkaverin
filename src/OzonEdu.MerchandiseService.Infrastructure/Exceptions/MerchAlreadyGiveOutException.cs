using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions
{
    public class MerchAlreadyGiveOutException : Exception
    {
        public MerchAlreadyGiveOutException(string message) : base(message)
        {

        }

        public MerchAlreadyGiveOutException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}