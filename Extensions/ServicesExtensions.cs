using DeviceSystem.Repositories.AuthRepository;
using DeviceSystem.Repositories.DepartmentRepository;
using DeviceSystem.Repositories.DeviceLoanRepository;
using DeviceSystem.Repositories.DeviceRepository;
using DeviceSystem.Repositories.DeviceTypeRepository;
using DeviceSystem.Repositories.EmployeeRespository;
using DeviceSystem.Repositories.OfficeRepository;
using DeviceSystem.Repositories.PeopleRepository;

using DeviceSystem.Services.PersonService;
using DeviceSystem.Services.AuthService;
using DeviceSystem.Services.DepartmentService;
using DeviceSystem.Services.DeviceLoanService;
using DeviceSystem.Services.DeviceService;
using DeviceSystem.Services.DeviceTypeService;
using DeviceSystem.Services.EmployeeService;
using DeviceSystem.Services.OfficeService;



using Microsoft.AspNetCore.Authentication.Cookies;

namespace DeviceSystem.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<IEmployeeRespository, EmployeeRespository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceTypeRepository, DeviceTypeRepository>();
            services.AddScoped<IDeviceLoanRepository, DeviceLoanRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddScoped<IPeopleService, PeopleService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IDeviceTypeService, DeviceTypeService>();
            services.AddScoped<IDeviceLoanService, DeviceLoanService>();
            services.AddScoped<IAuthService, AuthService>();
            
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
                    options.UseMySQL(connectionString));
            return services;
        }
        
        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Users/Login";
                    option.AccessDeniedPath = "/Home/AccessDenied";
                    option.LogoutPath = "/Home/Logout";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    option.SlidingExpiration = true;

                    option.Cookie.Name = "datalicious";
                    option.Cookie.SecurePolicy = CookieSecurePolicy.Always; //set this to none in development mode
                    option.Cookie.SameSite = SameSiteMode.Strict;  //set this to strict for single site and Lax is need to support OAuth authentication
                    //option.Cookie.HttpOnly = true; //uncomment it in production and comment it on development mode
                    option.Cookie.IsEssential = true;
                    option.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                });

            return services;
        }
    }
}
