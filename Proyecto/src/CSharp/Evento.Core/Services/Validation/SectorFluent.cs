using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class SectorFluent : AbstractValidator<Sector>
    {
        public SectorFluent()
        {
            RuleFor(s => s.local)
            .NotNull().WithMessage("El local es obligatorio");

            RuleFor(s => s.Capacidad)
                .GreaterThan((byte)0).WithMessage("La capacidad debe ser mayor a 0");
        }
    }
}