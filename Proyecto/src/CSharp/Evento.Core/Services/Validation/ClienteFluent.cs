using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class ClienteFluent : AbstractValidator<Cliente>
    {
        public ClienteFluent()
        {
            RuleFor(c => c.DNI)
            .GreaterThan(0).WithMessage("El DNI debe ser mayor a 0");

        RuleFor(c => c.nombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");

        RuleFor(c => c.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Formato de teléfono inválido");
        }
    }
}