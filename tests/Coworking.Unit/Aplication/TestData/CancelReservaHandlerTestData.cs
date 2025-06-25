using Bogus;
using Coworking.Aplication.Commands.Reservas.CancelReserva;

namespace Coworking.Unit.Aplication.TestData
{
    public static class CancelReservaHandlerTestData
    {
        private static readonly Faker<CancelReservaCommand> CancelReservaHandlerFaker = new Faker<CancelReservaCommand>()
            .RuleFor(r => r.Id, f => f.Random.Guid());

        public static CancelReservaCommand GenerateValidCommand()
        {
            return CancelReservaHandlerFaker.Generate();
        }
    }
}
