using InstaDelivery.OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace InstaDelivery.OrderService.Repository.Contracts;

public interface IGenericRepository<T> where T : class, IEntity
{
    IQueryable<T> Query();

    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken ct = default);

    Task<T> AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
}