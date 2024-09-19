using AutoMapper;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class RemoveEventParticipationUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
{
    public override void Execute(int eventId, int userId)
    {
        _unitOfWork.EventRepository.RemoveParticipant(eventId, userId);

        if (!_unitOfWork.SaveChanges())
            throw new ValidationException("Internal error");
    }

    public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.EventRepository.RemoveParticipantAsync(eventId, userId, cancellationToken);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}

