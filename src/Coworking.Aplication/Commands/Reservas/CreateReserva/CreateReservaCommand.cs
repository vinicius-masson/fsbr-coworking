using MediatR;

namespace Coworking.Aplication.Commands.Reservas.CreateReserva
{
    public class CreateReservaCommand : IRequest<CreateReservaResponse>
    {
        public DateTime DataInicioReserva { get; set; }
        public DateTime DataFimReserva { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid SalaId { get; set; }
    }
}
