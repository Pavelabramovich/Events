using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public static partial class UserUseCases
{
    public class GetPage(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<int, int, IEnumerable<UserWithoutParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<UserWithoutParticipantsDto> Execute(int pageNum, int pageSize)
        {
            return _unitOfWork
                .UserRepository
                .PageAll(skip: pageNum * pageSize, take: pageSize)
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArray();
        }

        public override async Task<IEnumerable<UserWithoutParticipantsDto>> ExecuteAsync(int pageNum, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .UserRepository
                .PageAllAsync(skip: pageNum * pageSize, take: pageSize, cancellationToken)
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArrayAsync(cancellationToken);
        }
    }
}
