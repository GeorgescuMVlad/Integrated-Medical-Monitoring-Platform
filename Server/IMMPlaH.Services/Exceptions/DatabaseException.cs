using System;
using System.Collections.Generic;
using System.Text;

namespace IMMPlaH.Services.Exceptions
{
    public class DatabaseException : Exception
    {
        public override string Message { get; }

        public DatabaseException(string message)
        {
            Message = message;
        }
    }
}
