using Coworking.Aplication.Commands.Reservas.CreateReserva;
using FluentValidation;

namespace Coworking.API.Validation
{
    public class CreateReservaCommandValidator : AbstractValidator<CreateReservaCommand>
    {
        public CreateReservaCommandValidator()
        {
            RuleFor(r => r.DataInicioReserva)
                .NotEmpty()
                .WithMessage("Data Início Reserva é obrigatório.")
                .Must((reserva, datainicio) => VerificarDataInicioMenorQueFim(reserva))
                .WithMessage("Data Início Reserva deve ser menor do que a Data Fim Reserva.");

            RuleFor(r => r.DataFimReserva)
                .NotEmpty()
                .WithMessage("Data Fim Reserva é obrigatório.");

            RuleFor(r => r.SalaId)
                .NotEmpty()
                .WithMessage("Sala é obrigatório.");

            RuleFor(r => r.UsuarioId)
                .NotEmpty()
                .WithMessage("Usuario é obrigatório.");
        }

        private bool VerificarDataInicioMenorQueFim(CreateReservaCommand reserva) => reserva.DataInicioReserva < reserva.DataFimReserva;
    }
}
