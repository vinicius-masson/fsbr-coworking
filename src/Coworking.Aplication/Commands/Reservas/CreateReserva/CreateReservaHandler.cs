using AutoMapper;
using Coworking.Aplication.Exceptions;
using Coworking.Domain.Repositories;
using Coworking.Domain.Entities;
using MediatR;
using Coworking.Domain.Interfaces;
using Coworking.Domain.Enums;

namespace Coworking.Aplication.Commands.Reservas.CreateReserva
{
    public class CreateReservaHandler : IRequestHandler<CreateReservaCommand, CreateReservaResponse>
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public CreateReservaHandler(IReservaRepository reservaRepository, IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _reservaRepository = reservaRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<CreateReservaResponse> Handle(CreateReservaCommand command, CancellationToken cancellationToken)
        {
            if (await _reservaRepository.ExisteConflitoReservaAsync(command.SalaId, command.DataInicioReserva, command.DataFimReserva, cancellationToken: cancellationToken))
                throw new BusinessException("Já existe uma reserva confirmada neste horário");

            var reserva = _mapper.Map<Reserva>(command);
            var reservaCriada = await _reservaRepository.CreateAsync(reserva, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            _ = Task.Run(() =>
                _emailService.EnviarEmailConfirmacaoReservaAsync(reservaCriada.Usuario.Email, reservaCriada.Sala.Codigo, reservaCriada.DataInicioReserva, OperacaoReserva.Confirmada, cancellationToken),
                cancellationToken);

            var reservaResponse = _mapper.Map<CreateReservaResponse>(reservaCriada);
            return reservaResponse;
        }
    }
}
