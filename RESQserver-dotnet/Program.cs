using Application.DependencyInjection;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using RESQserver_dotnet.Api;
using RESQserver_dotnet.Api.CivilianApi;
using RESQserver_dotnet.Api.CivilianLocationApi;
using RESQserver_dotnet.Api.CivilianStatusApi;
using RESQserver_dotnet.Api.CivilianStatusRequestApi;
using RESQserver_dotnet.Api.EmergencyCategoryApi;
using RESQserver_dotnet.Api.EmergencySubCategoryApi;
using RESQserver_dotnet.Api.EmergencyToCivilianApi;
using RESQserver_dotnet.Api.EmergencyToVehicleApi;
using RESQserver_dotnet.Api.FirstAidDetailApi;
using RESQserver_dotnet.Api.RescueVehicleApi;
using RESQserver_dotnet.Api.RescueVehicleAssignmentApi;
using RESQserver_dotnet.Api.RescueVehicleCategoryApi;
using RESQserver_dotnet.Api.RescueVehicleLocationApi;
using RESQserver_dotnet.Api.RescueVehicleRequestApi;
using RESQserver_dotnet.Api.RescueVehicleTypeApi;
using RESQserver_dotnet.Api.SnakeApi;
using RESQserver_dotnet.Api.UserApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ResqDB"),
        sql =>
        {
            sql.MigrationsAssembly("Infrastructure");
            sql.UseNetTopologySuite();
        }
    )
);

builder.Services.AddSingleton<GeometryFactory>(
    NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326)
);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddMemoryCache();

//var config = builder.Configuration;

//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = config["Jwt:Issuer"],
//            ValidAudience = config["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
//        };

//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = ctx =>
//            {
//                ctx.Token = ctx.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//                return Task.CompletedTask;
//            }
//        };
//    });

//builder.Services.AddAuthorization();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowLocalhost3000",
//        policy => policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "https://rotten-eyes-bathe.loca.lt")
//                        .AllowAnyHeader()
//                        .AllowAnyMethod());
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});


builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddInMemorySubscriptions()
    .ModifyCostOptions(o => o.EnforceCostLimits = false)
    .AddSpatialTypes()

    .AddQueryType<RESQserver_dotnet.Api.Query>()
    .AddTypeExtension<UserQuery>()
    .AddTypeExtension<CivilianStatusQuery>()
    .AddTypeExtension<CivilianQuery>()
    .AddTypeExtension<CivilianStatusRequestQuery>()
    .AddTypeExtension<CivilianLocationQuery>()
    .AddTypeExtension<EmergencyCategoryQuery>()
    .AddTypeExtension<EmergencySubCategoryQuery>()
    .AddTypeExtension<EmergencyToCivilianQuery>()
    .AddTypeExtension<EmergencyToVehicleQuery>()
    .AddTypeExtension<FirstAidDetailQuery>()
    .AddTypeExtension<RescueVehicleCategoryQuery>()
    .AddTypeExtension<RescueVehicleQuery>()
    .AddTypeExtension<RescueVehicleLocationQuery>()
    .AddTypeExtension<RescueVehicleRequestQuery>()
    .AddTypeExtension<RescueVehicleAssignmentQuery>()
    .AddTypeExtension<SnakeQuery>()

    .AddMutationType<Mutation>()
    .AddTypeExtension<UserMutation>()
    .AddTypeExtension<CivilianStatusMutation>()
    .AddTypeExtension<CivilianMutation>()
    .AddTypeExtension<CivilianStatusRequestMutation>()
    .AddTypeExtension<CivilianLocationMutation>()
    .AddTypeExtension<EmergencyCategoryMutation>()
    .AddTypeExtension<EmergencySubCategoryMutation>()
    .AddTypeExtension<EmergencyToCivilianMutation>()
    .AddTypeExtension<EmergencyToVehicleMutation>()
    .AddTypeExtension<FirstAidDetailMutation>()
    .AddTypeExtension<RescueVehicleCategoryMutation>()
    .AddTypeExtension<RescueVehicleMutation>()
    .AddTypeExtension<RescueVehicleLocationMutation>()
    .AddTypeExtension<RescueVehicleRequestMutation>()
    .AddTypeExtension<RescueVehicleAssignmentMutation>()
    .AddTypeExtension<SnakeMutation>()

    .AddSubscriptionType<Subscription>()
    .AddTypeExtension<RescueVehicleSubscription>()
    .AddTypeExtension<RescueVehicleLocationSubscription>()

    .AddType<UserType>()
    .AddType<CivilianStatusType>()
    .AddType<CivilianType>()
    .AddType<CivilianStatusRequestType>()
    .AddType<CivilianLocationType>()
    .AddType<EmergencyCategoryType>()
    .AddType<EmergencySubCategoryType>()
    .AddType<EmergencyToCivilianType>()
    .AddType<EmergencyToVehicleType>()
    .AddType<FirstAidDetailType>()
    .AddType<RescueVehicleCategoryType>()
    .AddType<RescueVehicleType>()
    .AddType<RescueVehicleLocationType>()
    .AddType<RescueVehicleRequestType>()
    .AddType<RescueVehicleAssignmentType>()
    .AddType<Snake>()
    .AddType<UploadType>();

var app = builder.Build();

app.UseStaticFiles();

app.UseWebSockets();

app.MapGraphQL();

app.UseCors("AllowAll");

app.Run();
