using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.DTOs;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class TipoEventoFluent : AbstractValidator<TipoEventoDto>
    {
        public TipoEventoFluent()
        {
            RuleFor(te => te.tipoEvento)
                .IsInEnum().WithMessage("Tipo de evento inválido");
            RuleFor(te => te.tipoEvento)
                .NotEmpty().WithMessage("El tipo de evento no puede estar vacio.");
        }
    }
}