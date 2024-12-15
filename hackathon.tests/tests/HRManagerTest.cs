using hackathon.contracts;
using hackathon.model;
using hackathon.strategy;
using Moq;
using Xunit;

namespace hackathon.tests.tests;

public class HRManagerTest
{
    [Fact]
    public void CreateTeams_ShouldReturnCorrectNumberOfTeams()
    {
        var strategy = new RandomTeamBuildingStrategy();
        var manager = new HRManager(strategy);

        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();
        var teamLeadsWishlists = WishListGenerator.GenerateRandomWishlists(teamLeads, juniors);
        var juniorsWishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        var teams = manager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
        Assert.Equal(20, teams.Count); /* Ensure we have the correct number of teams */
    }
    
    [Fact]
    public void CreateTeams_WithHateStrategy_ShouldReturnPredefinedDistribution()
    {
        var strategy = new TeamLeadsHateTheirJuniorsStrategy();
        var manager = new HRManager(strategy);

        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();
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

        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();
        var teamLeadsWishlists = WishListGenerator.GenerateStaticWishlists(teamLeads, juniors);
        var juniorsWishlists = WishListGenerator.GenerateStaticWishlists(juniors, teamLeads);

        var teams = manager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

        strategyMock.Verify(s => s.BuildTeams(It.IsAny<List<Employee>>(), It.IsAny<List<Employee>>(), It.IsAny<List<WishList>>(), It.IsAny<List<WishList>>()), Times.Once);
    }
}