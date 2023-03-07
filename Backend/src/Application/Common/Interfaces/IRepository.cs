using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IRepository<T> where T: class
{
    Task Create(T entity, CancellationToken token = default);
    Task Update(T entity, CancellationToken token = default);
    ValueTask<T?> GetById(Guid id, CancellationToken token = default);
    IQueryable<TProject> Browse<TProject>(Expression<Func<T, TProject>> selectExpression, int skip, int take);
    Task Delete(T entity, CancellationToken token = default);
}