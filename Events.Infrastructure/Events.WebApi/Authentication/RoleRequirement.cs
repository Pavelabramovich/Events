using Microsoft.AspNetCore.Authorization;


namespace Events.WebApi.Authentication;


public record RoleRequirement(string Role) : IAuthorizationRequirement;
