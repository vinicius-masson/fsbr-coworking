using AutoMapper;
using Coworking.Aplication.Commands.Reservas.CancelReserva;
using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Coworking.Domain.Interfaces;
using Coworking.Domain.Repositories;
using Coworking.Unit.Aplication.TestData;
using Coworking.Unit.Domain.Entities.TestData;
using FluentAssertions;
using NSubstitute;

namespace Coworking.Unit.Aplication
{
    public class CancelReservaHandlerTests
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly CancelReservaHandler _handler;

        public CancelReservaHandlerTests()
        {
            _reservaRepository = Substitute.For<IReservaRepository>();
            _mapper = Substitute.For<IMapper>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _emailService = Substitute.For<IEmailService>();
            _handler = new CancelReservaHandler(_reservaRepository, _unitOfWork, _emailService);
        }

        [Fact(DisplayName = "Given valid reservation data When canceling reservation Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = CancelReservaHandlerTestData.GenerateValidCommand();

            var reserva = ReservaTestData.GenerateValidReservation();
            reserva.Id = command.Id;

            var response = new CancelReservaResponse
            {
                Success = true
            };

            _reservaRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(reserva);
            _reservaRepository.UpdateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>())
                .Returns(reserva);

            // When
            var cancelReservaResponse = await _handler.Handle(command, CancellationToken.None);

            // Then
            cancelReservaResponse.Should().NotBeNull();
            cancelReservaResponse.Success.Should().Be(true);
            await _reservaRepository.Received(1).UpdateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given valid reservation When canceled Then should send confirmation email")]
        public async Task Handle_ValidRequest_ShouldSendEmail()
        {
            // Given
            var command = CancelReservaHandlerTestData.GenerateValidCommand();
            var reserva = ReservaTestData.GenerateValidReservation();
            
            reserva.Id = command.Id;
            reserva.SetTestSala(new Sala("sala-01", "S01"));
            reserva.SetTestUsuario(new Usuario("João da Silva", "usuarioteste@gmail.com"));

            _reservaRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(reserva);
            _reservaRepository.UpdateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>()).Returns(reserva);

            // When
            await _handler.Handle(command, CancellationToken.None);

            // Then
            await _emailService.Received(1)
                .EnviarEmailConfirmacaoReservaAsync(
                    reserva.Usuario.Email,
                    reserva.Sala.Codigo,
                    reserva.DataInicioReserva,
                    OperacaoReserva.Cancelada,
                    Arg.Any<CancellationToken>());
        }
    }
}
