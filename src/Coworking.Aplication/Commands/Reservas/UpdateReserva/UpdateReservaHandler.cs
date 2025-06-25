using AutoMapper;
using Coworking.Aplication.Commands.Reservas.CreateReserva;
using Coworking.Aplication.Exceptions;
using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Coworking.Domain.Interfaces;
using Coworking.Domain.Repositories;
using MediatR;

namespace Coworking.Aplication.Commands.Reservas.UpdateReserva
{
    public class UpdateReservaHandler : IRequestHandler<UpdateReservaCommand, UpdateReservaResponse>
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public UpdateReservaHandler(IReservaRepository reservaRepository, IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _reservaRepository = reservaRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<UpdateReservaResponse> Handle(UpdateReservaCommand command, CancellationToken cancellationToken)
        {
            var reservaExistente = await _reservaRepository.GetByIdAsync(command.Id)
                ?? throw new NotFoundException("Reserva não encontrada");

            if (await _reservaRepository.ExisteConflitoReservaAsync(command.SalaId, command.DataInicioReserva, command.DataFimReserva, command.Id, cancellationToken: cancellationToken))
                throw new BusinessException("Já existe uma reserva confirmada neste horário");

            _mapper.Map(command, reservaExistente);
            
            await _reservaRepository.UpdateAsync(reservaExistente, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            _ = Task.Run(() =>
                _emailService.EnviarEmailConfirmacaoReservaAsync(reservaExistente.Usuario.Email, reservaExistente.Sala.Codigo, reservaExistente.DataInicioReserva, OperacaoReserva.Atualizada, cancellationToken),
                cancellationToken);

            var reservaResponse = new UpdateReservaResponse { Success = true };
            return reservaResponse;
        }
    }
}
