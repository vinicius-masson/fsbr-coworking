using Coworking.Domain.Entities;

namespace Coworking.Aplication.Queries.Reservas.GetReserva
{
    public class GetReservaResponse
    {
        public Guid Id { get; set; }
        public DateTime DataInicioReserva { get; set; }
        public DateTime DataFimReserva { get; set; }
        public Guid UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public Guid SalaId { get; set; }
        public string SalaDescricao { get; set; }
        public string Status { get; set; }
    }
}
