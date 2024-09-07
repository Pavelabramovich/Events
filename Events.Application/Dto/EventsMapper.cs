
namespace Events.Application.Dto;


public partial class EventsMapperProfile : AutoMapper.Profile
{
    public EventsMapperProfile()
    {
        CreateEventMap();
        CreateUserMap();
        CreateParticipantMap();
        CreateClaimMap();
        CreateExternalLoginMap();
    }
}
