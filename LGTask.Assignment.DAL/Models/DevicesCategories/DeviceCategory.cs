using LGTask.Assignment.DAL.Models.CategoryProperties;
using LGTask.Assignment.DAL.Models.Devices;

namespace LGTask.Assignment.DAL.Models.DevicesCategories
{
    public class DeviceCategory : ModelBase
    {
        public string CategoryName { get; set; } = null!;
        public ICollection<CategoryProperty> CategoryProperties { get; set; } = new List<CategoryProperty>();
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
