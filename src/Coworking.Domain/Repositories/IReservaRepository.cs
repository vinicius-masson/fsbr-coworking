using Coworking.Domain.Entities;

namespace Coworking.Domain.Repositories
{
    public interface IReservaRepository
    {
        Task<Reserva> CreateAsync(Reserva reserva, CancellationToken cancellationToken = default);
        Task<Reserva?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Reserva> UpdateAsync(Reserva reserva, CancellationToken cancellationToken = default);
        IQueryable<Reserva?> GetAll(CancellationToken cancellationToken = default);
        Task<bool> ExisteConflitoReservaAsync(Guid salaId, DateTime dataInicio, DateTime dataFim, Guid? reservaId = null, CancellationToken cancellationToken = default);
    }
}
