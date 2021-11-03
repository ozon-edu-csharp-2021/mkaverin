using System;

namespace OzonEdu.MerchandiseService.ApplicationServices.Exceptions
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