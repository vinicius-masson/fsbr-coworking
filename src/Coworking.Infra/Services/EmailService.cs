using Coworking.Domain.Interfaces;
using System.Net.Mail;
using System.Net;
using Coworking.Domain.Configuration;
using Microsoft.Extensions.Options;
using Coworking.Domain.Enums;

namespace Coworking.Infra.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task EnviarEmailConfirmacaoReservaAsync(string nomeUsuario, string codigoSala, DateTime dataReserva, OperacaoReserva operacao, CancellationToken cancellationToken)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.EnableSsl = _emailSettings.EnableSsl;
                client.Credentials = new NetworkCredential(
                    _emailSettings.SenderEmail,
                    _emailSettings.SenderPassword
                );

                using var mail = new MailMessage(
                        from: _emailSettings.SenderEmail,
                        to: _emailSettings.DestinationEmail,
                        subject: $"Confirmação de Reserva - {codigoSala}",
                        body: $"Olá {nomeUsuario}, sua reserva para {dataReserva:dd/MM/yyyy 'às' HH:mm} foi {operacao.ToString().ToLower()}!"
                      );

                await client.SendMailAsync(mail, cancellationToken);
            }
        }
    }
}
