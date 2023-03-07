using Application.Common.UnitOfWork;
using Application.Courses.Interfaces;
using Moq;

namespace Application.UnitTests.Setup;

public class UnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UnitOfWorkBuilder()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
    }

    public UnitOfWorkBuilder WithCourseRepository()
    {
        var repositoryMock = new Mock<ICourseRepository>();
        _unitOfWork.Setup(work => work.Courses)
            .Returns(() => repositoryMock.Object);
        
        return this;
    }

    public Mock<IUnitOfWork> Build()
    {
        return _unitOfWork;
    }
}