using Events.Domain_Entities;


namespace Events.WebApi.Dto;


public partial class EventsMapperProfile : AutoMapper.Profile
{
    private void CreateParticipantMap()
    {
        Create_Participation_To_ParticipantWithoutEventDto_Map();
        Create_Participation_To_ParticipantWithoutUserDto_Map();
    }

    private void Create_Participation_To_ParticipantWithoutEventDto_Map()
    {
        CreateMap<Participation, ParticipantWithoutEventDto>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId)
            )
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName)
            )
            .ForMember(
                dest => dest.UserSurname,
                opt => opt.MapFrom(src => src.User.Surname)
            );
    }

    private void Create_Participation_To_ParticipantWithoutUserDto_Map()
    {
        CreateMap<Participation, ParticipantWithoutUserDto>()
            .ForMember(
                dest => dest.EventId,
                opt => opt.MapFrom(src => src.EventId)
            )
            .ForMember(
                dest => dest.EventName,
                opt => opt.MapFrom(src => src.Event.Name)
            )
            .ForMember(
                dest => dest.EventDescription,
                opt => opt.MapFrom(src => src.Event.Description)
            );
    }
}
