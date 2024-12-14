using hackathon.contracts;

namespace hackathon.strategy;

public class RandomTeamBuildingStrategy
{
    public List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors,
                                        List<WishList> teamLeadsWishlists, List<WishList> juniorsWishlists)
    {
        var random = new Random();
        var shuffledJuniors = juniors.OrderBy(_ => random.Next()).ToList();
        var shuffledTeamLeads = teamLeads.OrderBy(_ => random.Next()).ToList();

        var teams = shuffledTeamLeads
            .Zip(shuffledJuniors, (teamLead, junior) => new Team(teamLead, junior))
            .ToList();

        return teams;
    }
}