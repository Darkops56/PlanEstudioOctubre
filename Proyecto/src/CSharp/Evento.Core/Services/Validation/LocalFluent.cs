using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class LocalFluent : AbstractValidator<Local>
    {
        public LocalFluent()
        {
            RuleFor(l => l.Nombre)
            .NotEmpty().WithMessage("El nombre del local es obligatorio");

            RuleFor(l => l.Ubicacion)
                .NotEmpty().WithMessage("La ubicación es obligatoria");

        }
    }
}