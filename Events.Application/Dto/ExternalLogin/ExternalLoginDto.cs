using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Application.Dto;


public record ExternalLoginDto
{
    public required int UserId { get; init; }
    public required string Provider { get; init; } 
    public required string ProviderKey { get; init; }
}
