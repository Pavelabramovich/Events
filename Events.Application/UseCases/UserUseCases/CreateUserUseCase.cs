using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class CreateUserUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<UserCreatingDto>(unitOfWork, mapper)
{
    public override void Execute(UserCreatingDto userDto)
    {
        User user = _mapper.Map<User>(userDto);

        _unitOfWork.UserRepository.Add(user);

        if (!_unitOfWork.SaveChanges())
            throw new ValidationException("Internal error");
    }

    public override async Task ExecuteAsync(UserCreatingDto userDto, CancellationToken cancellationToken = default)
    {
        User user = _mapper.Map<User>(userDto);

        _unitOfWork.UserRepository.Add(user);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}
