using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class EventosFluent : AbstractValidator<Eventos>
    {
        public EventosFluent()
        {
            RuleFor(e => e.Nombre)
            .NotEmpty().WithMessage("El nombre del evento es obligatorio");

            RuleFor(e => e.tipoEvento)
                .NotNull().WithMessage("El tipo de evento es obligatorio");

            RuleFor(e => e.fechaInicio)
                .LessThan(e => e.fechaFin).WithMessage("La fecha de inicio debe ser anterior a la fecha de fin");

            RuleFor(e => e.EstadoEvento)
                .NotEmpty().WithMessage("El estado es obligatorio");
    
        }
    }
}