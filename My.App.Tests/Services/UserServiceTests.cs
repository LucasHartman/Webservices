using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using My.Database.Context;
using My.App.Interfaces;
using My.App.Services;
using My.App.Dtos;
using My.App.Repos;


namespace My.App.Tests.Services;

public class UserServiceTests
{
    private readonly IUserService _UserService;
    private readonly ITestOutputHelper _output;


    public UserServiceTests(ITestOutputHelper output)
    {
        // Build Config
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Register
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddDbContext<MyDbContext>(options =>
            options.UseSqlite("Filename=MyDatabase.db"));
        serviceCollection.AddAutoMapper(typeof(Program)); 
        serviceCollection.AddTransient<IUserRepo, UserRepo>();
        serviceCollection.AddTransient<UserService>();

        // Providers
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _UserService = serviceProvider.GetRequiredService<UserService>();
        _output = output;
    }

    [Fact]
    public async Task GetAllUsersAsyncTest()
    {
        // Arrange, Act
        var result = await _UserService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        // Print list
        foreach (var item in result)
        {
            _output.WriteLine($"Id: {item.Id}");
        }
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Services.UserRepoTests.GetAllUsersAsyncTest" --logger "console;verbosity=detailed"
  
    [Fact]
    public async Task GetUserByIdAsyncTest()
    {
        // Arrange
        int id = 952904;

        // Act
        var result = await _UserService.GetUserByIdAsync(id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, id);
        _output.WriteLine($"ID: {result.Id}");
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Services.UserServiceTests.GetUserByIdAsyncTest" --logger "console;verbosity=detailed"

    [Fact]
    public async Task GetUsersByDateRangeAsyncTest()
    {
        // Arrange
        DateTime startDate = new DateTime(2024, 01, 01);
        DateTime endDate = DateTime.Now;

        // Act
        var result = await _UserService.GetUsersByCreatedAtAsync(startDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        // Print list
        foreach (var item in result)
        {
            //_output.WriteLine($"Id: {item.Id}, Date: {item.CreatedAt.ToString("yyyy-MM-dd") ?? "N/A"}");
        }
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Services.UserRepoTests.GetUsersByDateRangeAsyncTest" --logger "console;verbosity=detailed"

    [Fact]
    public async Task InsertUserAsyncTest()
    {
        // Arrange
        var User = new UserFormDto
        {
             Id = 1
        };


        // Act
        var result = await _UserService.InsertUserAsync(User);

        // Assert
        Assert.NotNull(result);

        // Print result
        //_output.WriteLine($"Inserted User - Id: {result.Id}, Date: {result.CreatedAt.ToString("yyyy-MM-dd") ?? "N/A"}");
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Services.UserServiceTests.InsertUserAsyncTest" --logger "console;verbosity=detailed"

    [Fact]
    public async Task UpdateUserAsyncTest()
    {
        // Arrange
        var User = new UserFormDto
        {
            Id = 1
        };

        // Act
        var result = await _UserService.UpdateUserAsync(User.Id, User);

        // Assert
        Assert.NotNull(result);
        // Print result
        //_output.WriteLine($"Updated User - Id: {result.Id}, Date: {result.CreatedAt.ToString("yyyy-MM-dd") ?? "N/A"}");
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Services.UserServiceTests.UpdateUserAsyncTest" --logger "console;verbosity=detailed"

} // dotnet test --filter "FullyQualifiedName~My.App.Tests.Services.UserServiceTests"
