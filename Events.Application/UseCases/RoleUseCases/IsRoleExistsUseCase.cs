using AutoMapper;


namespace Events.Application.UseCases;


public class IsRoleExistsUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, bool>(unitOfWork, mapper)
{
    public override bool Execute(string name)
    {
        var roles = _unitOfWork.RoleRepository.GetAll();
        return roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public override async Task<bool> ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
        return roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
