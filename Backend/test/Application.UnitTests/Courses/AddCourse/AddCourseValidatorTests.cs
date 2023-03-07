using Application.Courses;
using Application.UnitTests.TestData;
using FluentAssertions;

namespace Application.UnitTests.Courses.AddCourse;

public class AddCourseValidatorTests
{
    [Fact]
    public void Validate_ValidCommand_ValidResultReturned()
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommand("test", "description", new[]
        {
            new AddCourseTopic("name", 0),
            new AddCourseTopic("name1", 2),
            new AddCourseTopic("name1", 1),
        });

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(TooShortStringData))]
    public void Validate_TooShortName_InvalidResultReturned(string name)
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithName(name)
            .WithDescrption("test")
            .WithTopic("t1", 0)
            .WithTopic("t2", 1)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("NameTooShort"));
    }
    
    [Fact]
    public void Validate_TooLongName_InvalidResultReturned()
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithName(LongStringGenerator.Generate(31))
            .WithDescrption("test")
            .WithTopic("t1", 0)
            .WithTopic("t2", 1)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("NameTooLong"));
    }
    
    [Theory]
    [ClassData(typeof(TooShortStringData))]
    public void Validate_TooShortDescription_InvalidResultReturned(string description)
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithDescrption(description)
            .WithName("test")
            .WithTopic("t1", 0)
            .WithTopic("t2", 1)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("DescriptionTooShort"));
    }
    
    [Fact]
    public void Validate_TooLongDescription_InvalidResultReturned()
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithDescrption(LongStringGenerator.Generate(2001))
            .WithName("test")
            .WithTopic("t1", 0)
            .WithTopic("t2", 1)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("DescriptionTooLong"));
    }
    
    [Theory]
    [ClassData(typeof(TooShortStringData))]
    public void Validate_TooShortTopicName_InvalidResultReturned(string name)
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithDescrption("descroption")
            .WithName("test")
            .WithTopic(name, 0)
            .WithTopic("t2", 1)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("TopicNameTooShort"));
    }
    
    [Fact]
    public void Validate_TooLongTopicName_InvalidResultReturned()
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithDescrption("description")
            .WithName("test")
            .WithTopic(LongStringGenerator.Generate(41), 0)
            .WithTopic("t2", 1)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("TopicNameTooLong"));
    }
    
    [Fact]
    public void Validate_NoTopics_InvalidResultReturned()
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithDescrption("description")
            .WithName("test")
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("AtLeastOneTopicRequired"));
    }
    
    [Fact]
    public void Validate_InvalidTopicsOrder_InvalidResultReturned()
    {
        var validator = new AddCourseValidator();
        var command = new AddCourseCommandBuilder()
            .WithDescrption("description")
            .WithName("test")
            .WithTopic("t1", 0)
            .WithTopic("t2", 2)
            .WithTopic("t2", 3)
            .Build();

        var result = validator.Validate(command);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorCode.Equals("InvalidTopicsOrder"));
    }
}