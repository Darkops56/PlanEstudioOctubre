
using Evento.Core.DTOs;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class FuncionFluent : AbstractValidator<FuncionDto>
    {
        public FuncionFluent()
        {
            RuleFor(f => f.idEvento)
                .NotNull().WithMessage("El evento asociado es obligatorio");
            RuleFor(f => f.Fecha)
                .GreaterThan(DateTime.Now).WithMessage("La fecha debe ser mayor a la actual");
            RuleFor(f => f.Nombre)
                .NotEmpty().WithMessage("El nombre de la funcion no puede estar vacio");
            RuleFor(f => f.Estado)
                .NotNull().WithMessage("El estado es obligatorio");
            }
    }
}