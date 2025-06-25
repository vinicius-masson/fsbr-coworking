using AutoMapper;
using Coworking.Aplication.Queries.Reservas.GetReserva;
using Coworking.Domain.Repositories;
using MediatR;

namespace Coworking.Aplication.Queries.Reservas.GetAllReservas
{
    public class GetAllReservasHandler : IRequestHandler<GetAllReservasQuery, List<GetReservaResponse>>
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IMapper _mapper;

        public GetAllReservasHandler(IReservaRepository reservaRepository, IMapper mapper)
        {
            _reservaRepository = reservaRepository;
            _mapper = mapper;
        }

        public async Task<List<GetReservaResponse>> Handle(GetAllReservasQuery request, CancellationToken cancellationToken)
        {
            var reservas = _reservaRepository
                .GetAll(cancellationToken)
                .OrderByDescending(r => r.DataInicioReserva)
                .ToList();

            var result = _mapper.Map<List<GetReservaResponse>>(reservas);

            return await Task.FromResult(result);
        }
    }
}
