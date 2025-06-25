using Coworking.Aplication.Commands.Reservas.CreateReserva;
using Coworking.Domain.Entities;
using Coworking.Infra;
using Coworking.Infra.Repositories;
using Coworking.Infra.Services;
using Coworking.Integration.Common;
using Coworking.Integration.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Integration.Handlers.Reservas
{
    public class CreateReservaHandlerIntegrationTests
    {
        private readonly DbContextOptions<DefaultContext> _dbOptions;
        private readonly FakeEmailService _fakeEmailService;

        public CreateReservaHandlerIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: "TestesReservas")
                .Options;

            _fakeEmailService = new FakeEmailService();
        }

        [Fact(DisplayName = "Given valid reservation When created Then should persist in memory database and send confirmation email")]
        public async Task Handle_MustCreateReservationAndRegisterEmail()
        {
            // Arrange
            using var context = new DefaultContext(_dbOptions);

            var usuario = new Usuario("Teste", "teste@email.com") { Id = Guid.NewGuid() };
            var sala = new Sala("SALA-01", "S01") { Id = Guid.NewGuid() };

            context.Usuarios.Add(usuario);
            context.Salas.Add(sala);
            await context.SaveChangesAsync();

            var command = new CreateReservaCommand
            {
                DataInicioReserva = DateTime.Now.AddHours(1),
                DataFimReserva = DateTime.Now.AddHours(2),
                UsuarioId = usuario.Id,
                SalaId = sala.Id
            };

            var handler = new CreateReservaHandler(new ReservaRepository(context), TestMapperFactory.Create(), new UnitOfWork(context), _fakeEmailService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();

            _fakeEmailService.SentEmails.Should().Contain(e =>
                e.Email == usuario.Email &&
                e.Sala == sala.Codigo);
        }
    }
}
