using AutoMapper;
using Coworking.Aplication.Commands.Reservas.UpdateReserva;
using Coworking.Aplication.Exceptions;
using Coworking.Domain.Enums;
using Coworking.Domain.Interfaces;
using Coworking.Domain.Repositories;
using MediatR;

namespace Coworking.Aplication.Commands.Reservas.CancelReserva
{
    public class CancelReservaHandler : IRequestHandler<CancelReservaCommand, CancelReservaResponse>
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public CancelReservaHandler(IReservaRepository reservaRepository, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _reservaRepository = reservaRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<CancelReservaResponse> Handle(CancelReservaCommand command, CancellationToken cancellationToken)
        {
            var reservaExistente = await _reservaRepository.GetByIdAsync(command.Id)
                ?? throw new NotFoundException("Reserva não encontrada");

            reservaExistente.Cancelar();

            await _reservaRepository.UpdateAsync(reservaExistente, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            _ = Task.Run(() =>
                _emailService.EnviarEmailConfirmacaoReservaAsync(reservaExistente.Usuario.Email, reservaExistente.Sala.Codigo, reservaExistente.DataInicioReserva, OperacaoReserva.Cancelada, cancellationToken),
                cancellationToken);

            return new CancelReservaResponse { Success = true };
        }
    }
}
