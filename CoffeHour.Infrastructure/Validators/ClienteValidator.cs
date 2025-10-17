using CoffeHour.Core.Entities;
using FluentValidation;

namespace CoffeHour.Infrastructure.Validators
{
    public class ClienteValidator : AbstractValidator<Clientes>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo no es válido.");

            RuleFor(c => c.Telefono)
                .Matches(@"^\d+$").WithMessage("El teléfono solo puede contener números.")
                .When(c => !string.IsNullOrEmpty(c.Telefono));
        }
    }
}


