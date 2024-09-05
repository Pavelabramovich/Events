using Events.Entities;
using System.ComponentModel.DataAnnotations;


namespace Events.WebApi.Authentication;


public class RefreshToken
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string Value { get; init; } = default!;
    public DateTime Expires { get; init; } = default!;
}

public class Tokens
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
