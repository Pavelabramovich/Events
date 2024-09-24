using AutoMapper;
using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Diagnostics;


namespace Events.Application.UseCases;


public class CreateUserUseCase : ActionUseCase<UserCreatingDto>
{
    private readonly IValidator<UserCreatingDto> _validator;


    public CreateUserUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<UserCreatingDto> validator)
        : base(unitOfWork, mapper)
    {
        _validator = validator;
    }


    public override void Execute(UserCreatingDto userDto)
    {
        if (_validator.Validate(userDto) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        User user = _mapper.Map<User>(userDto);

        _unitOfWork.UserRepository.Add(user);

        if (!_unitOfWork.SaveChanges())
            throw new DataSavingException();
    }

    public override async Task ExecuteAsync(UserCreatingDto userDto, CancellationToken cancellationToken = default)
    {
        if (_validator.Validate(userDto) is ValidationResult { IsValid: false } result)
        {
            var error = result.Errors.First();
            throw new ValidationException(error.ErrorMessage);
        }

        User user = _mapper.Map<User>(userDto);

        _unitOfWork.UserRepository.Add(user);

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
            throw new DataSavingException();
    }
}
