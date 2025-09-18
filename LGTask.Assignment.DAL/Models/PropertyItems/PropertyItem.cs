using LGTask.Assignment.DAL.Models.CategoryProperties;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;

namespace LGTask.Assignment.DAL.Models.PropertyItems
{
    public class PropertyItem : ModelBase
    {
        public string PropertyDescription { get; set; } = null!;
        public ICollection<CategoryProperty>? CategoryProperties { get; set; } = new List<CategoryProperty>();

        public ICollection<DevicePropertyValue>? DevicePropertyValues { get; set; } = new List<DevicePropertyValue>();
    }
}
