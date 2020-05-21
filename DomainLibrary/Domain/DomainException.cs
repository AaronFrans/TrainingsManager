using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLibrary.Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
