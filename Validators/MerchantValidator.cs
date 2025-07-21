/*using FluentValidation;
using week2_Task.Models.Entities;

public class MerchantValidator : AbstractValidator<Merchant>
{
    public MerchantValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
} */