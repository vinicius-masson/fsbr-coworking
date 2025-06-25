using Coworking.Domain.Enums;
using Coworking.Domain.Interfaces;

namespace Coworking.Integration.Services
{
    public class FakeEmailService : IEmailService
    {
        public List<(string Email, string Sala, DateTime Data, OperacaoReserva operacao)> SentEmails { get; } = new();

        public Task EnviarEmailConfirmacaoReservaAsync(string email, string codigoSala, DateTime dataReserva, OperacaoReserva operacao, CancellationToken cancellationToken)
        {
            SentEmails.Add((email, codigoSala, dataReserva, operacao));
            return Task.CompletedTask;
        }
    }
}
