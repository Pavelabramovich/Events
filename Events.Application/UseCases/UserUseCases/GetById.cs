using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public static partial class UserUseCases
{
    public class GetById(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, UserWithoutParticipantsDto?>(unitOfWork, mapper)
    {
        public override UserWithoutParticipantsDto? Execute(int id)
        {
            var user = _unitOfWork.UserRepository.FindById(id);
            return _mapper.Map<UserWithoutParticipantsDto>(user);
        }

        public override async Task<UserWithoutParticipantsDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserRepository.FindByIdAsync(id);
            return _mapper.Map<UserWithoutParticipantsDto>(user);
        }
    }
}
