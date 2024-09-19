using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public static partial class EventUseCases
{
    public class GetAllWithParticipants(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<EventWithParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<EventWithParticipantsDto> Execute()
        {
            return _unitOfWork
                .EventRepository
                .GetAllWithParticipations()
                .Select(e => _mapper.Map<EventWithParticipantsDto>(e))
                .ToArray();
        }

        public override async Task<IEnumerable<EventWithParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .EventRepository
                .GetAllWithParticipationsAsync(cancellationToken)
                .Select(e => _mapper.Map<EventWithParticipantsDto>(e))
                .ToArrayAsync(cancellationToken);
        }
    }
}
