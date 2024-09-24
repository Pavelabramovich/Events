using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;


namespace Events.Application.UseCases;


public class GetEventByNameUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, EventWithoutParticipantsDto?>(unitOfWork, mapper)
{
    private const string NotFoundErrorMessage = "Not found event with this name.";


    public override EventWithoutParticipantsDto? Execute(string name)
    {
        var @event = _unitOfWork.EventRepository.FindByName(name)
            ?? throw new EntityNotFoundException(NotFoundErrorMessage);

        return _mapper.Map<EventWithoutParticipantsDto>(@event);
    }

    public override async Task<EventWithoutParticipantsDto?> ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByNameAsync(name, cancellationToken)
            ?? throw new EntityNotFoundException(NotFoundErrorMessage);

        return _mapper.Map<EventWithoutParticipantsDto>(@event);
    }
}
