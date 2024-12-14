using hackathon.model;
using hackathon.strategy;
using Xunit;

namespace hackathon.tests;

public class HRManagerTest
{
    [Fact]
    public void CreateTeams_ShouldReturnCorrectNumberOfTeams()
    {
        var strategy = new RandomTeamBuildingStrategy();
        var manager = new HRManager(strategy);

        var juniors = HackathonWorker.LoadJuniors();
        var teamLeads = HackathonWorker.LoadTeamLeads();
        var teamLeadsWishlists = HackathonWorker.GenerateRandomWishlists(teamLeads, juniors);
        var juniorsWishlists = HackathonWorker.GenerateRandomWishlists(juniors, teamLeads);

        var teams = manager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
        Assert.Equal(20, teams.Count); // Ensure we have the correct number of teams
    }
}