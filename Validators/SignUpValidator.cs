using FluentValidation;
using week2_Task.Models.DTOS;
using week2_Task.Data;
using Microsoft.EntityFrameworkCore;

public class SignupValidator : AbstractValidator<SignupDTO>
{
    private readonly ApplicationDBContext _db;

    public SignupValidator(ApplicationDBContext db)
    {
        _db = db;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MustAsync(BeUniqueEmail).WithMessage("Email is already registered.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => role == "Admin" || role == "User")
            .WithMessage("Role must be either 'Admin' or 'User'.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !await _db.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}

