using LGTask.Assignment.DAL.Models.CategoryProperties;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.Devices;
using LGTask.Assignment.DAL.Models.PropertyItems;
using LGTask.Assignment.DAL.Persistance.Data;
using LGTask.Assignment.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace LGTask.Assignment.DAL.Persistance.Repositories.DeviceCategory
{
    public class DeviceCategoryRepository : IDeviceRepository
    {
        private readonly ITDepartmentDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceCategoryRepository(ITDepartmentDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Models.DevicesCategories.DeviceCategory>> GetAllCategoriesAsync()
        {
            return await _dbContext.DeviceCategories
                .Include(c => c.CategoryProperties)
                .ThenInclude(cp => cp.PropertyItem)
                .ToListAsync();
        }
        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _dbContext.Devices
                                   .Include(d => d.DeviceCategory)
                                   .Include(d => d.DevicePropertyValues)
                                   .ThenInclude(d => d.Property)
                                   .ToListAsync();

        }
        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
            return await _dbContext.Devices
                                   .Include(d => d.DeviceCategory)
                                   .Include(d => d.DevicePropertyValues)
                                   .ThenInclude(d => d.Property)
                                   .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task<IEnumerable<PropertyItem>> GetAllPropertyItemsAsync()
        {
            return await _dbContext.PropertyItems.ToListAsync();
        }
        public async Task<PropertyItem?> GetPropertyItemByIdAsync(int propertyId)
        {
            return await _dbContext.PropertyItems.FindAsync(propertyId);
        }
        public async Task AddDeviceAsync(Device device)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            await _dbContext.Devices.AddAsync(device);
            await _unitOfWork.CompleteAsync();

        }
        public async Task UpdateDeviceAsync(Device device)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _dbContext.Devices.Update(device);
            await _unitOfWork.CompleteAsync();
        }
        public async Task DeleteDeviceAsync(int id)
        {
            var device = await _dbContext.Devices.FindAsync(id);
            if (device is not null)
            {
                _dbContext.Devices.Remove(device);
                await _unitOfWork.CompleteAsync();
            }

        }
        public async Task<IEnumerable<CategoryProperty>> GetPropertiesByCategoryIdAsync(int categoryId)
        {
            return await _dbContext.CategoryProperties
                                   .Include(cp => cp.PropertyItem)
                                   .Where(cp => cp.CategoryId == categoryId)
                                   .ToListAsync();
        }
        public async Task<IEnumerable<DevicePropertyValue>> GetPropertyValuesByDeviceIdAsync(int deviceId)
        {
            return await _dbContext.DevicePropertyValues
                                   .Include(d => d.Property)
                                   .Where(d => d.DeviceId == deviceId)
                                   .ToListAsync();
        }

        public async Task<Models.DevicesCategories.DeviceCategory> AddNewCategory(Models.DevicesCategories.DeviceCategory model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _dbContext.DeviceCategories.Add(model);
            try
            {
                await _unitOfWork.CompleteAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add category due to a database error.", ex);
            }
        }

        public async Task<CategoryProperty> AddNewProperty(CategoryProperty model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            _dbContext.CategoryProperties.Add(model);
            try
            {
                await _unitOfWork.CompleteAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add property due to a database error.", ex);
            }
        }

        public async Task<CategoryProperty?> GetCategoryPropertyAsync(int? categoryId, int? propertyId)
        {

            if (categoryId == null || propertyId == null)
                return null;

            return await _dbContext.CategoryProperties
                .Include(cp => cp.DeviceCategory)
                .Include(cp => cp.PropertyItem)
                .FirstOrDefaultAsync(cp => cp.CategoryId == categoryId && cp.PropertyId == propertyId);
        }

        public async Task<CategoryProperty?> AddCategoryPropertyAsync(CategoryProperty model)
        {

            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var existingProperty = await _dbContext.CategoryProperties
                .FirstOrDefaultAsync(cp => cp.CategoryId == model.CategoryId && cp.PropertyId == model.PropertyId);


            if (existingProperty != null)
                throw new InvalidOperationException("The property already exists for this category.");

            var newModel = new CategoryProperty
            {
                CategoryId = model.CategoryId,
                PropertyId = model.PropertyId
            };

            await _dbContext.CategoryProperties.AddAsync(newModel);
            try
            {
                await _unitOfWork.CompleteAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add category property due to a database error.", ex);
            }
        }

        public async Task<Models.DevicesCategories.DeviceCategory?> GetCategoryByIdAsync(int? categoryId)
        {
            return await _dbContext.DeviceCategories
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<PropertyItem?> GetPropertyByIdAsync(int? propertyId)
        {
            return await _dbContext.PropertyItems
                .FirstOrDefaultAsync(p => p.Id == propertyId);
        }

        public async Task<List<CategoryProperty>> GetAllCategoryPropertiesAsync()
        {
            return await _dbContext.CategoryProperties
                .Include(cp => cp.DeviceCategory)
                .Include(cp => cp.PropertyItem)
                .ToListAsync();
        }

        public async Task DeleteCategoryPropertyByIdAsync(int id)
        {
            var categoryProperty = await _dbContext.CategoryProperties.FindAsync(id);
            if (categoryProperty != null)
            {
                _dbContext.CategoryProperties.Remove(categoryProperty);
                await _unitOfWork.CompleteAsync();
            }

        }

        public async Task<CategoryProperty> UpdateCategoryPropertyAsync(CategoryProperty model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var existingProperty = await _dbContext.CategoryProperties.FindAsync(model.Id);
            if (existingProperty == null)
                throw new InvalidOperationException($"CategoryProperty with ID {model.Id} does not exist.");

            // Check for duplicate CategoryId/PropertyId combination
            var duplicate = await GetCategoryPropertyAsync(model.CategoryId, model.PropertyId);
            if (duplicate != null && duplicate.Id != model.Id)
                throw new InvalidOperationException("The property already exists for this category.");

            existingProperty.CategoryId = model.CategoryId;
            existingProperty.PropertyId = model.PropertyId;
            _dbContext.CategoryProperties.Update(existingProperty);

            try
            {
                await _unitOfWork.CompleteAsync();
                return existingProperty;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update category property due to a database error.", ex);
            }
        }

        public async Task<CategoryProperty?> GetCategoryPropertyByIdAsync(int id)
        {
            return await _dbContext.CategoryProperties
                .Include(cp => cp.DeviceCategory)
                .Include(cp => cp.PropertyItem)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }

        public async Task AddPropertyAsync(PropertyItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.PropertyItems.Add(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePropertyAsync(PropertyItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.PropertyItems.Update(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePropertyAsync(int id)
        {
            if (id == 0)
                throw new ArgumentException("Invalid ID.", nameof(id));

            var item = await _dbContext.PropertyItems.FindAsync(id);
            if (item != null)
            {
                _dbContext.PropertyItems.Remove(item);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
