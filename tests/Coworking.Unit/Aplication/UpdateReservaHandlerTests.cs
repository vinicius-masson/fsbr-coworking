using AutoMapper;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using Coworking.Aplication.Exceptions;
using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Coworking.Domain.Interfaces;
using Coworking.Domain.Repositories;
using Coworking.Unit.Aplication.TestData;
using FluentAssertions;
using NSubstitute;

namespace Coworking.Unit.Aplication
{
    public class UpdateReservaHandlerTests
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly UpdateReservaHandler _handler;

        public UpdateReservaHandlerTests()
        {
            _reservaRepository = Substitute.For<IReservaRepository>();
            _mapper = Substitute.For<IMapper>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _emailService = Substitute.For<IEmailService>();
            _handler = new UpdateReservaHandler(_reservaRepository, _mapper, _unitOfWork, _emailService);
        }

        [Fact(DisplayName = "Given valid reservation data When updating reservation Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = UpdateReservaHandlerTestData.GenerateValidCommand();
            var reserva = new Reserva(command.DataInicioReserva, command.DataFimReserva, command.UsuarioId, command.SalaId);
            reserva.Id = command.Id;

            var response = new UpdateReservaResponse
            {
                Success = true
            };


            _mapper.Map<Reserva>(command).Returns(reserva);
            _mapper.Map<UpdateReservaResponse>(reserva).Returns(response);

            _reservaRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(reserva);
            _reservaRepository.UpdateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>())
                .Returns(reserva);

            // When
            var updateReservaResponse = await _handler.Handle(command, CancellationToken.None);

            // Then
            updateReservaResponse.Should().NotBeNull();
            updateReservaResponse.Success.Should().Be(true);
            await _reservaRepository.Received(1).UpdateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given valid reservation When updated Then should send confirmation email")]
        public async Task Handle_ValidRequest_ShouldSendEmail()
        {
            // Given
            var command = UpdateReservaHandlerTestData.GenerateValidCommand();
            var reserva = new Reserva(command.DataInicioReserva, command.DataFimReserva, command.UsuarioId, command.SalaId) { Id = command.Id };
            reserva.SetTestSala(new Sala("sala-01", "S01"));
            reserva.SetTestUsuario(new Usuario("João da Silva", "usuarioteste@gmail.com"));

            _mapper.Map<Reserva>(command).Returns(reserva);

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
                    OperacaoReserva.Atualizada,
                    Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given reservation with date time conflicts Then should throw exception")]
        public async Task Handle_ReservationNotFound_ShouldThrowEception()
        {
            // Given
            var command = UpdateReservaHandlerTestData.GenerateValidCommand();
            var reserva = new Reserva(command.DataInicioReserva, command.DataFimReserva, command.UsuarioId, command.SalaId) { Id = command.Id };

            _reservaRepository.GetByIdAsync(reserva.Id).Returns(reserva);

            // When/Then
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new UpdateReservaCommand(), CancellationToken.None));
        }
    }
}
