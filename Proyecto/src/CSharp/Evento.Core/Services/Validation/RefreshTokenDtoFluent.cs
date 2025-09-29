using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.DTOs;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class RefreshTokenDtoFluent : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoFluent()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("El refresh token es obligatorio");
        }
    }
}