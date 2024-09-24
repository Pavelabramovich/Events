using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public class GetUserByLoginUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, UserWithoutParticipantsDto?>(unitOfWork, mapper)
{
    public override UserWithoutParticipantsDto? Execute(string login)
    {
        var user = _unitOfWork.UserRepository.FindByLogin(login);

        if (user is null)
            return null;

        return _mapper.Map<UserWithoutParticipantsDto>(user);
    }

    public override async Task<UserWithoutParticipantsDto?> ExecuteAsync(string login, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.FindByLoginAsync(login);

        if (user is null)
            return null;

        return _mapper.Map<UserWithoutParticipantsDto>(user);
    }
}
