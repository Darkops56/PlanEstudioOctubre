using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class UsuarioFluent : AbstractValidator<Usuario>
    {
        public UsuarioFluent()
        {
            RuleFor(u => u.Apodo)
            .NotEmpty().WithMessage("El apodo es obligatorio");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("Formato de email inválido");

            RuleFor(u => u.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("El rol es obligatorio")
                .Must(Role => UniqueFormatStrings.NormalizarString(Role.ToString()) == UniqueFormatStrings.NormalizarString(ERoles.Admin.ToString()) || UniqueFormatStrings.NormalizarString(Role.ToString()) == UniqueFormatStrings.NormalizarString(ERoles.Usuario.ToString()));
        }
    }
}