using Evento.Core.DTOs;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class LocalFluent : AbstractValidator<LocalDto>
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