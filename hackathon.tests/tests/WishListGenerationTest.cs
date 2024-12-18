using hackathon.model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace hackathon.tests.tests;

public class WishListGenerationTest
{
    public static IConfiguration Configuration { get; private set; }
    
    private ServiceProvider BuildServiceProvider()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();

        var services = new ServiceCollection();
        services.Configure<EmployeeLoaderOptions>(configuration.GetSection("EmployeeLoader"));
        services.AddSingleton<EmployeeLoader>();

        return services.BuildServiceProvider();
    }
    
    [Fact]
    public void Wishlist_Size_ShouldBeEqualToParticipants()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();
        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);
        Assert.Equal(20, wishlists.Count); /* Should match participant count */
        Assert.All(wishlists, w => Assert.NotNull(w)); /* Ensure no null wishlists exist */
    }
    
    [Fact]
    public void AllJuniors_ShouldBeAssignedToAWishlist()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();
        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        foreach (var junior in juniors)
        {
            Assert.Contains(wishlists, w => w.DesiredEmployees.Contains(junior.Id));
        }
    }
    
    [Fact]
    public void AllTeamLeads_ShouldBeAssignedToAWishlist()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();
        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        foreach (var teamLead in teamLeads)
        {
            Assert.Contains(wishlists, w => w.EmployeeId == teamLead.Id);
        }
    }
    
    [Fact]
    public void NoWishlist_ShouldNotHaveNullValues()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();
        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        foreach (var wishlist in wishlists)
        {
            Assert.NotNull(wishlist);
        }
    }
}