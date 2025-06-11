using Microsoft.EntityFrameworkCore;
using My.Database.Context;
using My.App.Interfaces;
using My.App.Repos;

namespace My.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register DbContext with SQL Server
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite("Data Source=../My.Database/app.db"));

            // Register Repositories
            builder.Services.AddScoped<IUserRepo, UserRepo>();

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
