using AutoMapper;
using Events.Application;
using Events.Domain;
using Events.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Events.Application.UseCases;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


namespace Events.WebApi.Controllers;


[Route("api/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly GetAllRolesUseCase _getAllUseCase;
    private readonly IsRoleExistsUseCase _existsUseCase;
    private readonly CreateRoleUseCase _createUseCase;
    private readonly UpdateRoleUseCase _updateUseCase;
    private readonly RemoveRoleUseCase _removeUseCase;


    public RolesController(
        GetAllRolesUseCase getAllUseCase,
        IsRoleExistsUseCase existsUseCase,
        CreateRoleUseCase createUseCase,
        UpdateRoleUseCase updateUseCase,
        RemoveRoleUseCase removwUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _existsUseCase = existsUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
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
        try
        {
            await _createUseCase.ExecuteAsync(name);
            return NoContent();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("rename/{name}")]
    [Authorize]
    public async Task<IActionResult> Put(string name, string newName)
    {
        try
        {
            await _updateUseCase.ExecuteAsync(name, newName);
            return NoContent();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }


    [HttpDelete("{name}")]
    [Authorize("Admin")]
    public async Task<IActionResult> Delete(string name)
    {
        try
        {
            await _removeUseCase.ExecuteAsync(name);
            return NoContent();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
