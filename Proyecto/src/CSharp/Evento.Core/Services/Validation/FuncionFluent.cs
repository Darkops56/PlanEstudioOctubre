using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class FuncionFluent : AbstractValidator<Funcion>
    {
        public FuncionFluent()
        {
            RuleFor(f => f.evento)
            .NotNull().WithMessage("El evento asociado es obligatorio");

            RuleFor(f => f.Fecha)
                .GreaterThan(DateTime.Now).WithMessage("La fecha debe ser en el futuro");
        }
    }
}