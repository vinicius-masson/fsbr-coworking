using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using Coworking.Domain.Entities;
using FluentValidation;

namespace Coworking.API.Validation
{
    public class UpdateReservaCommandValidator : AbstractValidator<UpdateReservaCommand>
    {
        public UpdateReservaCommandValidator()
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

        private bool VerificarDataInicioMenorQueFim(UpdateReservaCommand reserva)
        {
            return reserva.DataInicioReserva < reserva.DataFimReserva;
        }
    }
}
