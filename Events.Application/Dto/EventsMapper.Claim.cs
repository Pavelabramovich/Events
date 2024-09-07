using Events.Domain;


namespace Events.Application.Dto;


public partial class EventsMapperProfile : AutoMapper.Profile
{
    private void CreateClaimMap()
    {
        Create_Claim_To_ClaimDto_Map();
    }


    private void Create_Claim_To_ClaimDto_Map()
    {
        CreateMap<Claim, ClaimDto>();
    }
}
