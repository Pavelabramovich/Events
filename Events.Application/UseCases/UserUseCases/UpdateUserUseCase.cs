using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;
using Events.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Diagnostics;


namespace Events.Application.UseCases;


public class UpdateUserUseCase : ActionUseCase<UserWithoutParticipantsDto>
{
    private readonly IValidator<UserWithoutParticipantsDto> _validator;


    public UpdateUserUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<UserWithoutParticipantsDto> validator)
        : base(unitOfWork, mapper)
    {
        _validator = validator;
    }



    public override void Execute(UserWithoutParticipantsDto userDto)
    {
        if (_validator.Validate(userDto) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        var user = _unitOfWork.UserRepository.FindById(userDto.Id)
            ?? throw new EntityNotFoundException("User with this id not found");

        User userToReplace = _mapper.Map(userDto, user);

        _unitOfWork.UserRepository.Update(userToReplace);

        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException();
    }

    public override async Task ExecuteAsync(UserWithoutParticipantsDto userDto, CancellationToken cancellationToken = default)
    {
        if (await _validator.ValidateAsync(userDto, cancellationToken) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        var user = await _unitOfWork.UserRepository.FindByIdAsync(userDto.Id, cancellationToken)
            ?? throw new EntityNotFoundException("User with this id not found");

        var userToReplace = _mapper.Map(userDto, user);

        _unitOfWork.UserRepository.Update(userToReplace);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}
