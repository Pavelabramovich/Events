using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class UpdateEventParticipationUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
{
    public override void Execute(int eventId, int userId)
    {
        var @event = _unitOfWork.EventRepository.FindById(eventId)
            ?? throw new EntityNotFoundException(eventId, $"Could not find event with id = {eventId}");

        if (_unitOfWork.UserRepository.FindById(userId) is null)
            throw new EntityNotFoundException(userId, $"Could not find user with id = {userId}");

        int participantsCount = _unitOfWork.EventRepository.ParticipantsCount(eventId);

        if (@event.MaxPeopleCount <= participantsCount)
            throw new ParticipationsOverflowException(@event.MaxPeopleCount, "All seats for the event are taken");

        _unitOfWork.EventRepository.AddParticipant(eventId, userId);
        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(@eventId, cancellationToken)
            ?? throw new EntityNotFoundException(eventId, $"Could not find event with id = {eventId}");

        if (await _unitOfWork.UserRepository.FindByIdAsync(userId, cancellationToken) is null)
            throw new EntityNotFoundException(userId, $"Could not find user with id = {userId}");

        int participantsCount = await _unitOfWork.EventRepository.ParticipantsCountAsync(eventId, cancellationToken);

        if (@event.MaxPeopleCount <= participantsCount)
            throw new EntityNotFoundException(@event.MaxPeopleCount, "All seats for the event are taken");

        await _unitOfWork.EventRepository.AddParticipantAsync(eventId, userId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

