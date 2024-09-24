
namespace Events.Application.Dto;


public class TokensDto
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
}
