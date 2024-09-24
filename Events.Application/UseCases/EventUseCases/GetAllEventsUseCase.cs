using AutoMapper;
using Events.Domain;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public class GetAllEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<EventWithoutParticipantsDto>>(unitOfWork, mapper)
{
    public override IEnumerable<EventWithoutParticipantsDto> Execute()
    {
        return _unitOfWork
            .EventRepository
            .GetAll()
            .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
            .ToArray();
    }

    public override async Task<IEnumerable<EventWithoutParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork
            .EventRepository
            .GetAllAsync(cancellationToken)
            .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
            .ToArrayAsync(cancellationToken);
    }
}
