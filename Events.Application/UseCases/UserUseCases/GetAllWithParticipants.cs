using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public static partial class UserUseCases
{
    public class GetAllWithParticipants(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<UserWithParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<UserWithParticipantsDto> Execute()
        {
            return _unitOfWork
               .UserRepository
               .GetAllWithParticipations()
               .Select(u => _mapper.Map<UserWithParticipantsDto>(u))
               .ToArray();
        }

        public override async Task<IEnumerable<UserWithParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .UserRepository
                .GetAllWithParticipationsAsync()
                .Select(u => _mapper.Map<UserWithParticipantsDto>(u))
                .ToArrayAsync();
        }
    }
}
