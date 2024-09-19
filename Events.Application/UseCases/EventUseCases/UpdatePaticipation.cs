using AutoMapper;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static partial class EventUseCases
{
    public class UpdatePaticipation(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int, int>(unitOfWork, mapper)
    {
        public override void Execute(int eventId, int userId)
        {
            var @event = _unitOfWork.EventRepository.FindById(@eventId)
                ?? throw new ValidationException($"Could not find event with id = {eventId}");

            int participantsCount = _unitOfWork.EventRepository.ParticipantsCount(eventId);

            if (@event.MaxPeopleCount <= participantsCount)
                throw new ValidationException("All seats for the event are taken");

            _unitOfWork.EventRepository.AddParticipant(eventId, userId);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(int eventId, int userId, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByIdAsync(@eventId, cancellationToken)
               ?? throw new ValidationException($"Could not find event with id = {eventId}");

            int participantsCount = await _unitOfWork.EventRepository.ParticipantsCountAsync(eventId, cancellationToken);

            if (@event.MaxPeopleCount <= participantsCount)
                throw new ValidationException("All seats for the event are taken");

            await _unitOfWork.EventRepository.AddParticipantAsync(eventId, userId, cancellationToken);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }
}
