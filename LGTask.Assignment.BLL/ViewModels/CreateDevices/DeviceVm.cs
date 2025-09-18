using LGTask.Assignment.BLL.ViewModels.DeviceCategory;
using LGTask.Assignment.BLL.ViewModels.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.DevicesCategories;
using System.ComponentModel.DataAnnotations;

namespace LGTask.Assignment.BLL.ViewModels.CreateDevices
{
    public class DeviceVm
    {
        public int Id { get; set; }
        public string DeviceName { get; set; } = null!;
        public string SerialNo { get; set; } = null!;
        public DateOnly AcquisitionDate { get; set; } 

        [Display(Name = "Categories")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<DeviceCategoryVm> Categories { get; set; } = new List<DeviceCategoryVm>();
        public int? PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public List<PropertyValuesVm> Properties { get; set; } = new List<PropertyValuesVm>();
    }
}
