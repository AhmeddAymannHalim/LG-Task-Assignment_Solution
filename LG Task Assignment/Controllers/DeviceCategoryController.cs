using LGTask.Assignment.BLL.Services;
using LGTask.Assignment.BLL.ViewModels.CategoryProperty;
using LGTask.Assignment.BLL.ViewModels.CreateDevices;
using LGTask.Assignment.BLL.ViewModels.DeviceCategory;
using LGTask.Assignment.BLL.ViewModels.DevicePropertyValues;
using LGTask.Assignment.BLL.ViewModels.PropertyItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Imaging;

namespace LG_Task_Assignment.Controllers
{
    public class DeviceCategoryController : Controller
    {
        private readonly IDeviceService _service;

        public DeviceCategoryController(IDeviceService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _service.GetAllDevicesAsync();

            return View(devices);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new DeviceVm();

            var categories = await _service.GetAllCategoriesAsync(); // Populate Categories Dropdown
            var properties = await _service.GetAllPropertiesAsync(); // Populate Properties Dropdown
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName",model.CategoryId);
            ViewBag.Properties = new SelectList(properties, "Id", "PropertyName",model.PropertyId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DeviceVm model)
        {

            if (!ModelState.IsValid)
            {
                var categories = await _service.GetAllCategoriesAsync();
                var properties = await _service.GetAllPropertiesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", model.CategoryId);
                ViewBag.Properties = new SelectList(properties, "Id", "PropertyName", model.PropertyId);
                return View();
            }

            try
            {
                await _service.AddDeviceAsync(model);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var categories = await _service.GetAllCategoriesAsync();
                var properties = await _service.GetAllPropertiesAsync();
                ViewBag.Properties = new SelectList(properties, "Id", "PropertyName",model.PropertyId);
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName",model.CategoryId);

                return View(model);
            }


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return NotFound();

            var device = await _service.GetDeviceByIdAsync(id);
            if (device == null)
                return NotFound();

            var categories = await _service.GetAllCategoriesAsync();
            var properties = await _service.GetAllPropertiesAsync();    
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName",device.CategoryId);
            ViewBag.Properties = new SelectList(properties, "Id", "PropertyName",device.PropertyId);

            return View(device);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DeviceVm model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _service.GetAllCategoriesAsync();
                var properties = await _service.GetAllPropertiesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", model.CategoryId);
                ViewBag.Properties = new SelectList(properties, "Id", "PropertyName", model.PropertyId);
                return View(model);
            }

            try
            {
                await _service.UpdateDeviceAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var categories = await _service.GetAllCategoriesAsync();
                var properties = await _service.GetAllPropertiesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", model.CategoryId);
                ViewBag.Properties = new SelectList(properties, "Id", "PropertyName", model.PropertyId);
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return NotFound();
            try
            {
                await _service.DeleteDeviceAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertyDetails(int propertyId) // AJAX Call to get Property Details based on PropertyId 
        {
            try
            {
                var property = await _service.GetPropertyByIdAsync(propertyId);
                if (property == null)
                    return Json(null);

                return Json(new
                {
                    id = property.Id,
                    propertyId = property.PropertyId,
                    propertyName = property.PropertyName
                });
            }
            catch (Exception)
            {
                return Json(null);
            }
        }


        [HttpGet]
        public IActionResult CreateCategory()
        {
            var model = new DeviceCategoryVm
            {
                CategoryProperties = new List<CategoryPropertyVm>()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateCategory(DeviceCategoryVm model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join("; ", errors) });
            }

            var newCategory = new DeviceCategoryVm
            {
                CategoryName = model.CategoryName
            };
            _service.AddNewCategory(newCategory);

            return RedirectToAction(nameof(Create));
        }


        [HttpPost]
        public IActionResult CreateProperty(CategoryPropertyVm model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join("; ", errors) });
            }

            var newProperty = new CategoryPropertyVm
            {
                PropertyId = model.PropertyId,
                PropertyName = model.PropertyName
            };
                _service.AddNewProperty(newProperty);

            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public async Task<IActionResult> AddCategoryProperty()
        {
            var model = new CategoryPropertyVm();
            var categories = await _service.GetAllCategoriesAsync();
            var properties = await _service.GetAllPropertiesAsync();
            var categoryProperties = await _service.GetAllCategoryPropertiesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.Properties = new SelectList(properties, "Id", "PropertyName");
            ViewBag.CategoryProperties = categoryProperties;
            return View(model);
        }
        public async Task<IActionResult> GetAllCategoryProperties()
        {
            var categoryProperties = await _service.GetAllCategoryPropertiesAsync();
            return View(categoryProperties);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryProperty(CategoryPropertyVm model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join("; ", errors) });
            }

            try
            {
                var addedCategoryProperty = await _service.AddNewCategoryProperty(model);
                var displayName = $"{addedCategoryProperty.CategoryName} - {addedCategoryProperty.PropertyName}";
                return RedirectToAction(nameof(GetAllCategoryProperties));
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> DeleteCategoryProperty(int id)
        {
            if (id <= 0)
                return NotFound();
            try
            {
                await _service.DeleteCategoryPropertyByIdAsync(id);
                return RedirectToAction(nameof(GetAllCategoryProperties));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCategoryProperty(int id)
        {
            var categoryProperty = await _service.GetCategoryPropertyByIdAsync(id);
            if (categoryProperty == null)
                return NotFound();

            var categories = await _service.GetAllCategoriesAsync();
            var properties = await _service.GetAllPropertiesAsync();
            var categoryProperties = await _service.GetAllCategoryPropertiesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName", categoryProperty.CategoryId);
            ViewBag.Properties = new SelectList(properties, "Id", "PropertyName", categoryProperty.PropertyId);
            ViewBag.CategoryProperties = categoryProperties;
            return View(categoryProperty);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategoryProperty(CategoryPropertyVm model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join("; ", errors) });
            }

            try
            {
                var updatedCategoryProperty = await _service.UpdateCategoryPropertyAsync(model);
                var displayName = $"{updatedCategoryProperty.CategoryName} - {updatedCategoryProperty.PropertyName}";
                return RedirectToAction(nameof(GetAllCategoryProperties));
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _service.GetAllPropertiesAsync();

            var vm = properties.Select(p => new PropertyItemVm
            {
                Id = p.Id,
                PropertyDescription = p.PropertyName,
            });

            return View(vm);
        }
        [HttpGet("DeviceCategory/CreateProperty")]
        public IActionResult CreateProperty()
        {
            var model = new PropertyItemVm();
            return View(model);
        }


        [HttpPost("DeviceCategory/CreateProperty")]
        public async Task<IActionResult> CreateProperty(PropertyItemVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _service.AddPropertyAsync(model);
                TempData["Success"] = "Property added successfully.";
                return RedirectToAction(nameof(GetAllProperties));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            var model = await _service.GetPropertyItemByIdAsync(id);
            if (model == null)
            {
                TempData["Error"] = "Property not found.";
                return RedirectToAction(nameof(GetAllProperties));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProperty(PropertyItemVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _service.UpdatePropertyAsync(model);
                TempData["Success"] = "Property updated successfully.";
                return RedirectToAction(nameof(GetAllProperties)); 
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }


        public async Task<IActionResult> DeleteProperty(int id)
        {

            if (id <= 0)
                return NotFound();
            try
            {
                await _service.DeletePropertyAsync(id);
                return RedirectToAction(nameof(GetAllProperties));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
