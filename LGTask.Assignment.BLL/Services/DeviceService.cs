using LGTask.Assignment.BLL.ViewModels.CategoryProperty;
using LGTask.Assignment.BLL.ViewModels.CreateDevices;
using LGTask.Assignment.BLL.ViewModels.DeviceCategory;
using LGTask.Assignment.BLL.ViewModels.DevicePropertyValues;
using LGTask.Assignment.BLL.ViewModels.PropertyItems;
using LGTask.Assignment.DAL.Models.CategoryProperties;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.Devices;
using LGTask.Assignment.DAL.Models.DevicesCategories;
using LGTask.Assignment.DAL.Models.PropertyItems;
using LGTask.Assignment.DAL.Persistance.Repositories.DeviceCategory;
using Microsoft.EntityFrameworkCore;

namespace LGTask.Assignment.BLL.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DeviceCategoryVm>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllCategoriesAsync();

            return categories.Select(c => new DeviceCategoryVm
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                CategoryProperties = c.CategoryProperties.Select(cp => new CategoryPropertyVm
                {
                    Id = cp.Id,
                    CategoryId = cp.CategoryId,
                    PropertyId = cp.PropertyId,
                    PropertyName = cp.PropertyItem?.PropertyDescription ?? string.Empty
                }).ToList()
            });
        }

        public async Task<IEnumerable<DeviceVm>> GetAllDevicesAsync()
        {
            var devices = await _repository.GetAllDevicesAsync();
            return devices.Select(d =>
            {
                var propertyValue = d.DevicePropertyValues.FirstOrDefault();
                return new DeviceVm
                {
                    Id = d.Id,
                    AcquisitionDate = d.AcquisitionDate,
                    CategoryId = d.CategoryId,
                    CategoryName = d.DeviceCategory?.CategoryName,
                    DeviceName = d.DeviceName,
                    SerialNo = d.SerialNo,
                    PropertyId = propertyValue?.PropertyId,
                    PropertyName = propertyValue?.Property?.PropertyDescription,
                    PropertyValue = propertyValue?.Value,
                    Properties = d.DevicePropertyValues.Select(p => new PropertyValuesVm
                    {
                        PropertyId = p.PropertyId,
                        PropertyName = p.Property?.PropertyDescription ?? string.Empty,
                        DeviceId = p.DeviceId,
                        Value = p.Value ?? string.Empty
                    }).ToList()
                };
            }).ToList();
        }

        public async Task<IEnumerable<PropertyValuesVm>> GetPropertiesByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
                return new List<PropertyValuesVm>();


            var properties = await _repository.GetPropertiesByCategoryIdAsync(categoryId);

            if (properties == null || !properties.Any())
                throw new Exception("No properties found for this category");

            return properties?.Select(p => new PropertyValuesVm
            {
                Id = p.PropertyId ?? 0,
                PropertyId = p.PropertyId,
                PropertyName = p.PropertyItem.PropertyDescription,
                DeviceId = 0,
                DeviceName = string.Empty,
                Value = string.Empty
            }) ?? new List<PropertyValuesVm>();
        }

        public async Task<IEnumerable<PropertyValuesVm>> GetAllPropertiesAsync()
        {
            var properties = await _repository.GetAllPropertyItemsAsync();
            return properties?.Select(p => new PropertyValuesVm
            {
                Id = p.Id,
                PropertyId = p.Id,
                PropertyName = p.PropertyDescription,
                DeviceId = 0,
                Value = string.Empty,
                DeviceName = string.Empty

            }) ?? new List<PropertyValuesVm>();
        }

        public async Task<IEnumerable<PropertyItemVm>> GetAllPropertyItemAsync()
        {
            var properties = await _repository.GetAllPropertyItemsAsync();
            return properties.Select(p => new PropertyItemVm
            {
                Id = p.Id,
                PropertyDescription = p.PropertyDescription
            }).ToList();
        }

        public async Task<PropertyValuesVm> GetPropertyByIdAsync(int propertyId)
        {
            var property = await _repository.GetPropertyItemByIdAsync(propertyId);
            return property != null ? new PropertyValuesVm
            {
               Id = property.Id,
                PropertyId = property.Id,
                PropertyName = property.PropertyDescription,
                DeviceId = 0,
                Value = string.Empty,
                DeviceName = string.Empty

            } : null!;
        }
        public async Task<DeviceVm?> GetDeviceByIdAsync(int id)
        {
            var device = await _repository.GetDeviceByIdAsync(id);

            if (device is null)
                return null;

            var propertyValue = device.DevicePropertyValues.FirstOrDefault();
            return new DeviceVm
            {
                Id = device.Id,
                AcquisitionDate = device.AcquisitionDate,
                CategoryId = device.CategoryId,
                CategoryName = device.DeviceCategory?.CategoryName,
                DeviceName = device.DeviceName,
                SerialNo = device.SerialNo,
                PropertyId = propertyValue?.PropertyId,
                PropertyName = propertyValue?.Property?.PropertyDescription,
                PropertyValue = propertyValue?.Value,
                Properties = device.DevicePropertyValues.Select(p => new PropertyValuesVm
                {
                    PropertyId = p.PropertyId,
                    PropertyName = p.Property?.PropertyDescription ?? string.Empty,
                    DeviceId = p.DeviceId,
                    Value = p.Value ?? string.Empty
                }).ToList()
            };

        }

        public async Task AddDeviceAsync(DeviceVm viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (viewModel.CategoryId <= 0)
                throw new ArgumentException("Invalid category selected");

            if (string.IsNullOrWhiteSpace(viewModel.DeviceName))
                throw new ArgumentException("Device name is required");

            if (string.IsNullOrWhiteSpace(viewModel.SerialNo))
                throw new ArgumentException("Serial number is required");

            var devicePropertyValues = new List<DevicePropertyValue>();
            if (viewModel.PropertyId.HasValue && viewModel.PropertyId > 0)
            {
                var property = await _repository.GetPropertyItemByIdAsync(viewModel.PropertyId.Value);
                if (property == null)
                    throw new InvalidOperationException($"Property with ID {viewModel.PropertyId} not found");

                var categoryProperty = await _repository.GetCategoryPropertyAsync(viewModel.CategoryId, viewModel.PropertyId.Value);
                if (categoryProperty != null  && string.IsNullOrWhiteSpace(viewModel.PropertyValue))
                    throw new InvalidOperationException($"Value for required property '{property.PropertyDescription}' is missing");

                if (!string.IsNullOrWhiteSpace(viewModel.PropertyValue))
                {
                    devicePropertyValues.Add(new DevicePropertyValue
                    {
                        PropertyId = viewModel.PropertyId.Value,
                        Value = viewModel.PropertyValue
                    });
                }
            }

            var newDevice = new Device
            {
                DeviceName = viewModel.DeviceName,
                AcquisitionDate = viewModel.AcquisitionDate,
                CategoryId = viewModel.CategoryId,
                SerialNo = viewModel.SerialNo,
                DevicePropertyValues = devicePropertyValues
            };

            await _repository.AddDeviceAsync(newDevice);
        }

        public async Task UpdateDeviceAsync(DeviceVm viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (viewModel.Id <= 0)
                throw new ArgumentException("Invalid device ID");

            if (viewModel.CategoryId <= 0)
                throw new ArgumentException("Invalid category selected");

            if (string.IsNullOrWhiteSpace(viewModel.DeviceName))
                throw new ArgumentException("Device name is required");

            if (string.IsNullOrWhiteSpace(viewModel.SerialNo))
                throw new ArgumentException("Serial number is required");

            var device = await _repository.GetDeviceByIdAsync(viewModel.Id);
            if (device == null)
                throw new InvalidOperationException($"Device with ID {viewModel.Id} not found");

            try
            {
                device.DeviceName = viewModel.DeviceName;
                device.SerialNo = viewModel.SerialNo;
                device.AcquisitionDate = viewModel.AcquisitionDate;
                device.CategoryId = viewModel.CategoryId;
                device.DevicePropertyValues.Clear();

                if (viewModel.PropertyId.HasValue && viewModel.PropertyId > 0)
                {
                    var property = await _repository.GetPropertyItemByIdAsync(viewModel.PropertyId.Value);
                    if (property == null)
                        throw new InvalidOperationException($"Property with ID {viewModel.PropertyId.Value} not found");

                    var categoryProperty = await _repository.GetCategoryPropertyAsync(viewModel.CategoryId, viewModel.PropertyId.Value);
                    if (categoryProperty != null && string.IsNullOrWhiteSpace(viewModel.PropertyValue))
                        throw new InvalidOperationException($"Value for required property '{property.PropertyDescription}' is missing");

                    if (!string.IsNullOrWhiteSpace(viewModel.PropertyValue))
                    {
                        device.DevicePropertyValues.Add(new DevicePropertyValue
                        {
                            DeviceId = device.Id,
                            PropertyId = viewModel.PropertyId.Value,
                            Value = viewModel.PropertyValue
                        });
                    }
                }

                await _repository.UpdateDeviceAsync(device);
            }
            catch (DbUpdateException ex)
            {
                string errorMessage = $"Database update failed: {ex.Message}";
                if (ex.InnerException != null)
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                throw new InvalidOperationException(errorMessage, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating device: {ex.Message}", ex);
            }
        }
        public async Task DeleteDeviceAsync(int id)
        {
            var device = await _repository.GetDeviceByIdAsync(id);

            if (device is null)
            {
                throw new Exception("Device not found");

            }

            await _repository.DeleteDeviceAsync(id); 
        }

        public async Task<DeviceCategoryVm> AddNewCategory(DeviceCategoryVm model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                throw new ArgumentException("Category name is required.", nameof(model.CategoryName));
            }
            

            var category = new DeviceCategory
            {
                CategoryName = model.CategoryName,
                CategoryProperties = model.CategoryProperties?.Select(cp => new DAL.Models.CategoryProperties.CategoryProperty
                {
                    PropertyId = cp.PropertyId,
                }).ToList() ?? new List<DAL.Models.CategoryProperties.CategoryProperty>()
            };

            var addedCategory = await _repository.AddNewCategory(category);
            return new DeviceCategoryVm
            {
                Id = addedCategory.Id,
                CategoryName = addedCategory.CategoryName,
                CategoryProperties = addedCategory.CategoryProperties.Select(cp => new CategoryPropertyVm
                {
                    Id = cp.Id,
                    CategoryId = cp.CategoryId,
                    PropertyId = cp.PropertyId,
                    PropertyName = cp.PropertyItem?.PropertyDescription ?? string.Empty,
                }).ToList()
            };

            


        }

        public async Task<CategoryPropertyVm> AddNewProperty(CategoryPropertyVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.PropertyName))
                throw new ArgumentException("Property name is required.", nameof(model.PropertyName));

            var property = new CategoryProperty
            {
                PropertyId = model.PropertyId, // Assuming PropertyId is set in the model
            };

            var addedProperty = await _repository.AddNewProperty(property);
            return new CategoryPropertyVm
            {
                PropertyId = addedProperty.PropertyId,
                PropertyName = addedProperty.PropertyItem.PropertyDescription
            };
        }

        public async Task<CategoryPropertyVm> AddNewCategoryProperty(CategoryPropertyVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.CategoryId <= 0)
                throw new ArgumentException("Invalid category selected.", nameof(model.CategoryId));
            if (model.PropertyId <= 0)
                throw new ArgumentException("Invalid property selected.", nameof(model.PropertyId));

            var existingProperty = await _repository.GetCategoryPropertyAsync(model.CategoryId, model.PropertyId);
            if (existingProperty != null)
                throw new InvalidOperationException("The property already exists for this category.");

            var categoryProperty = new CategoryProperty
            {
                CategoryId = model.CategoryId,
                PropertyId = model.PropertyId
            };

            var addedCategoryProperty = await _repository.AddCategoryPropertyAsync(categoryProperty);

            // Fetch CategoryName and PropertyName for the response
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId!.Value);
            var property = await _repository.GetPropertyByIdAsync(model.PropertyId!.Value);

            return new CategoryPropertyVm
            {
                Id = addedCategoryProperty!.Id,
                CategoryId = addedCategoryProperty.CategoryId,
                PropertyId = addedCategoryProperty.PropertyId,
                CategoryName = category?.CategoryName ?? string.Empty,
                PropertyName = property?.PropertyDescription ?? string.Empty
            };
        }

        public async Task<List<CategoryPropertyVm>> GetAllCategoryPropertiesAsync()
        {
            var categoryProperties = await _repository.GetAllCategoryPropertiesAsync();
            return categoryProperties.Select(cp => new CategoryPropertyVm
            {
                Id = cp.Id,
                CategoryId = cp.CategoryId,
                PropertyId = cp.PropertyId,
                CategoryName = cp.DeviceCategory?.CategoryName ?? string.Empty,
                PropertyName = cp.PropertyItem?.PropertyDescription ?? string.Empty
            }).ToList();
        }

        public Task DeleteCategoryPropertyByIdAsync(int id)
        {
            var categoryProperty = _repository.GetAllCategoryPropertiesAsync()
                                               .Result
                                               .FirstOrDefault(cp => cp.Id == id);

            if (categoryProperty == null)
                throw new InvalidOperationException("Category property not found.");

            return _repository.DeleteCategoryPropertyByIdAsync(id);
        }

        public async Task<CategoryPropertyVm> UpdateCategoryPropertyAsync(CategoryPropertyVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.Id <= 0)
                throw new ArgumentException("Invalid category property ID.", nameof(model.Id));
            if (model.CategoryId <= 0)
                throw new ArgumentException("Invalid category selected.", nameof(model.CategoryId));
            if (model.PropertyId <= 0)
                throw new ArgumentException("Invalid property selected.", nameof(model.PropertyId));

            // Check for duplicate CategoryId/PropertyId combination
            var existingProperty = await _repository.GetCategoryPropertyAsync(model.CategoryId, model.PropertyId);
            if (existingProperty != null && existingProperty.Id != model.Id)
                throw new InvalidOperationException("The property already exists for this category.");

            var categoryProperty = new CategoryProperty
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                PropertyId = model.PropertyId
            };

            var updatedCategoryProperty = await _repository.UpdateCategoryPropertyAsync(categoryProperty);

            var category = await _repository.GetCategoryByIdAsync(model.CategoryId!.Value);
            var property = await _repository.GetPropertyByIdAsync(model.PropertyId!.Value);

            return new CategoryPropertyVm
            {
                Id = updatedCategoryProperty.Id,
                CategoryId = updatedCategoryProperty.CategoryId,
                PropertyId = updatedCategoryProperty.PropertyId,
                CategoryName = category?.CategoryName ?? string.Empty,
                PropertyName = property?.PropertyDescription ?? string.Empty
            };
        }

        public async Task<CategoryPropertyVm> GetCategoryPropertyByIdAsync(int id)
        {
            var categoryProperty = await _repository.GetCategoryPropertyByIdAsync(id);

            if (categoryProperty == null)
                return null!;

            return new CategoryPropertyVm
            {
                Id = categoryProperty.Id,
                CategoryId = categoryProperty.CategoryId,
                PropertyId = categoryProperty.PropertyId,
                CategoryName = categoryProperty.DeviceCategory?.CategoryName ?? string.Empty,
                PropertyName = categoryProperty.PropertyItem?.PropertyDescription ?? string.Empty
            };
        }

        public async Task AddPropertyAsync(PropertyItemVm viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (string.IsNullOrWhiteSpace(viewModel.PropertyDescription))
                throw new ArgumentException("Property description is required.", nameof(viewModel.PropertyDescription));

            var newProperty = new PropertyItem
            {
                PropertyDescription = viewModel.PropertyDescription
            };

            await _repository.AddPropertyAsync(newProperty);
        }

        public async Task UpdatePropertyAsync(PropertyItemVm viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (viewModel.Id <= 0)
                throw new ArgumentException("Invalid property ID.", nameof(viewModel.Id));

            var property = await _repository.GetPropertyItemByIdAsync(viewModel.Id);

            if (property == null)
                throw new InvalidOperationException($"Property with ID {viewModel.Id} not found.");

            property.PropertyDescription = viewModel.PropertyDescription;
            await _repository.UpdatePropertyAsync(property);
        }

        public async Task DeletePropertyAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid property ID.", nameof(id));

            var propertyItem = await _repository.GetPropertyItemByIdAsync(id);
            if (propertyItem == null)
                throw new InvalidOperationException($"Property with ID {id} not found.");

            await _repository.DeletePropertyAsync(id);
        }

        public async Task<PropertyItemVm> GetPropertyItemByIdAsync(int id)
        {
            var property = await _repository.GetPropertyItemByIdAsync(id);
            if (property == null)
                return null!;

            return new PropertyItemVm
            {
                Id = property.Id,
                PropertyDescription = property.PropertyDescription
            };
        }
    }
}
