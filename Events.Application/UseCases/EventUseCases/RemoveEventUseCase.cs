using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;


namespace Events.Application.UseCases;


public class RemoveEventUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
{
    public override void Execute(int id)
    {
        var @event = _unitOfWork.EventRepository.FindById(id)
            ?? throw new EntityNotFoundException(id, $"Event with id = {id} not found.");

        _unitOfWork.EventRepository.Remove(@event.Id);

        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException(id, $"Event with id = {id} not found.");

        _unitOfWork.EventRepository.Remove(@event.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
