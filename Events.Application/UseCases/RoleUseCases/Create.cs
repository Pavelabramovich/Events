using AutoMapper;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public static partial class RoleUseCases
{
    public class Create(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<string>(unitOfWork, mapper)
    {
        public override void Execute(string name)
        {
            _unitOfWork.RoleRepository.Add(new Role { Name = name });

            if (!_unitOfWork.SaveChanges())
                throw new ValidationException("Internal error");
        }

        public override async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            _unitOfWork.RoleRepository.Add(new Role { Name = name });

            if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
                throw new ValidationException("Internal error");
        }
    }
}
