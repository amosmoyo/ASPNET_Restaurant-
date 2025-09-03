using Restaurant.Infrastucture.Extension;
using Restaurant.Infrastucture.Seeders;
using Restaurant.Application.Extension;
using Serilog;
using Serilog.Events;

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

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();

builder.Services.AddInfrastucture(builder.Configuration);

builder.Services.AddApplication();

builder.Host.UseSerilog();


var app = builder.Build();

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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = "swagger"; // UI at /swagger
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
