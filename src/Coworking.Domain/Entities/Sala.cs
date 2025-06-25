using Coworking.Domain.Common;

namespace Coworking.Domain.Entities
{
    public class Sala : BaseEntity
    {
        public Sala(string descricao, string codigo)
        {
            Descricao = descricao;
            Codigo = codigo;
        }

        public string Descricao { get; private set; } = string.Empty;
        public string? Andar { get; private set; }
        public string? Bloco { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public List<Reserva> Reservas { get; private set; } = new();
    }
}
