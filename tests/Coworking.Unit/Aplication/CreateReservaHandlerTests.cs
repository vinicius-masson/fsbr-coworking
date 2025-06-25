using AutoMapper;
using Coworking.Aplication.Commands.Reservas.CreateReserva;
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
    public class CreateReservaHandlerTests
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly CreateReservaHandler _handler;

        public CreateReservaHandlerTests()
        {
            _reservaRepository = Substitute.For<IReservaRepository>();
            _mapper = Substitute.For<IMapper>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _emailService = Substitute.For<IEmailService>();
            _handler = new CreateReservaHandler(_reservaRepository, _mapper, _unitOfWork, _emailService);
        }

        [Fact(DisplayName = "Given valid reservation data When creating reservation Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = CreateReservaHandlerTestData.GenerateValidCommand();
            var reserva = new Reserva(command.DataInicioReserva, command.DataFimReserva, command.UsuarioId, command.SalaId);
            reserva.Id = Guid.NewGuid();

            var response = new CreateReservaResponse
            {
                DataInicioReserva = reserva.DataInicioReserva,
                DataFimReserva = reserva.DataFimReserva,
                SalaId = reserva.SalaId,
                UsuarioId = reserva.UsuarioId
            };

            _mapper.Map<Reserva>(command).Returns(reserva);
            _mapper.Map<CreateReservaResponse>(reserva).Returns(response);

            _reservaRepository.CreateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>())
                .Returns(reserva);

            // When
            var createReservaResponse = await _handler.Handle(command, CancellationToken.None);

            // Then
            createReservaResponse.Should().NotBeNull();
            //createReservaResponse.Id.Should().Be(reserva.Id);
            await _reservaRepository.Received(1).CreateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given valid reservation When created Then should send confirmation email")]
        public async Task Handle_ValidRequest_ShouldSendEmail()
        {
            // Given
            var command = CreateReservaHandlerTestData.GenerateValidCommand();
            var reserva = new Reserva(command.DataInicioReserva, command.DataFimReserva, command.UsuarioId, command.SalaId) { Id = Guid.NewGuid() };
            reserva.SetTestSala(new Sala("sala-01", "S01"));
            reserva.SetTestUsuario(new Usuario("João da Silva", "usuarioteste@gmail.com"));

            _mapper.Map<Reserva>(command).Returns(reserva);
            _reservaRepository.CreateAsync(Arg.Any<Reserva>(), Arg.Any<CancellationToken>()).Returns(reserva);

            // When
            await _handler.Handle(command, CancellationToken.None);

            // Then
            await _emailService.Received(1)
                .EnviarEmailConfirmacaoReservaAsync(
                    reserva.Usuario.Email,
                    reserva.Sala.Codigo,
                    reserva.DataInicioReserva,
                    OperacaoReserva.Confirmada,
                    Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given reservation with date time conflicts Then should throw exception")]
        public async Task Handle_ReservationConflicts_ShouldThrowEception()
        {
            // Given
            _reservaRepository.ExisteConflitoReservaAsync(default, default, default, default)
                   .ReturnsForAnyArgs(true);

            // When/Then
            await Assert.ThrowsAsync<BusinessException>(() =>
                _handler.Handle(new CreateReservaCommand(), CancellationToken.None));
        }
    }
}
