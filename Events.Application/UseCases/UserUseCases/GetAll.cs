﻿using AutoMapper;
using Events.Application.Dto;


namespace Events.Application.UseCases;


public static partial class UserUseCases
{
    public class GetAll(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<IEnumerable<UserWithoutParticipantsDto>>(unitOfWork, mapper)
    {
        public override IEnumerable<UserWithoutParticipantsDto> Execute()
        {
            return _unitOfWork
                .UserRepository
                .GetAll()
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArray();
        }

        public override async Task<IEnumerable<UserWithoutParticipantsDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork
                .UserRepository
                .GetAllAsync()
                .Select(u => _mapper.Map<UserWithoutParticipantsDto>(u))
                .ToArrayAsync(cancellationToken);
        }
    }
}
