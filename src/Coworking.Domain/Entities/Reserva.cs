using Coworking.Domain.Common;
using Coworking.Domain.Enums;
using Coworking.Domain.Exceptions;

namespace Coworking.Domain.Entities
{
    public class Reserva : BaseEntity
    {
        public Reserva(DateTime dataInicioReserva, DateTime dataFimReserva, Guid usuarioId, Guid salaId)
        {
            if (dataInicioReserva >= dataFimReserva)
                throw new DomainException("Data início deve ser menor que data fim");

            DataInicioReserva = dataInicioReserva;
            DataFimReserva = dataFimReserva;
            UsuarioId = usuarioId;
            SalaId = salaId;
            Status = StatusReserva.Confirmada;
        }

        public DateTime DataInicioReserva { get; private set; }
        public DateTime DataFimReserva { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
        public Guid SalaId { get; private set; }
        public Sala Sala { get; private set; }
        public StatusReserva Status { get; private set; }

        public void SetDataInicioReserva(DateTime dataInicioReserva)
        {
            if (dataInicioReserva >= this.DataFimReserva)
                throw new DomainException("Data início deve ser menor que data fim");

            DataInicioReserva = dataInicioReserva;
        }

        public void SetDataFimReserva(DateTime dataFimReserva)
        {
            if (dataFimReserva <= this.DataInicioReserva)
                throw new DomainException("Data início deve ser menor que data fim");

            DataFimReserva = dataFimReserva;
        }

        public void Cancelar()
        {
            if ((DataInicioReserva - DateTime.Now).TotalHours < 24)
                throw new DomainException("Esta Reserva não pode ser cancelada pois falta menos de 24 horas para o seu início.");

            Status = StatusReserva.Cancelada;
        }

        public void SetTestUsuario(Usuario usuario) => Usuario = usuario;
        public void SetTestSala(Sala sala) => Sala = sala;
    }
}
