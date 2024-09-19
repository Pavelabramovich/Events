using AutoMapper;
using Events.Application.Dto;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static partial class EventUseCases
{
    public class GetById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, EventWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        private const string NotFoundErrorMessage = "Not found event with this id.";


        public override EventWithoutParticipantsDto? Execute(int id)
        {
            var @event = _unitOfWork.EventRepository.FindById(id)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }

        public override async Task<EventWithoutParticipantsDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByIdAsync(id, cancellationToken)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }
    }
}
