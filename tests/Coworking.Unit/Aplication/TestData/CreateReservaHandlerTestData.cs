using Bogus;
using Coworking.Aplication.Commands.Reservas.CreateReserva;

namespace Coworking.Unit.Aplication.TestData
{
    public static class CreateReservaHandlerTestData
    {
        private static readonly Faker<CreateReservaCommand> CreateReservaHandlerFaker = new Faker<CreateReservaCommand>()
            .RuleFor(r => r.DataInicioReserva, f => f.Date.Between(DateTime.Now.AddHours(4), DateTime.Now.AddHours(8)))
            .RuleFor(r => r.DataFimReserva, f => f.Date.Between(DateTime.Now.AddHours(10), DateTime.Now.AddHours(14)))
            .RuleFor(r => r.UsuarioId, f => f.Random.Guid())
            .RuleFor(r => r.SalaId, f => f.Random.Guid());

        public static CreateReservaCommand GenerateValidCommand()
        {
            return CreateReservaHandlerFaker.Generate();
        }
    }
}
