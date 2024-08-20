using AllCompaniesAPI.Data;
using AllCompaniesAPI;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MySql;
using System.Configuration;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Swagger Documentation Section
        var info = new OpenApiInfo()
        {
            Title = "Company API",
            Version = "v1",
            Description = "an API to Manage Companies",
            Contact = new OpenApiContact()
            {
                Name = "Baris Cem ",
                Email = "bariscemyilmaz22@gmial.com",
            }
        };
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", info);

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

var connectionString = $"server=localhost;port=3306;database=all_companies;user=root;password=1234c4321c";
builder.Services.AddDbContext<DataContext>(a => a.UseMySQL(connectionString));

builder.Services.AddHangfire(config =>
{
    config.UseStorage(
           new MySqlStorage(
               
               "server=127.0.0.1;uid=root;pwd=1234c4321c;database=all_companies;Allow User Variables=True",
               new MySqlStorageOptions
               {
                   TablesPrefix = "Hangfire"
               }
           )
       );
});

builder.Services.AddScoped<ICompaniesJob, CompaniesJob>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection"), true);
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(u =>
    {
        u.RouteTemplate = "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Company API");
    });
}
app.UseHangfireServer();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<ICompaniesJob>("fetch data", (x) => x.ExecuteAsync(), Cron.Hourly(4));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
