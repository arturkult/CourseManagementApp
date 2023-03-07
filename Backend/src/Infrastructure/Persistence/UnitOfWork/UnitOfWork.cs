using Application.Common.UnitOfWork;
using Application.Courses.Interfaces;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork: IUnitOfWork
{
    private readonly CoursesDbContext _context;
    private ICourseRepository? _courseRepository;

    public UnitOfWork(CoursesDbContext context)
    {
        _context = context;
    }

    public ICourseRepository Courses
    {
        get { return _courseRepository ??= new CourseRepository(_context); }
    }

    public Task<int> Complete(CancellationToken token)
    {
        return _context.SaveChangesAsync(token);
    }
}