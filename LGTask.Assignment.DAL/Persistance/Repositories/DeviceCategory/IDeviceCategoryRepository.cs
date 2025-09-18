using LGTask.Assignment.DAL.Models.CategoryProperties;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.Devices;
using LGTask.Assignment.DAL.Models.PropertyItems;

namespace LGTask.Assignment.DAL.Persistance.Repositories.DeviceCategory
{
    public interface IDeviceRepository
    {

        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(int id);
        Task<IEnumerable<PropertyItem>> GetAllPropertyItemsAsync();
        Task<PropertyItem?> GetPropertyItemByIdAsync(int propertyId); 
        Task AddDeviceAsync(Device device);
        Task UpdateDeviceAsync(Device device);
        Task DeleteDeviceAsync(int id);
        Task<IEnumerable<Models.DevicesCategories.DeviceCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryProperty>> GetPropertiesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<DevicePropertyValue>> GetPropertyValuesByDeviceIdAsync(int deviceId);
        Task<CategoryProperty?> GetCategoryPropertyAsync(int? categoryId, int? propertyId);
        Task<CategoryProperty?> AddCategoryPropertyAsync(CategoryProperty model);

        Task<Models.DevicesCategories.DeviceCategory> AddNewCategory(Models.DevicesCategories.DeviceCategory model);
        Task<Models.DevicesCategories.DeviceCategory?> GetCategoryByIdAsync(int? categoryId);
        Task<PropertyItem?> GetPropertyByIdAsync(int? propertyId);
        Task<CategoryProperty> AddNewProperty(CategoryProperty model);

        Task<List<CategoryProperty>> GetAllCategoryPropertiesAsync();

        Task DeleteCategoryPropertyByIdAsync(int id);

        Task<CategoryProperty> UpdateCategoryPropertyAsync(CategoryProperty model);

        Task<CategoryProperty?> GetCategoryPropertyByIdAsync(int id);

        Task AddPropertyAsync(PropertyItem item);
        Task UpdatePropertyAsync(PropertyItem item);
        Task DeletePropertyAsync(int id);
    }
}
