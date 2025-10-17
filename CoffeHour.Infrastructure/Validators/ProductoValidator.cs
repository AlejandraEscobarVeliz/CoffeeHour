using CoffeHour.Core.Entities;
using FluentValidation;

namespace CoffeHour.Infrastructure.Validators
{
    public class ProductoValidator : AbstractValidator<Productos>
    {
        public ProductoValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(p => p.Categoria)
                .NotEmpty().WithMessage("La categoría es obligatoria.");

            RuleFor(p => p.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que 0.");

            RuleFor(p => p.Estado)
                .NotEmpty().WithMessage("El estado del producto es obligatorio.")
                .Must(e => e == "Activo" || e == "Inactivo")
                .WithMessage("El estado debe ser 'Activo' o 'Inactivo'.");
        }
    }
}

