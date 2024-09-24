using AutoMapper;
using Events.Application.Dto;
using Events.Domain;


namespace Events.Application.UseCases;


public class GetParticipantsByEventIdUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, IEnumerable<ParticipantWithoutEventDto>>(unitOfWork, mapper)
{
    public override IEnumerable<ParticipantWithoutEventDto> Execute(int id)
    {
        return _unitOfWork
            .EventRepository
            .GetEventParticipations(id)
            .Select(p => _mapper.Map<ParticipantWithoutEventDto>(p))
            .ToArray();
    }

    public override async Task<IEnumerable<ParticipantWithoutEventDto>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork
            .EventRepository
            .GetEventParticipationsAsync(id, cancellationToken)
            .Select(p => _mapper.Map<ParticipantWithoutEventDto>(p))
            .ToArrayAsync(cancellationToken);
    }
}

