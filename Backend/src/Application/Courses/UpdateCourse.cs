using Application.Common.UnitOfWork;
using Domain.Courses;
using FluentValidation;
using MediatR;

namespace Application.Courses;

public sealed record UpdateCourseCommand(Guid Id, string Name, string Description, UpdateCourseTopic[] Topics) : IRequest<Unit>;

public sealed record UpdateCourseTopic(string Name, int Number);

public sealed class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithErrorCode(CourseErrorCodes.NameTooShort)
            .MaximumLength(30)
            .WithErrorCode(CourseErrorCodes.NameTooLong);
        RuleFor(command => command.Description)
            .NotEmpty()
            .WithErrorCode(CourseErrorCodes.DescriptionTooShort)
            .MaximumLength(2000)
            .WithErrorCode(CourseErrorCodes.DescriptionTooLong);
        RuleFor(command => command.Topics)
            .NotEmpty()
            .WithErrorCode(CourseErrorCodes.AtLeastOneTopicRequired)
            .Must(topics =>
            {
                var sum = topics.OrderBy(topic => topic.Number)
                    .Aggregate(0,(agg, topic) => agg + topic.Number);
                return sum == (0 + topics.Length - 1) * topics.Length / 2;
            })
            .WithErrorCode(CourseErrorCodes.InvalidTopicsOrder);
        RuleForEach(command => command.Topics)
            .ChildRules(rules =>
            {
                rules.RuleFor(topic => topic.Name)
                    .NotEmpty()
                    .WithErrorCode(CourseErrorCodes.TopicNameTooShort)
                    .MaximumLength(40)
                    .WithErrorCode(CourseErrorCodes.TopicNameTooLong);
            });
    }
}

public sealed class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetById(request.Id, cancellationToken);
        course.Name = request.Name;
        course.Description = request.Description;
        course.Topics = request.Topics.Select(topic => new CourseTopic()
        {
            Name = topic.Name,
            Number = topic.Number
        }).ToList();
        await _unitOfWork.Courses.Update(course, cancellationToken);
        await _unitOfWork.Complete(cancellationToken);
        
        return Unit.Value;
    }
}