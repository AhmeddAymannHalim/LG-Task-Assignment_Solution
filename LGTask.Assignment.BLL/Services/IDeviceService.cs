using LGTask.Assignment.BLL.ViewModels.CategoryProperty;
using LGTask.Assignment.BLL.ViewModels.CreateDevices;
using LGTask.Assignment.BLL.ViewModels.DeviceCategory;
using LGTask.Assignment.BLL.ViewModels.DevicePropertyValues;
using LGTask.Assignment.BLL.ViewModels.PropertyItems;
using LGTask.Assignment.DAL.Models.PropertyItems;

namespace LGTask.Assignment.BLL.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceVm>> GetAllDevicesAsync();
        Task<DeviceVm?> GetDeviceByIdAsync(int id);
        Task<IEnumerable<PropertyValuesVm>> GetAllPropertiesAsync(); 
        Task<PropertyValuesVm> GetPropertyByIdAsync(int propertyId);
        Task AddDeviceAsync(DeviceVm viewModel);
        Task UpdateDeviceAsync(DeviceVm viewModel);
        Task DeleteDeviceAsync(int id);
        Task<IEnumerable<DeviceCategoryVm>> GetAllCategoriesAsync();
        Task<IEnumerable<PropertyValuesVm>> GetPropertiesByCategoryIdAsync(int categoryId);

        Task<DeviceCategoryVm> AddNewCategory(DeviceCategoryVm model);
        Task<CategoryPropertyVm> AddNewProperty(CategoryPropertyVm model);

        Task<CategoryPropertyVm> AddNewCategoryProperty(CategoryPropertyVm model);

        Task<List<CategoryPropertyVm>> GetAllCategoryPropertiesAsync();
        Task DeleteCategoryPropertyByIdAsync(int id);

        Task<CategoryPropertyVm> UpdateCategoryPropertyAsync(CategoryPropertyVm model);

        Task<CategoryPropertyVm> GetCategoryPropertyByIdAsync(int id);


        Task AddPropertyAsync(PropertyItemVm viewModel);
        Task UpdatePropertyAsync(PropertyItemVm viewModel);
        Task DeletePropertyAsync(int id);

        Task<PropertyItemVm> GetPropertyItemByIdAsync(int id);

        Task<IEnumerable<PropertyItemVm>> GetAllPropertyItemAsync();




    }

}
