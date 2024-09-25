using AutoMapper;
using Events.Application.Exceptions;
using Events.Domain;


namespace Events.Application.UseCases;


public class RemoveUserUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
{
    public override void Execute(int id)
    {
        if (_unitOfWork.UserRepository.FindById(id) is null)
           throw new EntityNotFoundException(id, $"User with id = {id} not found");

        _unitOfWork.UserRepository.Remove(id);
        _unitOfWork.SaveChanges();
    }

    public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.UserRepository.FindByIdAsync(id, cancellationToken) is null)
           throw new EntityNotFoundException(id, $"User with id = {id} not found");

        _unitOfWork.UserRepository.Remove(id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
