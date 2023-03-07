using Application.Common.UnitOfWork;
using Application.Exceptions;
using MediatR;

namespace Application.Courses;

public record GetCourseByIdQuery(Guid Id) : IRequest<GetCourseByIdResult>;

public record GetCourseByIdResult(string Name, string Description, GetCourseTopicResult[] Topics);

public record GetCourseTopicResult(string Name, int Number);

public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, GetCourseByIdResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetCourseByIdResult> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetById(request.Id, cancellationToken);
        if (course is null)
        {
            throw new NotFoundException();
        }

        return new GetCourseByIdResult(
            course.Name,
            course.Description,
            course.Topics
                .Select(topic =>
                    new GetCourseTopicResult(topic.Name, topic.Number))
                .ToArray());
    }
}