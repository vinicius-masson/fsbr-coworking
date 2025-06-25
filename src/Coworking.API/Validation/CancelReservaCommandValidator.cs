using Coworking.Aplication.Commands.Reservas.CancelReserva;
using FluentValidation;

namespace Coworking.API.Validation
{
    public class CancelReservaCommandValidator : AbstractValidator<CancelReservaCommand>
    {
        public CancelReservaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Id da Reserva é obrigatório.");
        }
    }
}
