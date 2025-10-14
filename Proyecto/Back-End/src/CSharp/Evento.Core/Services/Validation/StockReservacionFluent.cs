using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.DTOs;
using FluentValidation;

namespace Evento.Core.Services.Validation
{
    public class StockReservacionFluent : AbstractValidator<StockReservacionesDto>
    {
        public StockReservacionFluent()
        {
            RuleFor(sr => sr.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad deber ser mayor a 0")
                .NotEmpty().WithMessage("No debe estar vacio");
            RuleFor(sr => sr.idTarifa)
                .NotEmpty().WithMessage("La asociacion con Tarifa no debe estar vacia");
            RuleFor(sr => sr.idOrdenCompra)
                .NotEmpty().WithMessage("La asociacion con ordenCompra no debe estar vacia.");
        }
    }
}