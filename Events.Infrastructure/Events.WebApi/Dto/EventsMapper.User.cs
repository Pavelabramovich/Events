using Events.Domain.Entities;


namespace Events.WebApi.Dto;


public partial class EventsMapperProfile : AutoMapper.Profile
{
    private void CreateUserMap()
    {
        Create_UserCreationDto_To_User_Map();

        Create_User_To_UserWithParticipantsDto_Map();
        Create_User_To_UserWithoutParticipantsDto_Map();
    }


    private void Create_UserCreationDto_To_User_Map()
    {
        CreateMap<UserCreatingDto, User>();
    }

    private void Create_User_To_UserWithoutParticipantsDto_Map()
    {
        CreateMap<User, UserWithoutParticipantsDto>().ReverseMap();
    }

    private void Create_User_To_UserWithParticipantsDto_Map()
    {
        CreateMap<User, UserWithParticipantsDto>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Participants))
            .ReverseMap();
    }
}
