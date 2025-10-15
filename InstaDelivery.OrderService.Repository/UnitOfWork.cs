using InstaDelivery.OrderService.Repository.Context;
using InstaDelivery.OrderService.Repository.Contracts;

namespace InstaDelivery.OrderService.Repository
{
    internal class UnitOfWork(
        OrderDbContext db,
        IOrderRepository orders) : IUnitOfWork
    {
        public IOrderRepository Orders { get; } = orders;

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await db.SaveChangesAsync(ct);

        public void Dispose() => db.Dispose();
    }
}
