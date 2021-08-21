using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class PlanServiceException : Exception
    {
        public string Message { get; set; }

        public PlanServiceException(string message)
        {
            Message = message;
        }
    }
}
