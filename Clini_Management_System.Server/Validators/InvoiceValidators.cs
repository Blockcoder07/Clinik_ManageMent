using Clini_Management_System.Server.Models.DTOs;
using FluentValidation;

namespace Clini_Management_System.Server.Validators;

public class InvoiceCreateValidator : AbstractValidator<InvoiceCreateDto>
{
    public InvoiceCreateValidator()
    {
        RuleFor(x => x.AppointmentId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
