using InstaDelivery.OrderService.Domain.Entities;
using InstaDelivery.OrderService.Repository.Context;
using InstaDelivery.OrderService.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

internal class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity

{
    protected readonly OrderDbContext _dbContext;
    protected readonly DbSet<T> _set;

    public GenericRepository(OrderDbContext db)
    {
        _dbContext = db ?? throw new ArgumentNullException(nameof(db));
        _set = _dbContext.Set<T>();
    }

    public IQueryable<T> Query()
    {
        return _set.AsQueryable();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _set.FindAsync([id], ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _set.AsNoTracking().ToListAsync(ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _set.AsNoTracking().Where(predicate).ToListAsync(ct).ConfigureAwait(false);
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 10;

        IQueryable<T> query = _set.AsNoTracking();

        if (filter != null) query = query.Where(filter);
        var total = await query.CountAsync(ct).ConfigureAwait(false);

        if (orderBy != null)
            query = orderBy(query);
        else
            query = query.OrderBy(e => e.Id);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct).ConfigureAwait(false);
        return (items, total);
    }

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _set.AddAsync(entity, ct).ConfigureAwait(false);
        return entity;
    }

    public void Update(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _set.Update(entity);
    }

    public void Remove(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _set.Remove(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _set.AnyAsync(predicate, ct).ConfigureAwait(false);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
    {
        return predicate == null ? await _set.CountAsync(ct).ConfigureAwait(false)
                                 : await _set.CountAsync(predicate, ct).ConfigureAwait(false);
    }
}
