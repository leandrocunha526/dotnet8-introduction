using System.Text.Json.Serialization;
using dotnet8_introduction.Data;
using dotnet8_introduction.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using dotnet8_introduction.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add environment variables
//bool isDevelopment = builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Testing";

// Analyze the environment and choose the appropriate database
//if (isDevelopment)
//{
// Use SQL Server in development
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))
);
//}
//else
/*{
    // Use SQLite in production or testing
    builder.Services.AddDbContext<DataContext>(options =>
       options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"))
   );
}
*/

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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var userId = int.Parse(context.Principal.Identity.Name);
            var user = userService.GetById(userId);
            if (user == null)
            {
                context.Fail("Error 401: Unauthorized");
                context.HttpContext.Response.StatusCode = 401;
                // Should return a task with the failure result when the user is not found
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    };
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet8_introduction v1"));
}

app.UseRouting(); // Add this line to register EndpointRoutingMiddleware

// Place the UseAuthorization and UseAuthentication before UseEndpoints
app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
