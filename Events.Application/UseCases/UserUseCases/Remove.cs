using AutoMapper;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static partial class UserUseCases
{
    public class Remove(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<int>(unitOfWork, mapper)
    {
        public override void Execute(int id)
        {
            _unitOfWork.UserRepository.Remove(id);

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            _unitOfWork.UserRepository.Remove(id);

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }
}
