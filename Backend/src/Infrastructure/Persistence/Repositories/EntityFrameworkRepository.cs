using System.Linq.Expressions;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class EntityFrameworkRepository<T>: IRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet;

    public EntityFrameworkRepository(CoursesDbContext context)
    {
        _dbSet = context.Set<T>();
    }
    public async Task Create(T entity, CancellationToken token = default)
    {
        await _dbSet.AddAsync(entity, token);
    }

    public Task Update(T entity, CancellationToken token = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public ValueTask<T?> GetById(Guid id, CancellationToken token = default)
    {
        return _dbSet.FindAsync(id, token);
    }

    public IQueryable<TProject> Browse<TProject>(Expression<Func<T, TProject>> selectExpression, int skip, int take)
    {
        return _dbSet
            .Select(selectExpression)
            .Skip(skip)
            .Take(take)
            .AsQueryable();
    }

    public Task Delete(T entity, CancellationToken token = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}