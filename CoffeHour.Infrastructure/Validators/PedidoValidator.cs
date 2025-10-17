using CoffeHour.Core.Entities;
using FluentValidation;

namespace CoffeHour.Infrastructure.Validators
{
    public class PedidoValidator : AbstractValidator<Pedidos>
    {
        public PedidoValidator()
        {
            RuleFor(p => p.IdCliente)
                .GreaterThan(0).WithMessage("Debe seleccionar un cliente válido.");

            RuleFor(p => p.Total)
                .GreaterThan(0).WithMessage("El total del pedido debe ser mayor a 0.");

            RuleFor(p => p.Estado)
                .NotEmpty().WithMessage("El estado del pedido es obligatorio.")
                .Must(e => e == "Pendiente" || e == "Pagado" || e == "Cancelado")
                .WithMessage("El estado debe ser Pendiente, Pagado o Cancelado.");
        }
    }
}


