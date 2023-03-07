using Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourseTopicTypeConfiguration: IEntityTypeConfiguration<CourseTopic>
{
    public void Configure(EntityTypeBuilder<CourseTopic> builder)
    {
        builder.ToTable("CourseTopics", "courses");
        builder.Property(topic => topic.Name).HasMaxLength(40);
    }
}