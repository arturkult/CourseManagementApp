using Application.Courses.Interfaces;
using Domain.Courses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository: EntityFrameworkRepository<Course>, ICourseRepository
{
    public CourseRepository(CoursesDbContext context) : base(context)
    {
    }

    public async ValueTask<Course?> GetById(Guid id, CancellationToken token = default)
    {
        return await _dbSet.Include(i => i.Topics)
            .SingleOrDefaultAsync(course=> course.Id == id, token);
    }
}