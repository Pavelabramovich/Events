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
    private readonly RoleUseCases.GetAll _getAllUseCase;
    private readonly RoleUseCases.Exists _existsUseCase;
    private readonly RoleUseCases.Create _createUseCase;
    private readonly RoleUseCases.Update _updateUseCase;
    private readonly RoleUseCases.Remove _removeUseCase;


    public RolesController(
        RoleUseCases.GetAll getAllUseCase,
        RoleUseCases.Exists existsUseCase,
        RoleUseCases.Create createUseCase,
        RoleUseCases.Update updateUseCase,
        RoleUseCases.Remove removwUseCase)
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
