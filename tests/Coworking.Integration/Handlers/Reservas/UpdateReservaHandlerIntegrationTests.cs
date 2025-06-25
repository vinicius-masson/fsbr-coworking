using Coworking.Domain.Entities;
using Coworking.Infra.Repositories;
using Coworking.Infra.Services;
using Coworking.Infra;
using Coworking.Integration.Common;
using Coworking.Integration.Services;
using Microsoft.EntityFrameworkCore;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using FluentAssertions;

namespace Coworking.Integration.Handlers.Reservas
{
    public class UpdateReservaHandlerIntegrationTests
    {
        private readonly DbContextOptions<DefaultContext> _dbOptions;
        private readonly FakeEmailService _fakeEmailService;

        public UpdateReservaHandlerIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: "TestesReservas")
                .Options;
            _fakeEmailService = new FakeEmailService();
        }

        [Fact(DisplayName = "Given valid reservation When updated Then should persist changes in memory database and send confirmation email")]
        public async Task Handle_MustUpdateReservationAndRegisterEmail()
        {
            // Arrange
            using var context = new DefaultContext(_dbOptions);

            var usuario = new Usuario("Teste", "teste@email.com") { Id = Guid.NewGuid() };
            var sala = new Sala("SALA-01", "S01") { Id = Guid.NewGuid() };
            var reservaId = Guid.NewGuid();

            var reservaExistente = new Reserva(
                    DateTime.Now.AddHours(3),
                    DateTime.Now.AddHours(5),
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

            var command = new UpdateReservaCommand
            {
                Id = reservaId,
                DataInicioReserva = DateTime.Now.AddHours(1),
                DataFimReserva = DateTime.Now.AddHours(2),
                UsuarioId = usuario.Id,
                SalaId = sala.Id
            };            

            // Act
            var handler = new UpdateReservaHandler(new ReservaRepository(context), TestMapperFactory.Create(), new UnitOfWork(context), _fakeEmailService);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();

            context.ChangeTracker.Clear();

            var reservaAtualizada = await context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Sala)
                .FirstOrDefaultAsync(r => r.Id == reservaId);

            reservaAtualizada.Should().NotBeNull();
            reservaAtualizada.DataInicioReserva.Should().Be(command.DataInicioReserva);
            reservaAtualizada.DataFimReserva.Should().Be(command.DataFimReserva);
            reservaAtualizada.Usuario.Email.Should().Be(usuario.Email);
            reservaAtualizada.Sala.Codigo.Should().Be(sala.Codigo);

            _fakeEmailService.SentEmails.Should().Contain(e =>
                e.Email == usuario.Email &&
                e.Sala == sala.Codigo);
        }
    }
}
