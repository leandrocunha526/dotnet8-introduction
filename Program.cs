using System.Text.Json.Serialization;
using dotnet8_introduction.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))
);

// Add services to the container.

// Configure JSON serialization to ignore circular references in the response.
// This helps prevent serialization exceptions when dealing with entities that have cyclic relationships.
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerDoc =>
    swaggerDoc.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ASP.NET Core 8 introduction",
        Version = "v1"
    }
    )
);

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddCors();

var app = builder.Build();

// HTTPS redirection
//app.UseHttpsRedirection();

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet8_introduction v1"));
}

app.UseRouting();

//app.UseAuthorization();
//app.UseAuthentication();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.Run();
