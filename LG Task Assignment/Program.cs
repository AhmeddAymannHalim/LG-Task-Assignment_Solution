using LGTask.Assignment.BLL.Services;
using LGTask.Assignment.DAL.Persistance.Data;
using LGTask.Assignment.DAL.Persistance.Repositories.DeviceCategory;
using LGTask.Assignment.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace LG_Task_Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region ConfigureServices

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ITDepartmentDbContext>(options =>
            {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDeviceService , DeviceService>();
            builder.Services.AddScoped<IDeviceRepository, DeviceCategoryRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
