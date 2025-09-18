using AutoMapper;
using LGTask.Assignment.BLL.ViewModels.CreateDevices;
using LGTask.Assignment.BLL.ViewModels.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.DevicePropertyValues;
using LGTask.Assignment.DAL.Models.Devices;

namespace LG_Task_Assignment.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Device, DeviceVm>().ReverseMap();

            CreateMap<DevicePropertyValue,PropertyValuesVm>().ReverseMap();


        }
    }
}
