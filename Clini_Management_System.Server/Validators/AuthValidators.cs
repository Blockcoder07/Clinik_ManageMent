using Clini_Management_System.Server.Models.DTOs;
using FluentValidation;

namespace Clini_Management_System.Server.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.ClinicName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
