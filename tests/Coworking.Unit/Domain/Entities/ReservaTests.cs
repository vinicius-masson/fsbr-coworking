using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Coworking.Domain.Exceptions;
using Coworking.Unit.Domain.Entities.TestData;

namespace Coworking.Unit.Domain.Entities
{
    public class ReservaTests
    {
        [Fact(DisplayName = "Status should change to Cancelled when cancelled")]
        public void Given_ValidReservationDate_When_CancelReservation_Then_ReservationCanceled()
        {
            //Arrange
            var reserva = ReservaTestData.GenerateValidReservation();

            //Act
            reserva.Cancelar();

            //Assert
            Assert.Equal(StatusReserva.Cancelada, reserva.Status);
        }

        [Fact(DisplayName = "Throws domain exception if cancel reservation is not possible")]
        public void Given_InValidCancelReservationDate_When_CancelReservation_Then_ShouldThrowDomainException()
        {
            //Arrange
            var reserva = ReservaTestData.GenerateInValidReservationForCancel();

            //Act
            var exception = Assert.Throws<DomainException>(() => reserva.Cancelar());

            //Assert
            Assert.Equal("Esta Reserva não pode ser cancelada pois falta menos de 24 horas para o seu início.", exception.Message);
        }

        [Fact(DisplayName = "Throws domain exception if begin date reservation is greater than final date reservation")]
        public void Given_InValidBeginReservationDate_When_SetDataInicioReserva_Then_ShouldThrowDomainException()
        {
            var reserva = ReservaTestData.GenerateValidReservation();

            var exception = Assert.Throws<DomainException>(() =>
                reserva.SetDataInicioReserva(reserva.DataFimReserva.AddHours(1)));

            Assert.Equal("Data início deve ser menor que data fim", exception.Message);
        }

        [Fact(DisplayName = "Constructor with invalid dates should throw immediately")]
        public void Given_InvalidDates_When_CreatingReservation_Then_Throws()
        {
            //Arrange
            var dataFim = DateTime.Now.AddDays(1);
            var dataInicioInvalida = dataFim.AddHours(1);

            //Act
            var ex = Assert.Throws<DomainException>(() =>
                new Reserva(dataInicioInvalida, dataFim, Guid.NewGuid(), Guid.NewGuid()));

            //Assert
            Assert.Equal("Data início deve ser menor que data fim", ex.Message);
        }
    }
}
