using LGTask.Assignment.DAL.Models.Devices;
using LGTask.Assignment.DAL.Models.PropertyItems;

namespace LGTask.Assignment.BLL.ViewModels.DevicePropertyValues
{
    public class PropertyValuesVm
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int? PropertyId { get; set; } 
        public string DeviceName { get; set; } = null!;
        public string PropertyName { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
