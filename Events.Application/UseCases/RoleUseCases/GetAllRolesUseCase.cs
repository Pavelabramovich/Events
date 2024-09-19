using AutoMapper;


namespace Events.Application.UseCases;


public class GetAllRolesUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<string>>(unitOfWork, mapper)
{
    public override IEnumerable<string> Execute()
    {
        var roles = _unitOfWork.RoleRepository.GetAll();
        return roles.Select(r => r.Name).ToArray();
    }

    public override async Task<IEnumerable<string>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken).ToArrayAsync(cancellationToken);
        return roles.Select(r => r.Name).ToArray();
    }
}
