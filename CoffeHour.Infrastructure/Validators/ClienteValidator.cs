using CoffeHour.Core.DTOs;
using CoffeHour.Core.Entities;
using CoffeHour.Infrastructure.DTOs;
using FluentValidation;

namespace CoffeHour.Infrastructure.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDTO>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio.");

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("El formato del correo no es válido.");

            RuleFor(x => x.Telefono)
                .Matches(@"^[0-9]+$").WithMessage("El teléfono solo puede contener números.");
        }
    }
}



