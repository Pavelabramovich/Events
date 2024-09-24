using AutoMapper;
using Events.Application.Exceptions;


namespace Events.Application.UseCases;


public class RemoveUserUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
{
    public override void Execute(int id)
    {
        _unitOfWork.UserRepository.Remove(id);

        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException();
    }

    public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        _unitOfWork.UserRepository.Remove(id);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}
