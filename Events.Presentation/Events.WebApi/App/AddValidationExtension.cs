using Events.WebApi.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;


namespace Events.WebApi.App;


public static class AddValidationExtension
{
    public static WebApplicationBuilder AddValidation(this WebApplicationBuilder @this)
    {
        @this.Services.AddValidatorsFromAssemblyContaining<UserLoginDtoValidator>();

        return @this;
    }
}
