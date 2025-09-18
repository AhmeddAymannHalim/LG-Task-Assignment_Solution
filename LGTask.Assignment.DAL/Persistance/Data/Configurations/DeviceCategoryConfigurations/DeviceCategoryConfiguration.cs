using LGTask.Assignment.DAL.Models.DevicesCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LGTask.Assignment.DAL.Persistance.Data.Configurations.CategoryCategoryConfigurations
{
    public class DeviceCategoryConfiguration : IEntityTypeConfiguration<DeviceCategory>
    {
        public void Configure(EntityTypeBuilder<DeviceCategory> builder)
        {
            builder.Property(d => d.CategoryName)
                   .IsRequired();

            builder.HasMany(d => d.CategoryProperties)
                   .WithOne(cp => cp.DeviceCategory)
                   .HasForeignKey(cp => cp.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Devices)
                   .WithOne(dev => dev.DeviceCategory)
                   .HasForeignKey(dev => dev.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
