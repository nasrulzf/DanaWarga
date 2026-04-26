using DanaWarga.Application.Features.Residents.Commands.CreateResident;
using FluentValidation;

namespace DanaWarga.Application.Validators;

public sealed class CreateResidentCommandValidator : AbstractValidator<CreateResidentCommand>
{
    public CreateResidentCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(32);
    }
}