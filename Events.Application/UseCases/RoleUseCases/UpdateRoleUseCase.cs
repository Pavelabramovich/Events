using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class UpdateRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string, string>(unitOfWork, mapper)
{
    public override void Execute(string oldName, string newName)
    {
        throw new NotImplementedException();
    }

    public override async Task ExecuteAsync(string oldName, string newName, CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
        var role = roles.FirstOrDefault(r => r.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));

        if (role is null)
            throw new EntityNotFoundException($"Role with name {oldName.ToLower()} is not found.");

        _unitOfWork.RoleRepository.Remove(role.Name);
        _unitOfWork.RoleRepository.Add(new Role() { Name = newName });

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}
