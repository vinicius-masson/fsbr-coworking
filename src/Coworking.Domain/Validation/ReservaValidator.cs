using Coworking.Domain.Entities;
using FluentValidation;

namespace Coworking.Domain.Validation
{
    public class ReservaValidator : AbstractValidator<Reserva>
    {
        public ReservaValidator()
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

        private bool VerificarDataInicioMenorQueFim(Reserva reserva)
        {
            return reserva.DataInicioReserva < reserva.DataFimReserva;
        }
    }
}
