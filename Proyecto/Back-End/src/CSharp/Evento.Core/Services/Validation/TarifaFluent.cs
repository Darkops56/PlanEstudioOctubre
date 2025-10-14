using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.DTOs;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class TarifaFluent : AbstractValidator<TarifaDto>
    {
        public TarifaFluent()
        {
            RuleFor(t => t.idFuncion)
            .NotNull().WithMessage("La función asociada es obligatoria");

            RuleFor(t => t.Tipo)
                .NotEmpty().WithMessage("El tipo de tarifa es obligatorio");

            RuleFor(t => t.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

            RuleFor(t => t.Stock)
                .GreaterThan((byte)0).WithMessage("El stock debe ser mayor a 0");
        }
    }
}