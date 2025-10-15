namespace InstaDelivery.OrderService.Repository.Contracts
{
    public interface IUnitOfWork
    {
        public IOrderRepository Orders { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
