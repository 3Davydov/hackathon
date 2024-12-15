using hackathon.contracts;

namespace hackathon.strategy;

public class TeamLeadsHateTheirJuniorsStrategy : ITeamBuildingStrategy
{
    public List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors,
        List<WishList> teamLeadsWishlists, List<WishList> juniorsWishlists)
    {
        var teams = new List<Team>();
        
        var juniorList = juniors.ToList();
        var teamLeadList = teamLeads.ToList();

        /* An example of a strategy that distributes junes in reverse according to their lists */
        for (var i = 0; i < teamLeadList.Count; i++)
        {
            var teamLead = teamLeadList[i];
            var reverseJunior = juniorList[teamLeadList.Count - 1 - i];
            teams.Add(new Team(teamLead, reverseJunior));
        }
        
        return teams;
    }
}