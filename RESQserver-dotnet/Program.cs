using System.Text;
using Application.DependencyInjection;
using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RESQserver_dotnet.Api;
using RESQserver_dotnet.Api.CivilianApi;
using RESQserver_dotnet.Api.CivilianStatusApi;
using RESQserver_dotnet.Api.CivilianStatusRequestApi;
using RESQserver_dotnet.Api.CivilianType;
using RESQserver_dotnet.Api.UserApi;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
    .AddMutationType<Mutation>()
    .AddTypeExtension<UserMutation>()
    .AddTypeExtension<CivilianStatusMutation>()
    .AddTypeExtension<CivilianMutation>()
    .AddTypeExtension<CivilianStatusRequestMutation>()
    .AddType<UserType>()
    .AddType<CivilianStatusType>()
    .AddType<CivilianType>()
    .AddType<CivilianStatusRequestType>()
    .AddType<UploadType>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGraphQL();

app.Run();
