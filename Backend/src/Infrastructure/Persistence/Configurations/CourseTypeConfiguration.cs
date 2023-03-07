using Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourseTypeConfiguration: IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses", "courses");
        builder.Property(course => course.Name).HasMaxLength(30);
        builder.Property(course => course.Description).HasMaxLength(2000);
        builder.HasKey(course => course.Id);
        builder.HasMany(course => course.Topics)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}