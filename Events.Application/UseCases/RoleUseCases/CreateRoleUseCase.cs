using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using Events.Domain.Entities;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class CreateRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
{
    public override void Execute(string name)
    {
        if (_unitOfWork.RoleRepository.GetWhere(r => r.Name == name).Any())
            throw new DuplicatedIdentifierException(name, $"Role with name = {name} already exists.");

        _unitOfWork.RoleRepository.Add(new Role { Name = name });
        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.RoleRepository.GetWhereAsync(r => r.Name == name, cancellationToken).AnyAsync(cancellationToken))
            throw new DuplicatedIdentifierException(name, $"Role with name = {name} already exists.");

        _unitOfWork.RoleRepository.Add(new Role { Name = name });
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
