using Application.DependencyInjection;
using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.UserApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("Testing"));

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<UserQuery>()
    .AddType<UserType>()
    .AddMutationType<UserMutation>()
    .AddType<UploadType>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGraphQL();

app.Run();
