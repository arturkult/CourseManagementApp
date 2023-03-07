using Application.Courses.Interfaces;

namespace Application.Common.UnitOfWork;

public interface IUnitOfWork
{
    ICourseRepository Courses { get; }
    Task<int> Complete(CancellationToken token);
}