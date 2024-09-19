using AutoMapper;
using Events.Application.Dto;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static partial class EventUseCases
{
    public class GetByName(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, EventWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        private const string NotFoundErrorMessage = "Not found event with this name.";


        public override EventWithoutParticipantsDto? Execute(string name)
        {
            var @event = _unitOfWork.EventRepository.FindByName(name)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }

        public override async Task<EventWithoutParticipantsDto?> ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.EventRepository.FindByNameAsync(name, cancellationToken)
                ?? throw new ValidationException(NotFoundErrorMessage);

            return _mapper.Map<EventWithoutParticipantsDto>(@event);
        }
    }
}
