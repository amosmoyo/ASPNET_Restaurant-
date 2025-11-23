using Restaurant.Infrastucture.Extension;
using Restaurant.Infrastucture.Seeders;
using Restaurant.Application.Extension;
using Serilog;
using Serilog.Events;
using Restaurant.Middleware;
using Restaurant.Domain.Entities;
using Restaurant.Infrastucture.Persistence;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Restaurant.Infrastucture.Authorization;

var builder = WebApplication.CreateBuilder(args);

//add serilogs
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.File("log/log-.txt", 
//        rollingInterval:RollingInterval.Day, 
//        fileSizeLimitBytes: 10_000_000,       // 10 MB max per file
//        rollOnFileSizeLimit: true,            // create new file when limit reached
//        retainedFileCountLimit: 7,            // keep logs for last 7 days/files
//        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] (Thread {ThreadId}) |{SourceContext}| {NewLine} {Message:lj}{NewLine}{Exception}",
//        shared: true)
//    .MinimumLevel.Debug()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
//    .CreateLogger();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

//HttpContext Accessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[]{ }
        }

    });
});

builder.Services.AddInfrastucture(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddTransient<RequestTimeMiddleware>();

builder.Host.UseSerilog();

// ? Identity (Minimal API endpoints, backed by EF)
builder.Services.AddIdentityApiEndpoints<User>()
        .AddRoles<IdentityRole>()
        .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
        .AddEntityFrameworkStores<RestaurantsDbContext>();

//builder.Services.AddAuthorizationBuilder().AddPolicy(PolicyNames.HasNationality, policy => policy.RequireClaim(PolicyAttributes.Nationality));


var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

    await seeder.Seed();   // Seed the database
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v11");
        options.RoutePrefix = "swagger"; // UI at /swagger
    });
}

app.
    MapGroup("api/users").
    WithTags("Users").
    MapIdentityApi<User>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
