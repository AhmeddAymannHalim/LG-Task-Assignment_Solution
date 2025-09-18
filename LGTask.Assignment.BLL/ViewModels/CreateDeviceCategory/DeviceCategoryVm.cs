using LGTask.Assignment.BLL.ViewModels.CategoryProperty;
using System.ComponentModel.DataAnnotations;

namespace LGTask.Assignment.BLL.ViewModels.DeviceCategory
{
    public class DeviceCategoryVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Category Name is required")]
        public string CategoryName { get; set; } = null!;
        public List<CategoryPropertyVm>? CategoryProperties { get; set; } = new List<CategoryPropertyVm>();


    }
}
