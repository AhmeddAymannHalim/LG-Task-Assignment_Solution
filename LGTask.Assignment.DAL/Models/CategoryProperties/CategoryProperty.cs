using LGTask.Assignment.DAL.Models.DevicesCategories;
using LGTask.Assignment.DAL.Models.PropertyItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGTask.Assignment.DAL.Models.CategoryProperties
{
    public class CategoryProperty : ModelBase
    {
        public int? CategoryId { get; set; }
        public DeviceCategory DeviceCategory { get; set; } = null!; // Every Property Has a Category not [ Optional ]

        public int? PropertyId { get; set; }
        public PropertyItem PropertyItem { get; set; } = null!; // Every Category Has a Properties  not [ Optional ] =>  IP , ISColor, Brand ...etc

    }
} 
