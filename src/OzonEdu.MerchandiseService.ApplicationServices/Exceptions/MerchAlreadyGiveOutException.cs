using System;

namespace OzonEdu.MerchandiseService.ApplicationServices.Exceptions
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