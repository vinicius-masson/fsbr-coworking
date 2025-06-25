using Coworking.Domain.Common;
using Coworking.Domain.Enums;

namespace Coworking.Domain.Entities
{
    public class Usuario : BaseEntity
    {

        public Usuario(string nome, string email)
        {
            Nome = nome;
            Email = email;
            Status = StatusUsuario.Ativo;
        }

        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? Telefone { get; private set; }
        public StatusUsuario Status { get; private set; }
        public List<Reserva> Reservas { get; private set; } = new();
    }
}
