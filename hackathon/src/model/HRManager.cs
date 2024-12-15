using hackathon.contracts;
using hackathon.strategy;

namespace hackathon.model;

public class HRManager
{
    private readonly ITeamBuildingStrategy _teamBuildingStrategy;

    public HRManager(ITeamBuildingStrategy teamBuildingStrategy)
    {
        _teamBuildingStrategy = teamBuildingStrategy;
    }

    public List<Team> CreateTeams(List<Employee> teamLeads, List<Employee> juniors, 
                                  List<WishList> teamLeadsWishlists, List<WishList> juniorsWishlists)
    {
        return _teamBuildingStrategy.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
    }
}