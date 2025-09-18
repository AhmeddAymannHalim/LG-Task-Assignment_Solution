using LGTask.Assignment.DAL.Models.CategoryProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LGTask.Assignment.DAL.Persistance.Data.Configurations.CategoryPropertyConfigurations
{
    public class CategoryPropertyConfiguration : IEntityTypeConfiguration<CategoryProperty>
    {
        public void Configure(EntityTypeBuilder<CategoryProperty> builder)
        {
            builder.HasOne(c => c.DeviceCategory)
                   .WithMany(c => c.CategoryProperties)
                   .HasForeignKey(c => c.CategoryId);

            builder.HasOne(c => c.PropertyItem)
                   .WithMany(p => p.CategoryProperties)
                   .HasForeignKey(c => c.PropertyId);
        }
    }
}
