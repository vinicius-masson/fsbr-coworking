using MediatR;

namespace Coworking.Aplication.Commands.Reservas.UpdateReserva
{
    public class UpdateReservaCommand : IRequest<UpdateReservaResponse>
    {
        public Guid Id { get; set; }
        public DateTime DataInicioReserva { get; set; }
        public DateTime DataFimReserva { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid SalaId { get; set; }
    }
}
