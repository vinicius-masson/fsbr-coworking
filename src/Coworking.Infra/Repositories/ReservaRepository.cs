using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Coworking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Infra.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly DefaultContext _context;
        
        public ReservaRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Reserva> CreateAsync(Reserva reserva, CancellationToken cancellationToken = default)
        {
            await _context.Reservas.AddAsync(reserva, cancellationToken);

            await _context.Entry(reserva)
                .Reference(r => r.Usuario)
                .LoadAsync(cancellationToken);

            await _context.Entry(reserva)
                .Reference(r => r.Sala)
                .LoadAsync(cancellationToken);

            return reserva;
        }

        public async Task<Reserva> UpdateAsync(Reserva reserva, CancellationToken cancellationToken = default)
        {
            var entry = _context.Entry(reserva);
            entry.State = EntityState.Modified;

            await Task.CompletedTask;
            return reserva;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var reserva = await GetByIdAsync(id);
            if (reserva == null)
                return false;

            _context.Remove(reserva);
            return true;
        }

        public IQueryable<Reserva?> GetAll(CancellationToken cancellationToken = default)
        {
            return _context.Reservas
                .Where(r => r.Status == StatusReserva.Confirmada)
                .Include(r => r.Sala)
                .Include(r => r.Usuario)
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task<Reserva?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas.Include(r => r.Sala).Include(r => r.Usuario).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ExisteConflitoReservaAsync(Guid salaId, DateTime dataInicio, DateTime dataFim, Guid? reservaId = null, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Where(r => r.SalaId == salaId)
                .Where(r => r.Status == StatusReserva.Confirmada)
                .Where(r => reservaId == null || r.Id != reservaId.Value)
                .AnyAsync(r =>
                    (dataInicio >= r.DataInicioReserva && dataInicio < r.DataFimReserva) ||
                    (dataFim > r.DataInicioReserva && dataFim <= r.DataFimReserva) ||
                    (dataInicio <= r.DataInicioReserva && dataFim >= r.DataFimReserva),
                    cancellationToken);
        }
    }
}
