using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public static partial class UserUseCases
{
    public class GetParticipantsById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, IEnumerable<ParticipantWithoutUserDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<ParticipantWithoutUserDto> Execute(int id)
        {
            return _unitOfWork
                .UserRepository
                .GetUserEvents(id)
                .Select(p => _mapper.Map<ParticipantWithoutUserDto>(p))
                .ToArray();
        }

        public override async Task<IEnumerable<ParticipantWithoutUserDto>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var participants = await _unitOfWork.UserRepository.GetUserEventsAsync(id, cancellationToken).ToArrayAsync(cancellationToken);

            return participants.Select(p => _mapper.Map<ParticipantWithoutUserDto>(p));
        }
    }
}
