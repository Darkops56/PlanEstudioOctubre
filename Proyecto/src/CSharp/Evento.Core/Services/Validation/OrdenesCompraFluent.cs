using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;

namespace Evento.Core.Services.Validation
{
    public class OrdenesCompraFluent : AbstractValidator<OrdenesCompra>
    {
        public OrdenesCompraFluent()
        {
            RuleFor(x => x.usuario)
                .NotNull()
                .WithMessage("El usuario asociado a la orden es obligatorio");

            RuleFor(x => x.Fecha)
                .NotEmpty()
                .WithMessage("La fecha de la orden es obligatoria")
                .Must(fecha => fecha > DateTime.MinValue)
                .WithMessage("La fecha de la orden no es válida");

            RuleFor(x => x.Total)
                .GreaterThan(0)
                .WithMessage("El total de la orden debe ser mayor a cero");

            RuleFor(x => x.metodoPago)
                .IsInEnum()
                .WithMessage("El método de pago no es válido");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("El estado de la orden es obligatorio")
                .Must(Estado => Estado == EEstados.Creado || Estado == EEstados.Pagado || Estado == EEstados.Cancelado)
                .WithMessage("El estado de la orden debe ser 'Creada', 'Pagada' o 'Cancelada'");
        }
    }
}