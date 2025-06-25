using Coworking.Aplication.Commands.Reservas.CancelReserva;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Coworking.Infra;
using Coworking.Infra.Repositories;
using Coworking.Infra.Services;
using Coworking.Integration.Common;
using Coworking.Integration.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Integration.Handlers.Reservas
{
    public class CancelReservaHandlerIntegrationTests
    {
        private readonly DbContextOptions<DefaultContext> _dbOptions;
        private readonly FakeEmailService _fakeEmailService;

        public CancelReservaHandlerIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: "TestesReservas")
                .Options;
            _fakeEmailService = new FakeEmailService();
        }

        [Fact(DisplayName = "Given valid reservation When canceled Then should change status of reservation to canceled in memory database and send confirmation email")]
        public async Task Handle_MustCancelReservationAndRegisterEmail()
        {
            // Arrange
            using var context = new DefaultContext(_dbOptions);

            var usuario = new Usuario("Teste", "teste@email.com") { Id = Guid.NewGuid() };
            var sala = new Sala("SALA-01", "S01") { Id = Guid.NewGuid() };
            var reservaId = Guid.NewGuid();

            var reservaExistente = new Reserva(
                    DateTime.Now.AddHours(42),
                    DateTime.Now.AddHours(46),
                    usuario.Id,
                    sala.Id
            );

            reservaExistente.Id = reservaId;
            reservaExistente.SetTestUsuario(usuario);
            reservaExistente.SetTestSala(sala);

            context.Usuarios.Add(usuario);
            context.Salas.Add(sala);
            context.Reservas.Add(reservaExistente);
            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var command = new CancelReservaCommand { Id = reservaId };

            // Act
            var handler = new CancelReservaHandler(new ReservaRepository(context), new UnitOfWork(context), _fakeEmailService);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();

            context.ChangeTracker.Clear();

            var reservaAtualizada = await context.Reservas
                .FirstOrDefaultAsync(r => r.Id == reservaId);

            reservaAtualizada.Should().NotBeNull();
            reservaAtualizada.Status.Should().Be(StatusReserva.Cancelada);

            _fakeEmailService.SentEmails.Should().Contain(e =>
                e.Email == usuario.Email &&
                e.Sala == sala.Codigo);
        }
    }
}
