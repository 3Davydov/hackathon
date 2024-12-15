namespace hackathon.contracts;

public interface ITeamBuildingStrategy
{
    List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors, List<WishList> teamLeadsWishlists,
        List<WishList> juniorsWishlists);
}