using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class RemoveEventParticipationUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
{
    public override void Execute(int eventId, int userId)
    {
        if (_unitOfWork.EventRepository.FindById(eventId) is null)
            throw new EntityNotFoundException(eventId, $"Could not find event with id = {eventId}");

        if (_unitOfWork.UserRepository.FindById(userId) is null)
            throw new EntityNotFoundException(eventId, $"Could not find user with id = {userId}");

        _unitOfWork.EventRepository.RemoveParticipant(eventId, userId);
        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.EventRepository.FindByIdAsync(eventId, cancellationToken) is null)
            throw new EntityNotFoundException(eventId, $"Could not find event with id = {eventId}");

        if (await _unitOfWork.UserRepository.FindByIdAsync(userId, cancellationToken) is null)
            throw new EntityNotFoundException(eventId, $"Could not find user with id = {userId}");

        await _unitOfWork.EventRepository.RemoveParticipantAsync(eventId, userId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

