using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Application.Dto;


public record ClaimDto
{
    public required int UserId { get; init; }
    public required string Type { get; init; }
    public required string Value { get; init; }
}
