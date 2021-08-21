using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class DuplicateKeyException : Exception
    {
        public override string Message { get; }

        public DuplicateKeyException(string message)
        {
            Message = message;
        }
    }
}
