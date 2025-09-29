using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class TipoEventoFluent : AbstractValidator<TipoEvento>
    {
        public TipoEventoFluent()
        {
            RuleFor(te => te.tipoEvento)
            .IsInEnum().WithMessage("Tipo de evento inválido");
        }
    }
}