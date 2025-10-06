using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Evento.Core.Entidades;
using Evento.Core.Services.Utility;
using Evento.Core.Services.Enums;
using Evento.Core.DTOs;
using System.Security.Cryptography.X509Certificates;

namespace Evento.Core.Services.Validation
{
    public class OrdenesCompraFluent : AbstractValidator<OrdenesCompraDto>
    {
        public OrdenesCompraFluent()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("El usuario asociado a la orden es obligatorio")
                .EmailAddress().WithMessage("El formato es invalido o el correo es incorrecto");

            RuleFor(x => x.Fecha)
                .NotEmpty()
                .WithMessage("La fecha de la orden es obligatoria")
                .Must(fecha => fecha > DateTime.MinValue)
                .WithMessage("La fecha de la orden no es válida");

            RuleFor(x => x.Total)
                .GreaterThan(0)
                .WithMessage("El total de la orden debe ser mayor a cero");

            RuleFor(x => x.metodoPago)
                .NotEmpty()
                .WithMessage("El método de pago no es válido");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("El estado de la orden es obligatorio")
                .Must(Estado => UniqueFormatStrings.NormalizarString(Estado.ToString()) == UniqueFormatStrings.NormalizarString(EEstados.Creado.ToString()))
                .WithMessage("El estado de la orden debe ser 'Creado'");
        }
    }
}