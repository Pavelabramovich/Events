using AutoMapper;
using Events.Application.Dto;
using Events.Domain;
using System.ComponentModel.DataAnnotations;


namespace Events.Application.UseCases;


public class UpdateUserUseCase(IUnitOfWork unitOfWork, IMapper mapper) : ActionUseCase<UserWithoutParticipantsDto>(unitOfWork, mapper)
{
    public override void Execute(UserWithoutParticipantsDto userDto)
    {
        throw new NotImplementedException();
    }

    public override async Task ExecuteAsync(UserWithoutParticipantsDto userDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(userDto.Id, cancellationToken)
            ?? throw new ValidationException("User with this id not found");

        var userToReplace = _mapper.Map<UserWithoutParticipantsDto, User>(userDto, user);

        _unitOfWork.UserRepository.Update(userToReplace);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new ValidationException("Internal error");
    }
}
