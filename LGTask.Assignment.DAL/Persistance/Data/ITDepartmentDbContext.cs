using LGTask.Assignment.DAL.Models.DevicesCategories;
using LGTask.Assignment.DAL.Models.CategoryProperties;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.Devices;
using LGTask.Assignment.DAL.Models.PropertyItems;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LGTask.Assignment.DAL.Persistance.Data
{
    public class ITDepartmentDbContext : DbContext
    {

        public ITDepartmentDbContext(DbContextOptions<ITDepartmentDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceCategory>  DeviceCategories { get; set; }

        public DbSet<CategoryProperty> CategoryProperties { get; set; }

        public DbSet<PropertyItem> PropertyItems { get; set; }

        public DbSet<DevicePropertyValue> DevicePropertyValues { get; set; }


    }
}
