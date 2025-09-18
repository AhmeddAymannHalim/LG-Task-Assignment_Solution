using LGTask.Assignment.DAL.Models.DevicesCategories;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;

namespace LGTask.Assignment.DAL.Models.Devices
{
    public class Device : ModelBase
    {
        public string DeviceName { get; set; } = null!;
        public string SerialNo { get; set; } = null!;
        public DateOnly AcquisitionDate { get; set; }
        public int CategoryId { get; set; }
        public DeviceCategory DeviceCategory { get; set; } = null!; // EveryDevice must have a category not [ Optional ]
        public ICollection<DevicePropertyValue> DevicePropertyValues { get; set; } = null!;

    }
}
