using AutoMapper;
using Coworking.Domain.Repositories;
using MediatR;

namespace Coworking.Aplication.Queries.Reservas.GetReserva
{
    public class GetReservaHandler : IRequestHandler<GetReservaQuery, GetReservaResponse>
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;

        public GetReservaHandler(IReservaRepository reservaRepository, IMapper mapper)
        {
            _reservaRepository = reservaRepository;
            _mapper = mapper;
        }

        public async Task<GetReservaResponse> Handle(GetReservaQuery request, CancellationToken cancellationToken)
        {
            var reserva = await _reservaRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reserva == null)
                throw new KeyNotFoundException($"Reserva com ID {request.Id} não foi encontrada");

            var reservaResponse = _mapper.Map<GetReservaResponse>(reserva);
            return reservaResponse;
        }
    }
}
