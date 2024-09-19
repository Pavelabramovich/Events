using AutoMapper;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class RemoveRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
{
    public override void Execute(string name)
    {
        throw new NotImplementedException();
    }

    public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
        var role = roles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (role is null)
            throw new ValidationException();

        _unitOfWork.RoleRepository.Remove(role.Name);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}

