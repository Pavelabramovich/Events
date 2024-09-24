using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;


namespace Events.Application.UseCases;


public class GetEventByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, EventWithoutParticipantsDto>(unitOfWork, mapper)
{
    private const string NotFoundErrorMessage = "Not found event with this id.";


    public override EventWithoutParticipantsDto Execute(int id)
    {
        var @event = _unitOfWork.EventRepository.FindById(id)
            ?? throw new EntityNotFoundException(NotFoundErrorMessage);

        return _mapper.Map<EventWithoutParticipantsDto>(@event);
    }

    public override async Task<EventWithoutParticipantsDto> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException(NotFoundErrorMessage);

        return _mapper.Map<EventWithoutParticipantsDto>(@event);
    }
}
