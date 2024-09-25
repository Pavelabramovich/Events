using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;
using System.Threading;


namespace Events.Application.UseCases;


public class RemoveRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
{
    public override void Execute(string name)
    {
        var roles = _unitOfWork.RoleRepository.GetAll();

        var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFoundException(name.ToLower(), $"Role with name {name.ToLower()} is not found.");

        _unitOfWork.RoleRepository.Remove(role.Name);
        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);

        var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFoundException(name.ToLower(), $"Role with name {name.ToLower()} is not found.");

        _unitOfWork.RoleRepository.Remove(role.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
