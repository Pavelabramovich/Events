using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class RemoveEventParticipationUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
{
    public override void Execute(int eventId, int userId)
    {
        try
        {
            _unitOfWork.EventRepository.RemoveParticipant(eventId, userId);
        }
        catch (ArgumentException exception)
        {
            throw new EntityNotFoundException(exception.Message, exception);
        }
        
        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException();
    }

    public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.EventRepository.RemoveParticipantAsync(eventId, userId, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            throw new EntityNotFoundException(exception.Message, exception);
        }

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}

