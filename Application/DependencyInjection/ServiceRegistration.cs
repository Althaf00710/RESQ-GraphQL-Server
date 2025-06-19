using System.Reflection;
using Application.Services;
using Application.Utils;
using Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace Application.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICivilianService, CivilianService>();
            services.AddScoped<ICivilianLocationService, CivilianLocationService>();
            services.AddScoped<ICivilianStatusService, CivilianStatusService>();
            services.AddScoped<ICivilianStatusRequestService, CivilianStatusRequestService>();

            services.AddScoped<IEmergencyCategoryService, EmergencyCategoryService>();
            services.AddScoped<IEmergencySubCategoryService, EmergencySubCategoryService>();

            services.AddScoped<IFirstAidDetailService, FirstAidDetailService>();
            services.AddScoped<IRescueVehicleAssignmentService, RescueVehicleAssignmentService>();
            services.AddScoped<IRescueVehicleLocationService, RescueVehicleLocationService>();
            services.AddScoped<IRescueVehicleService, RescueVehicleService>();
            services.AddScoped<IRescueVehicleRequestService, RescueVehicleRequestService>();
            services.AddScoped<IRescueVehicleCategoryService, RescueVehicleCategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<JwtTokenGenerator>();

            return services;
        }
    }
}
