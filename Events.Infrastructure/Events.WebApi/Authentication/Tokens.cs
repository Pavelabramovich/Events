
namespace Events.WebApi.Authentication;


public class Tokens
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
}
