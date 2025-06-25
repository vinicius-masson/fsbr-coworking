using MediatR;

namespace Coworking.Aplication.Queries.Reservas.GetReserva
{
    public class GetReservaQuery : IRequest<GetReservaResponse>
    {
        public Guid Id { get; set; }
    }
}
