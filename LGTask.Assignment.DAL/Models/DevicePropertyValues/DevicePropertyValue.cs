using LGTask.Assignment.DAL.Models.Devices;
using LGTask.Assignment.DAL.Models.PropertyItems;

namespace LGTask.Assignment.DAL.Models.DevicePropertyValues
{
    public class DevicePropertyValue : ModelBase
    {
        public int DeviceId { get; set; }
        public Device Device  { get; set; } = null!;
        public int PropertyId { get; set; }
        public PropertyItem Property { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
