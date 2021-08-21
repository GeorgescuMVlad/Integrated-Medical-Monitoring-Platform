using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class UserServiceException : Exception
    {
        public int StatusCode { get; set; }
        public UserServiceException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public UserServiceException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public UserServiceException(string message) : base(message)
        {

        }

        public UserServiceException(string message, Exception innerException, int statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
