﻿using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;


namespace Events.Application.UseCases;


public class GetUserByLoginUseCase(IUnitOfWork unitOfWork, IMapper mapper) : FuncUseCase<string, UserWithoutParticipantsDto?>(unitOfWork, mapper)
{
    public override UserWithoutParticipantsDto? Execute(string login)
    {
        var user = _unitOfWork.UserRepository.FindByLogin(login)
             ?? throw new EntityNotFoundException(login, $"User with login = {login} is not found");

        return _mapper.Map<UserWithoutParticipantsDto>(user);
    }

    public override async Task<UserWithoutParticipantsDto?> ExecuteAsync(string login, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.FindByLoginAsync(login)
            ?? throw new EntityNotFoundException(login, $"User with login = {login} is not found");

        return _mapper.Map<UserWithoutParticipantsDto>(user);
    }
}
