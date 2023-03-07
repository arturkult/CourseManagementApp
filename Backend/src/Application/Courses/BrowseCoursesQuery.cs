using Application.Common.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses;

public record BrowseCoursesQuery(int Skip, int Take) : IRequest<BrowseCoursesResult[]>;

public record BrowseCoursesResult(Guid Id, string Name, string Description, BrowseCoursesTopic[] Topics);

public record BrowseCoursesTopic(string Name, int Number);

public class BrowseCoursesHandler : IRequestHandler<BrowseCoursesQuery, BrowseCoursesResult[]>
{
    private readonly IUnitOfWork _unitOfWork;

    public BrowseCoursesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<BrowseCoursesResult[]> Handle(BrowseCoursesQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.Courses.Browse(
                course => new BrowseCoursesResult(course.Id, course.Name, course.Description,
                    course.Topics
                        .OrderBy(topic => topic.Number)
                        .Select(topic => new BrowseCoursesTopic(topic.Name, topic.Number)).ToArray()),
                request.Skip,
                request.Take)
            .ToArrayAsync(cancellationToken);
    }
}