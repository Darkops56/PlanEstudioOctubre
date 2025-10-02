using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.DTOs;
using Evento.Core.Services.Repo;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class RegisterDtoFluent : AbstractValidator<RegisterDto>
    {
        private readonly IRepoUsuario _repoUsuario;
        public RegisterDtoFluent(IRepoUsuario repoUsuario)
        {
            _repoUsuario = repoUsuario;
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("Formato de email inválido");

            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

            RuleFor(x => x.Apodo)
                .NotEmpty().WithMessage("El apodo es obligatorio")
                .MaximumLength(45).WithMessage("El apodo no puede superar los 45 caracteres");

            RuleFor(x => x.DNI)
                .NotEmpty().WithMessage("El DNI es obligatorio")
                .GreaterThan(0).WithMessage("El DNI debe ser mayor a 0");
        }
    }
}