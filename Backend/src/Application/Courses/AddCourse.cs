using Application.Common.UnitOfWork;
using Domain.Courses;
using FluentValidation;
using MediatR;

namespace Application.Courses;

public sealed record AddCourseCommand(string Name, string Description, AddCourseTopic[] Topics) : IRequest<Unit>;

public sealed record AddCourseTopic(string Name, int Number);

public sealed class AddCourseValidator : AbstractValidator<AddCourseCommand>
{
    public AddCourseValidator()
    {
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

public sealed class AddCourseHandler : IRequestHandler<AddCourseCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Name = request.Name,
            Description = request.Description,
            Topics = request.Topics.Select(topic => new CourseTopic
            {
                Name = topic.Name,
                Number = topic.Number
            }).ToList()
        };
        await _unitOfWork.Courses.Create(course, cancellationToken);
        await _unitOfWork.Complete(cancellationToken);
        
        return Unit.Value;
    }
}