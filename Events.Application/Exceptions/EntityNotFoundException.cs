
using System.Collections;

namespace Events.Application.Exceptions;


public class EntityNotFoundException : Exception
{
    public object NonExistentIdentifier { get; init; }


    public EntityNotFoundException(object nonExistentId)
        : base()
    {
        NonExistentIdentifier = nonExistentId;
        Data[nameof(NonExistentIdentifier)] = NonExistentIdentifier;
    }

    public EntityNotFoundException(object nonExistentId, string message)
        : base(message)
    {
        NonExistentIdentifier = nonExistentId;
        Data[nameof(NonExistentIdentifier)] = NonExistentIdentifier;
    }

    public EntityNotFoundException(object nonExistentId, string message, Exception innerException)
        : base(message, innerException)
    {
        NonExistentIdentifier = nonExistentId;
        Data[nameof(NonExistentIdentifier)] = NonExistentIdentifier;
    }
}
