using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class PatientServiceException : Exception
    {
        public string Message { get; set; }

        public PatientServiceException(string message)
        {
            Message = message;
        }
    }
}
