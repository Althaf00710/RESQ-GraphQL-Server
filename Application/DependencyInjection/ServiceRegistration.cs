using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            services.AddScoped<ICivilianTypeService, CivilianTypeService>();
            services.AddScoped<ICivilianTypeRequestService, CivilianTypeRequestService>();
            services.AddScoped<IFirstAidCategoryService, FirstAidCategoryService>();
            services.AddScoped<IFirstAidDetailService, FirstAidDetailService>();
            services.AddScoped<IFirstAidService, FirstAidService>();
            services.AddScoped<IRescueVehicleAssignmentService, RescueVehicleAssignmentService>();
            services.AddScoped<IRescueVehicleLocationService, RescueVehicleLocationService>();
            services.AddScoped<IRescueVehicleService, RescueVehicleService>();
            services.AddScoped<IRescueVehicleRequestService, RescueVehicleRequestService>();
            services.AddScoped<IRescueVehicleTypeService, RescueVehicleTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<JwtTokenGenerator>();

            return services;
        }
    }
}
