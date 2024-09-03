using System.ComponentModel.DataAnnotations;


namespace Events.WebApi.Authentication;


public class UserRefreshTokens
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    public string RefreshToken { get; set; } = default!;

    public bool IsActive { get; set; } = true;
}

public class Tokens
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
