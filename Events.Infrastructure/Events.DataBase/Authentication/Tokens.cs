using Events.Domain.Entities;


namespace Events.WebApi.Authentication;


public class RefreshToken
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string Value { get; init; } = default!;
    public DateTime Expires { get; init; } = default!;
}
