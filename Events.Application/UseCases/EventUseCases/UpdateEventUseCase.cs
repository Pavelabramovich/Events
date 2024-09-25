using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;
using Events.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;


namespace Events.Application.UseCases;


public class UpdateEventUseCase : ActionUseCase<EventWithoutParticipantsDto>
{
    private readonly IValidator<EventWithoutParticipantsDto> _validator;    


    public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventWithoutParticipantsDto> validator) 
        : base(unitOfWork, mapper) 
    {
        _validator = validator;
    }


    public override void Execute(EventWithoutParticipantsDto eventDto)
    {
        if (_validator.Validate(eventDto) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        var @event = _unitOfWork.EventRepository.FindById(eventDto.Id)
            ?? throw new EntityNotFoundException(eventDto.Id, $"Event with id = {eventDto.Id} not found");

        Event eventToReplace = _mapper.Map(eventDto, @event);

        _unitOfWork.EventRepository.Update(eventToReplace);
        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(EventWithoutParticipantsDto eventDto, CancellationToken cancellationToken = default)
    {
        if (await _validator.ValidateAsync(eventDto, cancellationToken) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        var @event = await _unitOfWork.EventRepository.FindByIdAsync(eventDto.Id, cancellationToken)
            ?? throw new EntityNotFoundException(eventDto.Id, $"Event with id = {eventDto.Id} not found");

        Event eventToReplace = _mapper.Map(eventDto, @event);

        _unitOfWork.EventRepository.Update(eventToReplace);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
