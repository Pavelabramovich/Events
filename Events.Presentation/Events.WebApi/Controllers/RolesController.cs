using AutoMapper;
using Events.Application;
using Events.Domain;
using Events.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Events.Application.UseCases;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Events.Application.Exceptions;


namespace Events.WebApi.Controllers;


[Route("api/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly GetAllRolesUseCase _getAllUseCase;
    private readonly IsRoleExistsUseCase _existsUseCase;
    private readonly CreateRoleUseCase _createUseCase;
    private readonly RemoveRoleUseCase _removeUseCase;


    public RolesController(
        GetAllRolesUseCase getAllUseCase,
        IsRoleExistsUseCase existsUseCase,
        CreateRoleUseCase createUseCase,
        RemoveRoleUseCase removwUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _existsUseCase = existsUseCase;
        _createUseCase = createUseCase;
        _removeUseCase = removwUseCase;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        var roles = await _getAllUseCase.ExecuteAsync();
        return Ok(roles);
    }

    [HttpGet("exists/{name}")]
    public async Task<ActionResult<bool>> Exists(string name)
    {
        var exists = await _existsUseCase.ExecuteAsync(name);
        return Ok(exists);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(string name)
    {       
        await _createUseCase.ExecuteAsync(name);
        return NoContent();
    }


    [HttpDelete("{name}")]
    [Authorize("Admin")]
    public async Task<IActionResult> Delete(string name)
    {
        await _removeUseCase.ExecuteAsync(name);
        return NoContent();
    }
}
