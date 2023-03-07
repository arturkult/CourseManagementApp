using Application.Courses;
using Application.UnitTests.Setup;
using Domain.Courses;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Courses.AddCourse;

public class AddCourseTests
{
    [Fact]
    public async Task Handle_PassValidObject_CreateCalledOnce()
    {
        //Arrange
        var unitOfWorkMock = new UnitOfWorkBuilder().WithCourseRepository().Build();
        Course? createdEntity = null;
        unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Create(It.IsAny<Course>(), CancellationToken.None))
            .Callback((Course course, CancellationToken token) => createdEntity = course);
        var command = new AddCourseCommand("test", "description", Array.Empty<AddCourseTopic>());
        var handler = new AddCourseHandler(unitOfWorkMock.Object);

        //Act
        await handler.Handle(command, CancellationToken.None);
        
        //Assert
        unitOfWorkMock.Verify(unitOfWork =>
                unitOfWork.Courses.Create(
                    It.Is<Course>(course => course.Name == command.Name),
                    CancellationToken.None),
            Times.Once);
        unitOfWorkMock.Verify(unitOfWork =>
                unitOfWork.Complete(CancellationToken.None),
            Times.Once);
        createdEntity.Should().NotBeNull();
        createdEntity.Name.Should().Be(command.Name);
        createdEntity.Description.Should().Be(command.Description);
    }
}