using Bogus;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;

namespace Coworking.Unit.Aplication.TestData
{
    public class UpdateReservaHandlerTestData
    {
        private static readonly Faker<UpdateReservaCommand> UpdateReservaHandlerFaker = new Faker<UpdateReservaCommand>()
            .RuleFor(r => r.Id, f => f.Random.Guid())
            .RuleFor(r => r.DataInicioReserva, f => f.Date.Between(DateTime.Now.AddHours(4), DateTime.Now.AddHours(8)))
            .RuleFor(r => r.DataFimReserva, f => f.Date.Between(DateTime.Now.AddHours(10), DateTime.Now.AddHours(14)))
            .RuleFor(r => r.UsuarioId, f => f.Random.Guid())
            .RuleFor(r => r.SalaId, f => f.Random.Guid());

        public static UpdateReservaCommand GenerateValidCommand()
        {
            return UpdateReservaHandlerFaker.Generate();
        }
    }
}
