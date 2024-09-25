using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Application.Exceptions;


public class ParticipationsOverflowException : Exception
{
    public int EventMaxParticipanstCount { get; init; }


    public ParticipationsOverflowException(int eventMaxParticipanstCount)
        : base()
    {
        EventMaxParticipanstCount = eventMaxParticipanstCount;
        Data[nameof(EventMaxParticipanstCount)] = EventMaxParticipanstCount;
    }

    public ParticipationsOverflowException(int eventMaxParticipanstCount, string message)
        : base(message)
    {
        EventMaxParticipanstCount = eventMaxParticipanstCount;
        Data[nameof(EventMaxParticipanstCount)] = EventMaxParticipanstCount;
    }

    public ParticipationsOverflowException(int eventMaxParticipanstCount, string message, Exception innerException)
        : base(message, innerException)
    {
        EventMaxParticipanstCount = eventMaxParticipanstCount;
        Data[nameof(EventMaxParticipanstCount)] = EventMaxParticipanstCount;
    }
}
