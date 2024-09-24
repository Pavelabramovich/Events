using Events.Domain.Entities;


namespace Events.Application.Dto;


public partial class EventsMapperProfile : AutoMapper.Profile
{
    private void CreateExternalLoginMap()
    {
        Create_ExternalLogin_To_ExternalLoginDto_Map();
    }


    private void Create_ExternalLogin_To_ExternalLoginDto_Map()
    {
        CreateMap<ExternalLogin, ExternalLoginDto>();
    }
}