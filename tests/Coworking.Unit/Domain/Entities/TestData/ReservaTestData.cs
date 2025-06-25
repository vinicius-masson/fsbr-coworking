using Bogus;
using Coworking.Domain.Entities;

namespace Coworking.Unit.Domain.Entities.TestData
{
    public static class ReservaTestData
    {

        private static readonly Faker<Reserva> ReservaFaker = new Faker<Reserva>()
            .UseSeed(123)
            .CustomInstantiator(f =>
            {
                var dataInicio = GerarDataFuturaValida(f);
                var dataFim = dataInicio.AddHours(f.Random.Int(1, 8));

                return new Reserva(
                    dataInicioReserva: dataInicio,
                    dataFimReserva: dataFim,
                    usuarioId: f.Random.Guid(),
                    salaId: f.Random.Guid()
                );
            });

        public static Reserva GenerateValidReservation()
        {
            return ReservaFaker.Generate();
        }

        public static Reserva GenerateInValidReservationForCancel()
        {
            var reserva = ReservaFaker.Generate();
            var data = DateTime.Now.AddHours(new Random().Next(1, 23));

            reserva.SetDataInicioReserva(data);
            return reserva;
        }

        private static DateTime GerarDataFuturaValida(Faker f)
        {
            return f.Date.Between(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(30));
        }
    }
}
