using Application.DependencyInjection;
using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api;
using RESQserver_dotnet.Api.CivilianApi;
using RESQserver_dotnet.Api.CivilianLocationApi;
using RESQserver_dotnet.Api.CivilianStatusApi;
using RESQserver_dotnet.Api.CivilianStatusRequestApi;
using RESQserver_dotnet.Api.EmergencyCategoryApi;
using RESQserver_dotnet.Api.RescueVehicleApi;
using RESQserver_dotnet.Api.RescueVehicleCategoryApi;
using RESQserver_dotnet.Api.RescueVehicleLocationApi;
using RESQserver_dotnet.Api.RescueVehicleTypeApi;
using RESQserver_dotnet.Api.UserApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ResqDB"), sql => sql.MigrationsAssembly("Infrastructure")));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("ResqInMemoryDB"));

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

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

builder.Services
    .AddGraphQLServer()

    .AddQueryType<RESQserver_dotnet.Api.Query>()
    .AddTypeExtension<UserQuery>()
    .AddTypeExtension<CivilianStatusQuery>()
    .AddTypeExtension<CivilianQuery>()
    .AddTypeExtension<CivilianStatusRequestQuery>()
    .AddTypeExtension<CivilianLocationQuery>()
    .AddTypeExtension<EmergencyCategoryQuery>()
    .AddTypeExtension<RescueVehicleCategoryQuery>()
    .AddTypeExtension<RescueVehicleQuery>()
    .AddTypeExtension<RescueVehicleLocationQuery>()

    .AddMutationType<Mutation>()
    .AddTypeExtension<UserMutation>()
    .AddTypeExtension<CivilianStatusMutation>()
    .AddTypeExtension<CivilianMutation>()
    .AddTypeExtension<CivilianStatusRequestMutation>()
    .AddTypeExtension<CivilianLocationMutation>()
    .AddTypeExtension<EmergencyCategoryMutation>()
    .AddTypeExtension<RescueVehicleCategoryMutation>()
    .AddTypeExtension<RescueVehicleMutation>()
    .AddTypeExtension<RescueVehicleLocationMutation>()

    .AddType<UserType>()
    .AddType<CivilianStatusType>()
    .AddType<CivilianType>()
    .AddType<CivilianStatusRequestType>()
    .AddType<CivilianLocationType>()
    .AddType<EmergencyCategoryType>()
    .AddType<RescueVehicleCategoryType>()
    .AddType<RescueVehicleType>()
    .AddType<RescueVehicleLocationType>()
    .AddType<UploadType>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGraphQL();

app.Run();
