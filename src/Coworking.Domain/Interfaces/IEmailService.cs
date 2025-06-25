using Coworking.Domain.Enums;

namespace Coworking.Domain.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailConfirmacaoReservaAsync(string nomeUsuario, string codigoSala, DateTime dataReserva, OperacaoReserva operacao, CancellationToken cancellationToken);
    }
}
