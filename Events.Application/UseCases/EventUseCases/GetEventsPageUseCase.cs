using AutoMapper;
using Events.Application.Dto;
using Events.Domain;


namespace Events.Application.UseCases;


public class GetEventsPageUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, int, IEnumerable<EventWithoutParticipantsDto>>(unitOfWork, mapper)
{
    public override IEnumerable<EventWithoutParticipantsDto> Execute(int pageNum, int pageSize)
    {
        return _unitOfWork
            .EventRepository
            .PageAll(skip: pageNum * pageSize, take: pageSize)
            .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
            .ToArray();
    }

    public override async Task<IEnumerable<EventWithoutParticipantsDto>> ExecuteAsync(int pageNum, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork
            .EventRepository
            .PageAllAsync(skip: pageNum * pageSize, take: pageSize, cancellationToken)
            .Select(e => _mapper.Map<EventWithoutParticipantsDto>(e))
            .ToArrayAsync(cancellationToken);
    }
}

