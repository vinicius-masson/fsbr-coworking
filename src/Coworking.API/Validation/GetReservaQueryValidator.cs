using Coworking.Aplication.Queries.Reservas.GetReserva;
using FluentValidation;

namespace Coworking.API.Validation
{
    public class GetReservaQueryValidator : AbstractValidator<GetReservaQuery>
    {
        public GetReservaQueryValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty()
                .WithMessage("Reserva Id é obrigatório");
        }
    }
}
