using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Core.Repositories.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.DependencyInjection
{
    public static class RepositoryRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICivilianRepository, CivilianRepository>();
            services.AddScoped<ICivilianLocationRepository, CivilianLocationRepository>();
            services.AddScoped<ICivilianStatusRepository, CivilianStatusRepository>();
            services.AddScoped<ICivilianStatusRequestRepository, CivilianStatusRequestRepository>();
            services.AddScoped<IFirstAidCategoryRepository, FirstAidCategoryRepository>();
            services.AddScoped<IFirstAidDetailRepository, FirstAidDetailRepository>();
            services.AddScoped<IFirstAidRepository, FirstAidRepository>();
            services.AddScoped<IRescueVehicleAssignmentRepository, RescueVehicleAssignmentRepository>();
            services.AddScoped<IRescueVehicleLocationRepository, RescueVehicleLocationRepository>();
            services.AddScoped<IRescueVehicleRepository, RescueVehicleRepository>();
            services.AddScoped<IRescueVehicleRequestRepository, RescueVehicleRequestRepository>();
            services.AddScoped<IRescueVehicleCategoryRepository, RescueVehicleCategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
