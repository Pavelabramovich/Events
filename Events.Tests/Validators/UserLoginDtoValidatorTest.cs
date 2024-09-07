using Events.Application.Dto;
using Events.WebApi.Validators;
using FluentAssertions.Execution;
using FluentValidation.TestHelper;


namespace Events.Tests.Validators;


public class Test_UserLoginDtoValidator
{
    [Fact]
    public void Validation_error_on_empty_login_and_password()
    {
        /// Arrange
        var validator = new UserLoginDtoValidator();
        var userLoginDto = new UserLoginDto() { HashedPassword = "", Login = "" };


        /// Act
        var v = validator.TestValidate(userLoginDto);
        

        /// Assert
        using (new AssertionScope())
        {
            v.ShouldHaveValidationErrorFor(model => model.Login);
            v.ShouldHaveValidationErrorFor(model => model.HashedPassword);
        }    
    }
}
