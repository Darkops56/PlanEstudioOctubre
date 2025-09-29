using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class EntradaFluent : AbstractValidator<Entrada>
    {
        public EntradaFluent()
        {
            RuleFor(en => en.tarifa)
            .NotNull().WithMessage("La tarifa es obligatoria");

            RuleFor(en => en.ordenesCompra)
                .NotNull().WithMessage("La orden de compra es obligatoria");
        }
    }
}