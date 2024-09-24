using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain.Entities;
using Events.Domain;
using FluentValidation;
using FluentValidation.Results;


namespace Events.Application.UseCases;



public class CreateEventUseCase : ActionUseCase<EventCreatingDto>
{
    private readonly IValidator<EventCreatingDto> _validator;


    public CreateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventCreatingDto> validator)
        : base(unitOfWork, mapper)
    {
        _validator = validator;
    }


    public override void Execute(EventCreatingDto eventDto)
    {
        if (_validator.Validate(eventDto) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        var @event = _mapper.Map<Event>(eventDto);

        _unitOfWork.EventRepository.Add(@event);

        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException();
    }

    public override async Task ExecuteAsync(EventCreatingDto eventDto, CancellationToken cancellationToken = default)
    {
        if (await _validator.ValidateAsync(eventDto, cancellationToken) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        var @event = _mapper.Map<Event>(eventDto);

        _unitOfWork.EventRepository.Add(@event);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}
