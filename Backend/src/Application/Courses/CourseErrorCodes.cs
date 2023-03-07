namespace Application.Courses;

public static class CourseErrorCodes
{
    public static string NameTooShort = nameof(NameTooShort);
    public static string NameTooLong = nameof(NameTooLong);
    public static string DescriptionTooShort = nameof(DescriptionTooShort);
    public static string DescriptionTooLong = nameof(DescriptionTooLong);
    public static string TopicNameTooShort = nameof(TopicNameTooShort);
    public static string TopicNameTooLong = nameof(TopicNameTooLong);
    public static string AtLeastOneTopicRequired = nameof(AtLeastOneTopicRequired);
    public static string InvalidTopicsOrder = nameof(InvalidTopicsOrder);
}