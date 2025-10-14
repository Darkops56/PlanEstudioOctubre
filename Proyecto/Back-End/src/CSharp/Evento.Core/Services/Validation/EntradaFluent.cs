using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.DTOs;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class EntradaFluent : AbstractValidator<EntradaDto>
    {
        public EntradaFluent()
        {
            RuleFor(en => en.idTarifa)
                .NotNull().WithMessage("La tarifa es obligatoria");

            RuleFor(en => en.idOrdenCompra)
                .NotNull().WithMessage("La orden de compra es obligatoria");
            RuleFor(e => e)
                .NotNull().WithMessage("El estado es obligatorio");
            RuleFor(e => e.PrecioPagado)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
                .NotNull().WithMessage("El precio no puede estar vacio");
        }
    }
}