using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class UpdateEventParticipationUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
{
    public override void Execute(int eventId, int userId)
    {
        var @event = _unitOfWork.EventRepository.FindById(@eventId)
            ?? throw new EntityNotFoundException($"Could not find event with id = {eventId}");

        int participantsCount = _unitOfWork.EventRepository.ParticipantsCount(eventId);

        if (@event.MaxPeopleCount <= participantsCount)
            throw new EntityNotFoundException("All seats for the event are taken");

        _unitOfWork.EventRepository.AddParticipant(eventId, userId);

        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException("Internal error");
    }

    public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(@eventId, cancellationToken)
            ?? throw new EntityNotFoundException($"Could not find event with id = {eventId}");

        int participantsCount = await _unitOfWork.EventRepository.ParticipantsCountAsync(eventId, cancellationToken);

        if (@event.MaxPeopleCount <= participantsCount)
            throw new EntityNotFoundException("All seats for the event are taken");

        await _unitOfWork.EventRepository.AddParticipantAsync(eventId, userId, cancellationToken);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException("Internal error");
    }
}

