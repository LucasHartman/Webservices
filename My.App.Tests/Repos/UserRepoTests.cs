using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using My.Database.Context;
using My.Database.Models;
using My.App.Interfaces;
using My.App.Repos;


namespace My.App.Tests.Repo;

public class UserRepoTests
{
    private readonly IUserRepo _UserRepo;
    private readonly ITestOutputHelper _output;


    public UserRepoTests(ITestOutputHelper output)
    {
        // Build Config
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Register
        var RepoCollection = new ServiceCollection();
        RepoCollection.AddSingleton<IConfiguration>(configuration);
        RepoCollection.AddDbContext<MyDbContext>(options =>
            options.UseSqlite("Filename=MyDatabase.db"));
        RepoCollection.AddTransient<UserRepo>();

        // Providers
        var RepoProvider = RepoCollection.BuildServiceProvider();
        _UserRepo = RepoProvider.GetRequiredService<UserRepo>();
        _output = output;
    }

  
    [Fact]
    public async Task GetAllUsersAsyncTest()
    {
        // Arrange, Act
        var result = (List<User>)await _UserRepo.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        // Print list
        foreach (var item in result)
        {
            _output.WriteLine($"Id: {item.Id}");
        }
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Repo.UserRepoTests.GetAllUsersAsyncTest" --logger "console;verbosity=detailed"



    [Fact]
    public async Task GetUserByIdAsyncTest()
    {
        // Arrange
        int id = 952904;

        // Act
        User result = await _UserRepo.GetUserByIdAsync(id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, id);
        _output.WriteLine($"ID: {result.Id}");
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Repo.UserRepoTests.GetUserByIdAsyncTest" --logger "console;verbosity=detailed"


    [Fact]
    public async Task GetUsersByDateRangeAsyncTest()
    {
        // Arrange
        DateTime startDate = new DateTime(2024, 01, 01);
        DateTime endDate = DateTime.Now;

        // Act
        var result = (List<User>)await _UserRepo.GetUsersByCreatedAtAsync(startDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        // Print list
        foreach (var item in result)
        {
            _output.WriteLine($"Id: {item.Id}, Date: {item.CreatedAt.ToString("yyyy-MM-dd") ?? "N/A"}");
        }
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Repo.UserRepoTests.GetUsersByDateRangeAsyncTest" --logger "console;verbosity=detailed"
    
    [Fact]
    public async Task InsertUserAsyncTest()
    {
        // Arrange
        var User = new User
        {
            Id = 1,
            CreatedAt = DateTime.Now,
            FirstName = "Jim",
            LastName = "Hoffman",
            Email = "jim.hoffman@example.me"
        };


        // Act
        var result = await _UserRepo.InsertUserAsync(User);

        // Assert
        Assert.NotNull(result);

        // Print result
        _output.WriteLine($"Inserted User - Id: {result.Id}, CreatedAt: {result.CreatedAt.ToString("yyyy-MM-dd") ?? "N/A"}");
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Repo.UserRepoTests.InsertUserAsyncTest" --logger "console;verbosity=detailed"

    [Fact]
    public async Task UpdateUserAsyncTest()
    {
        // Arrange
        var updatedUser = new User
        {
            Id = 1,
            FirstName = "Jim",
            LastName = "Hoffman",
            Email = "jim.hoffman@example.me"
        };

        // Act
        var result = await _UserRepo.UpdateUserAsync(updatedUser);

        // Assert
        Assert.NotNull(result);

        // Print result
        _output.WriteLine($"Updated User - Id: {result.Id}, CreatedAt: {result.CreatedAt.ToString("yyyy-MM-dd") ?? "N/A"}");
    } // dotnet test --filter "FullyQualifiedName=My.App.Tests.Repo.UserRepoTests.UpdateUserAsyncTest" --logger "console;verbosity=detailed"
}
