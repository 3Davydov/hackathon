using hackathon.contracts;
using hackathon.model;
using hackathon.strategy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace hackathon.tests.tests;

public class HRManagerTest
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
    public void CreateTeams_ShouldReturnCorrectNumberOfTeams()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();

        var strategy = new RandomTeamBuildingStrategy();
        var manager = new HRManager(strategy);

        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();

        Assert.NotNull(teamLeads);
        Assert.NotNull(juniors);

        var teamLeadsWishlists = WishListGenerator.GenerateRandomWishlists(teamLeads, juniors);
        var juniorsWishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        var teams = manager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
        Assert.Equal(20, teams.Count); /* Ensure we have the correct number of teams */
    }
    
    [Fact]
    public void CreateTeams_WithHateStrategy_ShouldReturnPredefinedDistribution()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();

        var strategy = new TeamLeadsHateTheirJuniorsStrategy();
        var manager = new HRManager(strategy);

        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();
        var teamLeadsWishlists = WishListGenerator.GenerateStaticWishlists(teamLeads, juniors);
        var juniorsWishlists = WishListGenerator.GenerateStaticWishlists(juniors, teamLeads);

        var teams = manager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

        for (var i = 0; i < teamLeads.Count; i++)
        {
            var teamLead = teamLeads[i];
            var expectedJunior = juniors[teamLeads.Count - 1 - i]; // Predefined reverse distribution
            Assert.Equal(teamLead.Id, teams[i].TeamLead.Id); // Verify team lead assignment
            Assert.Equal(expectedJunior.Id, teams[i].Junior.Id); // Verify junior assignment
        }
    }

    [Fact]
    public void CreateTeams_WithHateStrategy_ShouldCallStrategyOnce()
    {
        var serviceProvider = BuildServiceProvider();
        var loader = serviceProvider.GetRequiredService<EmployeeLoader>();

        var strategyMock = new Mock<ITeamBuildingStrategy>();
        strategyMock
            .Setup(s => s.BuildTeams(It.IsAny<List<Employee>>(), It.IsAny<List<Employee>>(), It.IsAny<List<WishList>>(), It.IsAny<List<WishList>>()))
            .Returns((List<Employee> teamLeads, List<Employee> juniors, List<WishList> teamLeadsWishlists, List<WishList> juniorsWishlists) =>
            {
                var teams = new List<Team>();
                for (var i = 0; i < teamLeads.Count; i++)
                {
                    teams.Add(new Team(teamLeads[i], juniors[teamLeads.Count - 1 - i]));
                }
                return teams;
            });

        var manager = new HRManager(strategyMock.Object);

        var juniors = loader.LoadJuniors();
        var teamLeads = loader.LoadTeamLeads();
        var teamLeadsWishlists = WishListGenerator.GenerateStaticWishlists(teamLeads, juniors);
        var juniorsWishlists = WishListGenerator.GenerateStaticWishlists(juniors, teamLeads);

        var teams = manager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

        strategyMock.Verify(s => s.BuildTeams(It.IsAny<List<Employee>>(), It.IsAny<List<Employee>>(), It.IsAny<List<WishList>>(), It.IsAny<List<WishList>>()), Times.Once);
    }
}