using AutoMapper;
using Events.Domain;
using Events.Domain.Entities;
using Events.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace Events.WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public RolesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetEvents()
    {
        var roles = await _unitOfWork.RoleRepository.GetAll().ToAsyncEnumerable().ToArrayAsync();

        return Ok(roles.Select(r => r.Name));
    }


    [HttpGet("exists/{name}")]
    public async Task<ActionResult<bool>> Exists(string name)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync().ToArrayAsync();

        return Ok(roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) is not null);
    }


    [HttpPost]
    public async Task<IActionResult> Post(string name)
    {
        _unitOfWork.RoleRepository.Add(new Role { Name = name });

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }

    [HttpPut("rename/{name}")]
    public async Task<IActionResult> Put(string name, string newName)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync().ToArrayAsync();
        var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (role is null)
            return NotFound();

        _unitOfWork.RoleRepository.Remove(role.Name);
        _unitOfWork.RoleRepository.Add(new Role() { Name = newName });

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }


    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync().ToArrayAsync();
        var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (role is null)
            return NotFound();

        _unitOfWork.RoleRepository.Remove(role.Name);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }
}
