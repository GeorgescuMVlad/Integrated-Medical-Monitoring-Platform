using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class MedicationServiceException : Exception
    {
        public string Message { get; set; }

        public MedicationServiceException(string message)
        {
            Message = message;
        }
    }
}
