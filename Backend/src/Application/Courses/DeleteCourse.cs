using Application.Common.UnitOfWork;
using Application.Exceptions;
using MediatR;

namespace Application.Courses;

public record DeleteCourseCommand (Guid Id): IRequest<Unit>;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetById(request.Id, cancellationToken);
        if (course is null)
        {
            throw new NotFoundException();
        }
        await _unitOfWork.Courses.Delete(course, cancellationToken);
        await _unitOfWork.Complete(cancellationToken);
        return Unit.Value;
    }
}