using Application.Courses;

namespace Application.UnitTests.Courses.AddCourse;

public class AddCourseCommandBuilder
{
    private string _name = string.Empty;
    private string _description;
    private List<AddCourseTopic> _topics = new List<AddCourseTopic>();

    public AddCourseCommandBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public AddCourseCommandBuilder WithDescrption(string description)
    {
        _description = description;
        return this;
    }

    public AddCourseCommandBuilder WithTopic(string name, int number)
    {
        _topics.Add(new AddCourseTopic(name, number));
        return this;
    }

    public AddCourseCommand Build()
    {
        return new AddCourseCommand(_name, _description, _topics.ToArray());
    }
}