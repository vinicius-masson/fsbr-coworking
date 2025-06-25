using MediatR;

namespace Coworking.Aplication.Commands.Reservas.CancelReserva
{
    public class CancelReservaCommand : IRequest<CancelReservaResponse>
    {
        public Guid Id { get; set; }
    }
}
