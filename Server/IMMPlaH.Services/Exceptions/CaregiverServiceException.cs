using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class CaregiverServiceException : Exception
    {
        public string Message { get; set; }

        public CaregiverServiceException(string message)
        {
            Message = message;
        }
    }
}
