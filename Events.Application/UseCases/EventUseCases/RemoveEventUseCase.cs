using AutoMapper;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class RemoveEventUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
{
    public override void Execute(int id)
    {
        var @event = _unitOfWork.EventRepository.FindById(id)
            ?? throw new ValidationException("Event with this id not found");

        _unitOfWork.EventRepository.Remove(@event.Id);

        if (!_unitOfWork.SaveChanges())
            throw new ValidationException("Internal error");
    }

    public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(id, cancellationToken)
            ?? throw new ValidationException("Event with this id not found");

        _unitOfWork.EventRepository.Remove(@event.Id);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}
