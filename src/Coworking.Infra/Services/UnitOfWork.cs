using Coworking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Infra.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultContext _context;
        public UnitOfWork(DefaultContext context) => _context = context;

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
