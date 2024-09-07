using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Application.Exceptions;


public class CreatingException : Exception
{
    public CreatingException()
        : base()
    { }

    public CreatingException(string message)
        : base(message)
    { }

    public CreatingException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
