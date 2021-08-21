using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class ConsumerServiceException : Exception
    {
        public string Message { get; set; }

        public ConsumerServiceException(string message)
        {
            Message = message;
        }
    }
}
