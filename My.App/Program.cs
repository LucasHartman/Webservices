using Microsoft.EntityFrameworkCore;
using My.Database.Context;
using My.App.Interfaces;
using My.App.Mappings;
using My.App.Repos;
using My.App.Services;

namespace My.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Read the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register DbContext with SQL Server
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite("Filename=App.db"));

            // Register Repositories
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            
            // Register Services
            builder.Services.AddScoped<IUserService, UserService>();

            // Register Providers
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register Controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            // Use HTTPS Redirection
            app.UseHttpsRedirection();

            // Map Controllers for routing
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
