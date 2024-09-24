using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Application.Exceptions;


public class DataSavingException : Exception
{
    public DataSavingException()
        : base()
    { }

    public DataSavingException(string message)
        : base(message)
    { }

    public DataSavingException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
