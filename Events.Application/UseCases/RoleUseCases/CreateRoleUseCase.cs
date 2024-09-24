using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class CreateRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
{
    public override void Execute(string name)
    {
        _unitOfWork.RoleRepository.Add(new Role { Name = name });

        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException();
    }

    public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        _unitOfWork.RoleRepository.Add(new Role { Name = name });

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}
