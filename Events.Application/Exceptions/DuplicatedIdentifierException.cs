
using System.Collections;

namespace Events.Application.Exceptions;


public class DuplicatedIdentifierException : Exception
{
    public object DuplicatedExistentIdentifier { get; init; }


    public DuplicatedIdentifierException(object nonExistentId)
        : base()
    {
        DuplicatedExistentIdentifier = nonExistentId;
        Data[nameof(DuplicatedExistentIdentifier)] = DuplicatedExistentIdentifier;
    }

    public DuplicatedIdentifierException(object nonExistentId, string message)
        : base(message)
    {
        DuplicatedExistentIdentifier = nonExistentId;
        Data[nameof(DuplicatedExistentIdentifier)] = DuplicatedExistentIdentifier;
    }

    public DuplicatedIdentifierException(object nonExistentId, string message, Exception innerException)
        : base(message, innerException)
    {
        DuplicatedExistentIdentifier = nonExistentId;
        Data[nameof(DuplicatedExistentIdentifier)] = DuplicatedExistentIdentifier;
    }
}
