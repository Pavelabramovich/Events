﻿
namespace Events.Application.Dto;


public record UserLoginDto
{
    public required string Login { get; init; }
    public required string HashedPassword { get; init; }
}
