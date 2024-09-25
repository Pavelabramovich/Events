using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;


namespace Events.Application.UseCases;


public class GetParticipationsByUserIdUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, IEnumerable<ParticipantWithoutUserDto>>(unitOfWork, mapper)
{
    public override IEnumerable<ParticipantWithoutUserDto> Execute(int userId)
    {
        if (_unitOfWork.UserRepository.FindById(userId) is null)
            throw new EntityNotFoundException(userId, $"User with id = {userId} is not found.");

        return _unitOfWork
            .UserRepository
            .GetUserEvents(userId)
            .Select(p => _mapper.Map<ParticipantWithoutUserDto>(p))
            .ToArray();
    }

    public override async Task<IEnumerable<ParticipantWithoutUserDto>> ExecuteAsync(int userId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.UserRepository.FindByIdAsync(userId, cancellationToken) is null)
            throw new EntityNotFoundException(userId, $"User with id = {userId} is not found.");

        var participants = await _unitOfWork.UserRepository.GetUserEventsAsync(userId, cancellationToken).ToArrayAsync(cancellationToken);

        return participants.Select(p => _mapper.Map<ParticipantWithoutUserDto>(p));
    }
}
