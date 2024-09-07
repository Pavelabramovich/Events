using Events.Domain;


namespace Events.WebApi.Authentication;


public class RefreshToken : IEntity<int>
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string Value { get; init; } = default!;
    public DateTime Expires { get; init; } = default!;

    int IEntity<int>.Id => UserId;
    object IEntity.Id => UserId;
}
