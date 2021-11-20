using System;

namespace OzonEdu.MerchandiseService.ApplicationServices.Exceptions
{
    public class NoSourceException : Exception
    {
        public NoSourceException(string message) : base(message)
        {

        }

        public NoSourceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}