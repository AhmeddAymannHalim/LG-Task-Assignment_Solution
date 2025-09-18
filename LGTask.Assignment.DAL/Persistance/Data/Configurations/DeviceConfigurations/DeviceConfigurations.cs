using LGTask.Assignment.DAL.Models.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LGTask.Assignment.DAL.Persistance.Data.Configurations.DeviceConfigurations
{
    public class DeviceConfigurations : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {

            builder.Property(d => d.Id)
                   .UseIdentityColumn(10, 10);

            builder.Property(d => d.DeviceName)
                   .IsRequired();

           

            builder.Property(d => d.AcquisitionDate)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.HasMany(d => d.DevicePropertyValues)
                   .WithOne(dp => dp.Device)
                   .HasForeignKey(dp => dp.DeviceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.DeviceCategory)
                   .WithMany(d => d.Devices)
                   .HasForeignKey(d => d.CategoryId);


        }
    }
}
