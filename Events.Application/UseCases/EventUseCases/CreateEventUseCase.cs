using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;



public class CreateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<EventCreatingDto>(unitOfWork, mapper)
{
    public override void Execute(EventCreatingDto eventDto)
    {
        var @event = _mapper.Map<Event>(eventDto);

        _unitOfWork.EventRepository.Add(@event);

        if (!_unitOfWork.SaveChanges())
            throw new ValidationException("Internal error");
    }

    public override async Task ExecuteAsync(EventCreatingDto eventDto, CancellationToken cancellationToken = default)
    {
        var @event = _mapper.Map<Event>(eventDto);

        _unitOfWork.EventRepository.Add(@event);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}

