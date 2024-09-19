using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<EventWithoutParticipantsDto>(unitOfWork, mapper)
{
    public override void Execute(EventWithoutParticipantsDto eventDto)
    {
        var @event = _unitOfWork.EventRepository.FindById(eventDto.Id)
            ?? throw new ValidationException("Event with this id not found");

        Event eventToReplace = _mapper.Map(eventDto, @event);

        _unitOfWork.EventRepository.Update(eventToReplace);

        if (!_unitOfWork.SaveChanges())
            throw new ValidationException("Internal error");
    }

    public override async Task ExecuteAsync(EventWithoutParticipantsDto eventDto, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(eventDto.Id, cancellationToken)
            ?? throw new ValidationException("Event with this id not found");

        Event eventToReplace = _mapper.Map(eventDto, @event);

        _unitOfWork.EventRepository.Update(eventToReplace);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}
